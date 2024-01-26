using UnityEngine;

namespace GGJ2024
{
    public class MovementSpawner : Spawner<Movement>
    {
        [SerializeField] SwarmController swarmController;
        protected override void OnRetrieve(Movement obj)
        {
            swarmController.Register(obj);
        }

        protected override void OnRelease(Movement obj)
        {
            swarmController.Unregister(obj);
        }

        protected override void OnElementDestroy(Movement obj)
        {
            swarmController.Unregister(obj);
        }
    }
}