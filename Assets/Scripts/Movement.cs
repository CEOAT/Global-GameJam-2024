using UnityEngine;

namespace GGJ2024
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] float currentSpeed = 1f;
        [SerializeField] float minSpeed = 1f;
        [SerializeField] float maxSpeed = 5f;
        [SerializeField] Vector3 targetPosition;

        void Awake()
        {
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            targetPosition = transform.position;
        }

        public void SetTargetPosition(Vector3 pos)
        {
            targetPosition = pos;
        }

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,Time.deltaTime * currentSpeed);

        }
    }
}

