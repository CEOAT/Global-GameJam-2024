using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

public class PlayerFire : MonoBehaviour
{
    public Camera cam;
    [SerializeField] float playerRange = 1;
    Collider2D[] detectAnts;

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);

            if(KillStreakManager.Inst.isUseKillStreak)
            {
                KillStreakManager.Inst.FireKillStreak(worldPoint);
                return;
            }

            detectAnts = Physics2D.OverlapCircleAll(worldPoint, playerRange);
            foreach (Collider2D ant in detectAnts)
            {
                if (ant.GetComponent<Ant>() != null)
                {
                    ant.transform.gameObject.GetComponent<Ant>().TakeDamage(1f);
                }
            }
        }
    }
}
