using UnityEngine;

namespace GGJ2024
{
    public abstract class AntMovementTarget
    {
        public abstract Vector3 GetPosition();
        public abstract bool IsValid();
    }

    public class TransformTarget : AntMovementTarget
    {
        public override Vector3 GetPosition() => transform.position;
        public override bool IsValid() => transform;
        public Transform transform;

        public TransformTarget(Transform transform)
        {
            this.transform = transform;
        }
    }

    public class PositionTarget : AntMovementTarget
    {
        public override Vector3 GetPosition() => position;

        public override bool IsValid()
        {
            return true;
        }

        public Vector3 position;

        public PositionTarget(Vector3 position)
        {
            this.position = position;
        }
    }

    public class RandomPositionTarget : AntMovementTarget
    {
        public static Camera mainCamera
        {
            get
            {
                if (unsafeMainCamera == null)
                    unsafeMainCamera = Camera.main;

                return unsafeMainCamera;
            }
        }

        static Camera unsafeMainCamera;

        public override Vector3 GetPosition()
        {
            float screenWidth = mainCamera.orthographicSize * 2 * Screen.width / Screen.height;
            float screenHeight = mainCamera.orthographicSize * 2;
            float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
            float randomY = Random.Range(-screenHeight / 2, screenHeight / 2);
            return new Vector3(randomX, randomY, 0);
        }

        public override bool IsValid()
        {
            return true;
        }

        public RandomPositionTarget()
        {
        }
    }

    public class FuneralPositionTarget : AntMovementTarget
    {
        public Ant deadAnt;
        public override Vector3 GetPosition()
        {
            return deadAnt.transform.position;
        }

        public override bool IsValid()
        {
            return deadAnt.CurrentState == AntState.Clearing;
        }
    }
}