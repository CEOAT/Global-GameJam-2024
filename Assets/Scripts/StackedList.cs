using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    [Serializable]
    public class StackedList<T>
    {
        public List<T> items = new List<T>();

        public void Push(T item)
        {
            items.Add(item);
        }

        public bool TryPop(out T result)
        {
            result = default;
            if (items.Count > 0)
            {
                result = Pop();
                return true;
            }

            return false;
        }

        public T[] PopAll()
        {
            var results = items.ToArray();
            items.Clear();
            return results;
        }

        public T Pop()
        {
            T temp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return temp;
        }

        public void Remove(T target)
        {
            items.Remove(target);
        }

        public int Count => items.Count;

        public bool Contains(T ant)
        {
            return items.Contains(ant);
        }
    }

    [Serializable]
    public class ChancePool
    {
        public int chanceRate;
        public int poolSize;
        [ReadOnly] public List<bool> boolList = new List<bool>();

        public ChancePool(int chanceRate, int poolSize)
        {
            this.chanceRate = chanceRate;
            this.poolSize = poolSize;
            RebuildPool();
        }

        public bool Get()
        {
            if (boolList.Count == 0)
                RebuildPool();

            var result = boolList.FirstOrDefault();
            boolList.RemoveAt(0);
            return result;
        }

        void RebuildPool()
        {
            boolList.Clear();
            for (int i = 0; i < poolSize; i++)
                boolList.Add(i < chanceRate);
            
            ShuffleList(boolList);
        }

        void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                // Generate a random index between 0 and i (inclusive)
                int randomIndex = Random.Range(0, i + 1);
                // Swap elements at randomIndex and i
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}