using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GGJ2024
{
    public class AntSpawner : Spawner<Ant>
    {
        public static AntSpawner Instance { get; private set; }
        [SerializeField] Transform spawnPosition;
        
        [Header("Attacking")] 
        [SerializeField] Transform attackTarget;
        [Range(0, 1f)] 
        [SerializeField] float attackCountPercentage = 0.5f;
        [SerializeField] float angryAntSpeedMultiplier = 5f;

        [Header("Swarm")]
        [SerializeField] SwarmController swarmController;
       
        [Header("Funeral")]
        
        [Tooltip("Chance that dead ant will have a funeral")]
        [SerializeField] ChancePool funeralChance;
        
        [SerializeField]List<string> mourningSentencePrefixList = new List<string>();
        [SerializeField]List<string> antNameList = new List<string>();
        [SerializeField]List<string> shoutSentenceList = new List<string>();
        
        [SerializeField] float mourningDistance = 1f;
        
        StackedList<Ant> kamikazeStack = new StackedList<Ant>();
        StackedList<Ant> freeRoamStack = new StackedList<Ant>();
        StackedList<Ant> attackingStack = new StackedList<Ant>();
        StackedList<Ant> busyStack = new StackedList<Ant>();
        StackedList<Ant> funeralTargetStack = new StackedList<Ant>();

        Dictionary<Ant, Ant> OnGoingFuneralDict = new Dictionary<Ant, Ant>();

        protected override int CurrentCount => base.CurrentCount - busyStack.Count;

        float elapsedTime;

        int CurrentAttackingCount => attackingStack.Count + kamikazeStack.Count;
        int TargetAttackCount => Mathf.FloorToInt(attackCountPercentage * Pool.CountActive);

        protected override void Awake()
        {
            if (Instance == null)
                Instance = this;
            
            base.Awake();
        }

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

        [Button]
        public void EnableSwarm(int amount = 200)
        {
            if (swarmController == null)
                return;

            var antList = new List<Ant>();
            antList.AddRange(freeRoamStack.PopAll());
            var lackingAmount = amount - antList.Count;
            if (lackingAmount > 0)
            {
                for (int i = 0; i < lackingAmount; i++)
                    antList.Add(Spawn());
            }
            
            swarmController.Register(antList.ToArray());
            foreach (var ant in antList)
                busyStack.Push(ant);
        }

        [Button]
        public void ClearSwarm()
        {
            if (swarmController == null)
                return;

            var ants = swarmController.Release();
            foreach (var ant in ants)
            {
                busyStack.Remove(ant);
                OrderToFreeRoam(ant);
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
            OnGoingFuneralDict[deadAnt] = readyToMournAnt;

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

            readyToMournAnt.Say(GetRandomString(mourningSentencePrefixList) + "\n" + GetRandomString(antNameList) + "!", 2f);
            readyToMournAnt.SetMovementTarget(funeralTarget, true);
            busyStack.Push(readyToMournAnt);
        }

        string GetRandomString(List<string> list)
        {
            if (list.Count == 0)
                return string.Empty;
            
            return list[Random.Range(0, list.Count)];
        }

        void CompleteFuneral(Ant deadAnt)
        {
            if (!OnGoingFuneralDict.TryGetValue(deadAnt, out var mourningAnt))
                return;
            
            mourningAnt.DOShake(1f);
            mourningAnt.Say(GetRandomString(shoutSentenceList), 1f);
            OnGoingFuneralDict.Remove(deadAnt);
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
            if (swarmController)
                swarmController.Unregister(ant);
            
            freeRoamStack.Remove(ant);
            attackingStack.Remove(ant);
            busyStack.Remove(ant);
            kamikazeStack.Remove(ant);
        }

        public void TryStartFuneral(Ant ant)
        {
            if (OnGoingFuneralDict.ContainsKey(ant))
                return;

            //No funeral for kamikaze ant
            if (kamikazeStack.Contains(ant))
                return;

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
