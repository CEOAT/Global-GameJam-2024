using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

public class PlayerFire : MonoBehaviour
{
    public Camera cam;
    [SerializeField] GameObject cursor;
    [SerializeField] float playerRange = 1;
    Collider2D[] detectAnts;

    bool isUseKillStreak = false;
    
    void Update()
    {
        cursor.transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        //cursor.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        Fire();
    }

    void Fire()
    {
        if(isUseKillStreak)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, playerRange);
            detectAnts = Physics2D.OverlapCircleAll(worldPoint, playerRange);
            foreach (Collider2D ant in detectAnts)
            {
                if (ant.GetComponent<Ant>() != null)
                {
                    ant.transform.gameObject.GetComponent<Ant>().TakeDamage(1f);
                    KillStreakManager.Inst.AddKillCount();
                }
            }
        }
    }
}
