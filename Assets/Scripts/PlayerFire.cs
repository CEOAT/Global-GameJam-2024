using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

public class PlayerFire : MonoBehaviour
{
    public Camera cam;
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    [SerializeField] float playerRange = 1;

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
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, playerRange);
            if(hit)
            {
                if (hit.collider.tag == "Ant")
                {
                    hit.transform.gameObject.GetComponent<Ant>().TakeDamage(1f);
                    KillStreakManager.Inst.AddKillCount();
                    print(hit.collider.gameObject.name);
                }
            }
            
        }
    }
}
