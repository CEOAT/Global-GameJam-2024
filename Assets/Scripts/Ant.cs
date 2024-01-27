using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public class Ant : MonoBehaviour
    {
        [SerializeField] float maxHealth = 1;
        [SerializeField] float currentHealth =1;
        [SerializeField] float currentSpeed = 1f;
        [SerializeField] float minSpeed = 1f;
        [SerializeField] float maxSpeed = 5f;

        public event Action<float> OnHealthChange;
        public event Action OnDie;
        bool isInitialized;
        Vector3 targetPosition;
        
        public void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;
            gameObject.SetActive(true);
            currentHealth = maxHealth;
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            targetPosition = transform.position;
        }

        public void DeInitialize()
        {
            isInitialized = false;
            OnHealthChange = null;
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
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChange?.Invoke(currentHealth);
            if (currentHealth <= 0)
                OnDie?.Invoke();
        }

        public void SetTargetPosition(Vector3 pos)
        {
            targetPosition = pos;
        }

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,Time.deltaTime * currentSpeed);
        }
    }
}

