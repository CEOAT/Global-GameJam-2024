﻿using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public class Ant : MonoBehaviour , IEntity
    {
        [Header("Health")] 
        [SerializeField] bool isMoving;
        [SerializeField] float maxHealth = 1;
        [SerializeField] float currentHealth =1;

        public AntState CurrentState => currentState;
        [SerializeField] AntState currentState;

        [Header("Movement")] 
        [SerializeField] float speedMultiplier = 1f;
        [SerializeField] float currentSpeed = 1f;
        [SerializeField] float minSpeed = 1f;
        [SerializeField] float maxSpeed = 5f;

        [Header("Clean Up")] 
        [SerializeField] float delayBeforeCleanUp = 1f;
        [SerializeField] ParticleSystem decalParticle;

        [Header("Talking")] 
        [SerializeField] AntMessageBalloon messageBalloon;
        
        [Header("VFX")] 
        [SerializeField] LocalAntVFX antVFX;

        [Header("Damage")]
        public int damage = 1;

        [Header("Sound")]
        [SerializeField] AudioClip dieSound;
        
        public event Action<float> OnHealthChange;
        public event Action OnDie;
        public event Action OnClearFinish;
        
        bool isInitialized;
        Vector3 targetPosition;
        AntMovementTarget movementTarget;
        Transform cachedTransform;
        AudioSource audioSource;
        public float DelayBeforeCleanUp
        {
            get => delayBeforeCleanUp;
            set => delayBeforeCleanUp = value;
        }

        public bool IsCannotChangeTarget { get; private set; }

        void Awake()
        {
            cachedTransform = transform;

            audioSource = GetComponent<AudioSource>();
        }

        public void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;
            gameObject.SetActive(true);
            SetHealth(maxHealth);
            currentState = AntState.Alive;
            isMoving = true;
            targetPosition = cachedTransform.position;
            antVFX.ClearTemp();
        }

        public void DeInitialize()
        {
            speedMultiplier = 1f;
            IsCannotChangeTarget = false;
            isInitialized = false;
            OnHealthChange = null;
            OnClearFinish = null;
            OnDie = null;
            movementTarget = null;
            gameObject.SetActive(false);
        }

        public void MarkAsCannotChangeTarget()
        {
            IsCannotChangeTarget = true;
        }

        [ContextMenu(nameof(Kill))]
        public void Kill()
        {
            SetHealth(0);
        }

        public void TakeDamage(float damage,bool addKillCount = true)
        {
            SetHealth(currentHealth - damage,addKillCount);
        }

        void SetHealth(float value,bool addKillCount = true)
        {
            if (currentState != AntState.Alive)
                return;

            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChange?.Invoke(currentHealth);
            if (currentHealth <= 0)
            {
                Die(addKillCount);
            }
        }

        public void Die(bool addKillCount)
        {
            HideBalloon();
            CancelShake();
            currentState = AntState.Dead;
            isMoving = false;
            if(addKillCount)
                KillStreakManager.Inst.AddKillCount();
            OnDie?.Invoke();
            antVFX.ExplodeKill();
            audioSource.PlayOneShot(dieSound);
            
            Clear();
        }

        public void Clear()
        {
            if (currentState == AntState.Clearing)
                return;
            
            if (currentState == AntState.Cleared)
                return;
            
            currentState = AntState.Clearing;
            StartCoroutine(ClearRoutine());
        }

        IEnumerator ClearRoutine()
        {
            if (decalParticle)
                decalParticle.Play();
            
            yield return new WaitForSeconds(delayBeforeCleanUp);
            if (decalParticle)
                decalParticle.Stop();
            
            OnClearFinish?.Invoke();
            currentState = AntState.Cleared;
        }

        public void SetMovementTarget(AntMovementTarget target, bool isCancelPendingTarget)
        {
            if (IsCannotChangeTarget)
                return;
            
            if (isCancelPendingTarget)
                targetPosition = cachedTransform.position;
            
            movementTarget = target;
        }

        void SetTargetPosition(Vector3 pos)
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            targetPosition = pos;
        }

        void Update()
        {
            if (!isMoving)
                return;

            if (movementTarget == null)
                return;

            if (!movementTarget.CheckIsValid())
                return;

            var currentPos = cachedTransform.position;
            if (currentPos == targetPosition)
            {
                currentSpeed = Random.Range(minSpeed, maxSpeed);
                movementTarget.CheckIsReach(cachedTransform.position);
                SetTargetPosition(movementTarget.GetPosition());
            }
            else
            {
                var vector = targetPosition - currentPos;
                vector.z = 0;
                cachedTransform.right = vector;
            }

            cachedTransform.position = Vector3.MoveTowards(currentPos, targetPosition,Time.deltaTime * currentSpeed * speedMultiplier);
        }

        public void Say(string message, float duration)
        {
            messageBalloon.Play(message, duration);
        }

        public void HideBalloon()
        {
            messageBalloon.Hide();
        }

        public void DOShake(float duration)
        {
            CancelShake();
            cachedTransform.DOShakePosition(duration);
        }

        void CancelShake()
        {
            cachedTransform.DOKill(true);
        }

        public void SetSpeedMultiplier(float value)
        {
            speedMultiplier = value;
        }
    }

    public enum AntState
    {
        Alive, Dead, Clearing, Cleared
    }
}

