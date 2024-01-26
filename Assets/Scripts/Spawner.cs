﻿using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] T prefab;
        [SerializeField] int startUpCount = 100;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int targetCount = 1000;
        [SerializeField] int maxSize = 1000000;
        [SerializeField] float minSpawnDelay = 0;
        [SerializeField] float maxSpawnDelay = 1f;

        float currentDelayLeft;
        ObjectPool<T> pool;
        
        void Awake()
        {
            pool = new ObjectPool<T>(
                () => Instantiate(prefab, transform, true),
                OnRetrieve,
                OnRelease,
                OnElementDestroy,
                true,
                defaultCapacity,
                maxSize
            );
        }

        void Start()
        {
            for (int i = 0; i < startUpCount; i++)
                Spawn();
        }

        protected abstract void OnRetrieve(T ant);
        protected abstract void OnRelease(T ant);
        protected abstract void OnElementDestroy(T ant);

        void Update()
        {
            if (pool.CountActive >= targetCount)
                return;
            
            if (currentDelayLeft > 0)
            {
                currentDelayLeft -= Time.deltaTime;
            }
            else
            {
                Spawn();
                currentDelayLeft = GetRandomDelay();
            }
        }

        float GetRandomDelay()
        {
            return Random.Range(minSpawnDelay, maxSpawnDelay);
        }

        public T Spawn()
        {
            return pool.Get();
        }

        public void Release(T target)
        {
            pool.Release(target);
        }
    }
}