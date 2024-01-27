using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

public class PlayerFire : MonoBehaviour
{
    public Camera cam;
    [SerializeField] float playerRange = 1;
    Collider2D[] detectAnts;

    bool isUseKillStreak = false;
    
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(isUseKillStreak)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            detectAnts = Physics2D.OverlapCircleAll(worldPoint, playerRange);
            foreach (Collider2D ant in detectAnts)
            {
                print(ant.gameObject.name);
                if (ant.GetComponent<Ant>() != null)
                {
                    ant.gameObject.GetComponent<Ant>().TakeDamage(1f);
                }
            }


        }
    }
}
