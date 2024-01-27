using UnityEngine;

namespace GGJ2024
{
    public class AntSpawner : Spawner<Ant>
    {
        [SerializeField] Transform spawnPosition;
        [SerializeField] Transform attackTarget;
        [SerializeField] SwarmController swarmController;

        [Range(0, 1f)]
        [SerializeField] float attackPercentage;
        
        protected override void OnRetrieve(Ant ant)
        {
            //swarmController.Register(ant);
            ant.transform.position = spawnPosition.position;
            ant.OnClearFinish += ()=> Release(ant);
            ant.Initialize();
            ant.SetMovementTarget(new RandomPositionTarget());
        }

        protected override void OnRelease(Ant ant)
        {
            //swarmController.Unregister(ant);
            ant.DeInitialize();
        }

        protected override void OnElementDestroy(Ant ant)
        {
            //swarmController.Unregister(ant);
        }
    }
}
