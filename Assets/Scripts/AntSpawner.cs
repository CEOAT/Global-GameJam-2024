using UnityEngine;

namespace GGJ2024
{
    public class AntSpawner : Spawner<Ant>
    {
        [SerializeField] SwarmController swarmController;
        protected override void OnRetrieve(Ant ant)
        {
            swarmController.Register(ant);
            ant.OnDie += ()=> Release(ant);
            ant.Initialize();
        }

        protected override void OnRelease(Ant ant)
        {
            swarmController.Unregister(ant);
            ant.DeInitialize();
        }

        protected override void OnElementDestroy(Ant ant)
        {
            swarmController.Unregister(ant);
        }
    }
}