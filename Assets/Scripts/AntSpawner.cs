using UnityEngine;

namespace GGJ2024
{
    public class AntSpawner : Spawner<Ant>
    {
        [SerializeField] SwarmController swarmController;
        protected override void OnRetrieve(Ant obj)
        {
            swarmController.Register(obj);
            obj.OnDie += ()=> Release(obj);
            obj.Initialize();
        }

        protected override void OnRelease(Ant obj)
        {
            swarmController.Unregister(obj);
            obj.DeInitialize();
        }

        protected override void OnElementDestroy(Ant obj)
        {
            swarmController.Unregister(obj);
        }
    }
}