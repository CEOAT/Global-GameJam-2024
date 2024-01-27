using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public class Ant : MonoBehaviour
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
        
        public event Action<float> OnHealthChange;
        public event Action OnDie;
        public event Action OnClearFinish;
        bool isInitialized;
        Vector3 targetPosition;
        AntMovementTarget movementTarget;
        
        public bool IsCannotChangeTarget { get; private set; }
        
        public void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;
            gameObject.SetActive(true);
            SetHealth(maxHealth);
            currentState = AntState.Alive;
            isMoving = true;
            targetPosition = transform.position;
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

        public void TakeDamage(float damage)
        {
            SetHealth(currentHealth - damage);
        }

        void SetHealth(float value)
        {
            if (currentState != AntState.Alive)
                return;

            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChange?.Invoke(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            HideBalloon();
            CancelShake();
            currentState = AntState.Dead;
            isMoving = false;
            OnDie?.Invoke();
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
                targetPosition = transform.position;
            
            movementTarget = target;
        }

        void SetTargetPosition(Vector3 pos)
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            targetPosition = pos;
            RotateTowardsDirection();
        }

        void Update()
        {
            if (!isMoving)
                return;

            if (movementTarget == null)
                return;

            if (!movementTarget.CheckIsValid())
                return;

            if (transform.position == targetPosition)
            {
                currentSpeed = Random.Range(minSpeed, maxSpeed);
                movementTarget.CheckIsReach(transform.position);
                SetTargetPosition(movementTarget.GetPosition());
            }
            else
                RotateTowardsDirection();

            transform.position = Vector3.MoveTowards(transform.position, targetPosition,Time.deltaTime * currentSpeed * speedMultiplier);
        }

        void RotateTowardsDirection()
        {
            transform.right = targetPosition - transform.position;
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
            transform.DOShakePosition(duration);
        }

        void CancelShake()
        {
            transform.DOKill(true);
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

