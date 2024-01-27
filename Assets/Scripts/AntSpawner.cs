using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace GGJ2024
{
    public class AntSpawner : Spawner<Ant>
    {
        [SerializeField] Transform spawnPosition;
        [SerializeField] SwarmController swarmController;

        [Header("Attacking")] 
        [SerializeField] Transform attackTarget;
        [Range(0, 1f)] 
        [SerializeField] float attackCountPercentage = 0.5f;
        [SerializeField] float angryAntSpeedMultiplier = 5f;

        [Header("Funeral")] 
        [SerializeField] ChancePool funeralChance;
        [SerializeField] float mourningDistance = 1f;

        [Header("Populations")]
        [SerializeField] StackedList<Ant> kamikazeStack = new StackedList<Ant>();
        [SerializeField] StackedList<Ant> freeRoamStack = new StackedList<Ant>();
        [SerializeField] StackedList<Ant> attackingStack = new StackedList<Ant>();
        [SerializeField] StackedList<Ant> busyStack = new StackedList<Ant>();
        [SerializeField] StackedList<Ant> funeralTargetStack = new StackedList<Ant>();

        Dictionary<Ant, Ant> OnGoingFuneralDice = new Dictionary<Ant, Ant>();

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

        int CurrentAttackingCount => attackingStack.Count + kamikazeStack.Count;
        int TargetAttackCount => Mathf.FloorToInt(attackCountPercentage * Pool.CountActive);

        void LateUpdate()
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

            if (funeralTargetStack.Count > 0)
            {
                if (freeRoamStack.TryPop(out var readyToMournAnt))
                    OrderToFuneral(readyToMournAnt);
            }
        }

        void OrderToAttack(Ant readyToAttackAnt)
        {
            readyToAttackAnt.SetMovementTarget(new TransformTarget(attackTarget), true);
            attackingStack.Push(readyToAttackAnt);
        }
        
        void OrderToKamikaze(Ant readyToAttackAnt)
        {
            readyToAttackAnt.SetSpeedMultiplier(angryAntSpeedMultiplier);
            readyToAttackAnt.SetMovementTarget(new TransformTarget(attackTarget), true);
            kamikazeStack.Push(readyToAttackAnt);
        }
        
        void OrderToFuneral(Ant readyToMournAnt)
        {
            var deadAnt = funeralTargetStack.Pop();
            OnGoingFuneralDice[deadAnt] = readyToMournAnt;

            var funeralTarget = new FuneralPositionTarget(deadAnt, mourningDistance, readyToMournAnt.transform.position);
            funeralTarget.OnBecameInvalid += () =>
            {
                CompleteFuneral(deadAnt);
                busyStack.Remove(readyToMournAnt);
                    
                //Turn mourning ant into angry ant that will keep attacking
                OrderToKamikaze(readyToMournAnt);
            };
            
            funeralTarget.OnReach += ()=>
            {
                CompleteFuneral(deadAnt);
            };

            readyToMournAnt.Say("No! Patrick!!!", 1f);
            readyToMournAnt.SetMovementTarget(funeralTarget, true);
            busyStack.Push(readyToMournAnt);
        }

        void CompleteFuneral(Ant deadAnt)
        {
            if (!OnGoingFuneralDice.TryGetValue(deadAnt, out var mourningAnt))
                return;
            
            mourningAnt.DOShake(1f);
            mourningAnt.Say("No!!!!!!", 1f);
            OnGoingFuneralDice.Remove(deadAnt);
        }

        void OrderToFreeRoam(Ant retriedAnt)
        {
            retriedAnt.SetMovementTarget(new FreeRoamPositionTarget(), true);
            freeRoamStack.Push(retriedAnt);
        }

        protected override void OnRetrieve(Ant ant)
        {
            //swarmController.Register(ant);
            ant.transform.position = spawnPosition.position;
            ant.OnClearFinish += () => Release(ant);
            ant.OnDie += () => NotifyDead(ant);
            ant.Initialize();
            OrderToFreeRoam(ant);
        }

        protected override void OnRelease(Ant ant)
        {
            //swarmController.Unregister(ant);
            funeralTargetStack.Remove(ant);
            ant.DeInitialize();
        }

        void NotifyDead(Ant ant)
        {
            freeRoamStack.Remove(ant);
            attackingStack.Remove(ant);
            busyStack.Remove(ant);
            
            //No funeral for kamikaze ant
            if (kamikazeStack.Contains(ant))
            {
                kamikazeStack.Remove(ant);
                return;
            }
            
            if (!funeralChance.Get())
                return;

            funeralTargetStack.Push(ant);
        }

        protected override void OnElementDestroy(Ant ant)
        {
            //swarmController.Unregister(ant);
        }
    }
}
