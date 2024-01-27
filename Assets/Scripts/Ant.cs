using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public class Ant : MonoBehaviour
    {
        [Header("Health")] 
        [SerializeField] bool isMoving;
        [SerializeField] float maxHealth = 1;
        [SerializeField] float currentHealth =1;
        [SerializeField] AntState currentState;
        
        [Header("Movement")]
        [SerializeField] float currentSpeed = 1f;
        [SerializeField] float minSpeed = 1f;
        [SerializeField] float maxSpeed = 5f;

        [Header("Clean Up")] 
        [SerializeField] float delayBeforeCleanUp = 1f;
        [SerializeField] ParticleSystem decalParticle;

        public event Action<float> OnHealthChange;
        public event Action OnDie;
        public event Action OnClearFinish;
        bool isInitialized;
        Vector3 targetPosition;
        
        public void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;
            gameObject.SetActive(true);
            SetHealth(maxHealth);
            currentState = AntState.Alive;
            isMoving = true;
            SetNextposition();
        }

        public void DeInitialize()
        {
            isInitialized = false;
            OnHealthChange = null;
            OnClearFinish = null;
            OnDie = null;
            gameObject.SetActive(false);
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
                currentState = AntState.Dead;
                isMoving = false;
                KillStreakManager.Inst.AddKillCount();
                OnDie?.Invoke();
                Clear();
            }
        }
        
        public void Clear()
        {
            if (currentState == AntState.Cleared)
                return;
            
            currentState = AntState.Cleared;
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
        }

        public void SetTargetPosition(Vector3 pos)
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            targetPosition = pos;
            RotateTowardsDirection();
        }

        public void SetNextposition()
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            SetTargetPosition(GetRandomPositionInScreen());
            RotateTowardsDirection();
        }

        void Update()
        {
            if (!isMoving)
                return;
            
            if(transform.position == targetPosition)
                SetNextposition();

            transform.position = Vector3.MoveTowards(transform.position, targetPosition,Time.deltaTime * currentSpeed);
        }

        Vector3 GetRandomPositionInScreen()
        {
            float screenWidth = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
            float screenHeight = Camera.main.orthographicSize * 2;

            float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
            float randomY = Random.Range(-screenHeight / 2, screenHeight / 2);

            return new Vector3(randomX, randomY, 0);
        }

        void RotateTowardsDirection()
        {
            transform.right = targetPosition - transform.position;
        }
    }

    public enum AntState
    {
        Alive, Dead, Cleared
    }
}

