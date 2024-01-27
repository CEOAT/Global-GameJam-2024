using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

public class PlayerFire : MonoBehaviour
{
    public Camera cam;
    [SerializeField] float playerRange = 1;
    Collider2D[] detectAnts;
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Input.mousePosition, playerRange);
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            detectAnts = Physics2D.OverlapCircleAll(worldPoint, playerRange);

            //int hit = Physics2D.CircleCast(worldPoint, playerRange, Vector2.right, contactFilter, results);
            foreach (Collider2D ant in detectAnts)
            {
                print(ant.gameObject.name);
                if (ant.GetComponent<Ant>() != null)
                {
                    ant.gameObject.GetComponent<Ant>().DeInitialize();
                }
            }
            //if (hit.collider.tag == "Ant")
            //{
            //    hit.transform.gameObject.GetComponent<Ant>().DeInitialize();
            //    print(hit.collider.gameObject.name);
            //}
        }
    }
}
