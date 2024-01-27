using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

public class PlayerFire : MonoBehaviour
{
    Camera cam => CameraHelper.mainCamera;
    [SerializeField] float playerRange = 1;
    [Header("Weapon")]
    [SerializeField] GameObject cursor;
    public WeaponScriptableObject currentWeapon;
    [SerializeField] int weaponIndex = 0;
    [SerializeField] List<WeaponScriptableObject> weaponList;
    private bool isAttackReady = true;
    float tempTime;

    Collider2D[] detectAnts;
   
    
    void Update()
    {
        tempTime += Time.deltaTime;

        SwitchWeapon();
        CursorMove();
        Fire();
    }

    void SwitchWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
           weaponIndex++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
           weaponIndex--;
        }
        if (weaponIndex > weaponList.Count-1)
        {
           weaponIndex = 0;
        }
        else if (weaponIndex < 0)
        {
           weaponIndex = weaponList.Count-1;
        }

        cursor.GetComponent<SpriteRenderer>().sprite = weaponList[weaponIndex].cursorSprite;
        currentWeapon = weaponList[weaponIndex];
        playerRange = currentWeapon.weaponRange;
    }

    void CursorMove()
    {
        cursor.transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
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

            if(tempTime < weaponList[weaponIndex].fireRate)
                return;

            //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, playerRange);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, currentWeapon.weaponRange);
            detectAnts = Physics2D.OverlapCircleAll(worldPoint, currentWeapon.weaponRange);
            foreach (Collider2D ant in detectAnts)
            {
                if (ant.GetComponent<Ant>() != null)
                {
                    //ant.transform.gameObject.GetComponent<Ant>().TakeDamage(1f);
                    ant.transform.gameObject.GetComponent<Ant>().TakeDamage(currentWeapon.weaponDamage);
                }
            }

            tempTime = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, playerRange);
    }
}
