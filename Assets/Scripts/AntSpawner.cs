using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GGJ2024
{
    public class AntSpawner : Spawner<Ant>
    {
        [SerializeField] Transform spawnPosition;
        [SerializeField] SwarmController swarmController;

        [Header("Attacking")] 
        [SerializeField] Transform attackTarget;
        [Range(0, 1f)] [SerializeField] float attackCountPercentage = 0.5f;
        [SerializeField] int maxOrderChangePerFrame = 3;

        [Header("Populations")] 
        [SerializeField] StackedList<Ant> freeRoamStack = new StackedList<Ant>();
        [SerializeField] StackedList<Ant> attackingStack = new StackedList<Ant>();
        [SerializeField] StackedList<Ant> busyStack = new StackedList<Ant>();

        [ShowInInspector]
        int TotalActiveCount
        {
            get
            {
                if (Pool == null)
                    return 0;

                return Pool.CountActive;
            }
        }

        float elapsedTime;

        int CurrentAttackingCount => attackingStack.Count;
        int TargetAttackCount => Mathf.FloorToInt(attackCountPercentage * Pool.CountActive);

        void LateUpdate()
        {
            for (int i = 0; i < maxOrderChangePerFrame; i++)
            {
                if (CurrentAttackingCount < TargetAttackCount)
                {
                    if (freeRoamStack.TryPop(out var readyToAttackAnt))
                        OrderToAttack(readyToAttackAnt);
                }
                else if (CurrentAttackingCount > TargetAttackCount)
                {
                    if (attackingStack.TryPop(out var retriedAnt))
                        OrderToFreeRoam(retriedAnt);
                }
            }
        }

        void OrderToAttack(Ant readyToAttackAnt)
        {
            readyToAttackAnt.SetMovementTarget(new TransformTarget(attackTarget));
            attackingStack.Push(readyToAttackAnt);
        }

        void OrderToFreeRoam(Ant retriedAnt)
        {
            retriedAnt.SetMovementTarget(new FreeRoamPositionTarget());
            freeRoamStack.Push(retriedAnt);
        }

        protected override void OnRetrieve(Ant ant)
        {
            //swarmController.Register(ant);
            ant.transform.position = spawnPosition.position;
            ant.OnClearFinish += () => Release(ant);
            ant.Initialize();
            OrderToFreeRoam(ant);
        }

        protected override void OnRelease(Ant ant)
        {
            //swarmController.Unregister(ant);
            freeRoamStack.Remove(ant);
            attackingStack.Remove(ant);
            busyStack.Remove(ant);
            ant.DeInitialize();
        }

        protected override void OnElementDestroy(Ant ant)
        {
            //swarmController.Unregister(ant);
        }
    }

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

        T Pop()
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
    }
}