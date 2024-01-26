using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] T prefab;
        [SerializeField] int startUpCount = 100;
        [SerializeField] int defaultCapacity = 10;
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

        protected abstract void OnRetrieve(T obj);
        protected abstract void OnRelease(T obj);
        protected abstract void OnElementDestroy(T obj);

        void Update()
        {
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

        T Spawn()
        {
            return pool.Get();
        }
    }
}