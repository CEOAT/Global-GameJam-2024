using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GGJ2024
{
    public abstract class AntMovementTarget
    {
        public abstract Vector3 GetPosition();

        public bool CheckIsValid()
        {
            if (!IsValid())
            {
                OnBecameInvalid?.Invoke();
                return false;
            }

            return true;
        }

        public void CheckIsReach(Vector3 position)
        {
            if (IsReachedOnce)
                return;
            
            if (position != GetPosition())
                return;
            
            IsReachedOnce = true;
            OnReach?.Invoke();
        }
        
        protected abstract bool IsValid();
        public event Action OnBecameInvalid;
        public event Action OnReach;
        public bool IsReachedOnce;
    }

    public class TransformTarget : AntMovementTarget
    {
        public override Vector3 GetPosition() => transform.position;
        protected override bool IsValid() => transform;
        public Transform transform;

        public TransformTarget(Transform transform)
        {
            this.transform = transform;
        }
    }

    public class PositionTarget : AntMovementTarget
    {
        public override Vector3 GetPosition() => position;

        protected override bool IsValid()
        {
            return true;
        }

        public Vector3 position;

        public PositionTarget(Vector3 position)
        {
            this.position = position;
        }
    }

    public class FreeRoamPositionTarget : AntMovementTarget
    {
        public override Vector3 GetPosition()
        {
            float screenWidth = CameraHelper.mainCamera.orthographicSize * 2 * Screen.width / Screen.height;
            float screenHeight = CameraHelper.mainCamera.orthographicSize * 2;
            float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
            float randomY = Random.Range(-screenHeight / 2, screenHeight / 2);
            return new Vector3(randomX, randomY, 0);
        }

        protected override bool IsValid()
        {
            return true;
        }

        public FreeRoamPositionTarget()
        {
        }
    }

    public class FuneralPositionTarget : AntMovementTarget
    {
        public Ant deadAnt;
        public float distanceFromBody;
        public Vector3 direction;
        public override Vector3 GetPosition()
        {
            return deadAnt.transform.position - (direction * distanceFromBody);
        }

        public FuneralPositionTarget(Ant deadAnt, float distanceFromBody, Vector3 startPosition)
        {
            this.deadAnt = deadAnt;
            this.distanceFromBody = distanceFromBody;
            direction = (deadAnt.transform.position - startPosition).normalized;
        }

        protected override bool IsValid()
        {
            return deadAnt.CurrentState == AntState.Clearing;
        }
    }
}