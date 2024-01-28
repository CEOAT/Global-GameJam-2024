using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        tempTime -= Time.deltaTime;

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
        if(GameManager.Inst.isGameOver)
            return;
            
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            if(KillStreakManager.Inst.isUseKillStreak)
            {
                KillStreakManager.Inst.FireKillStreak(worldPoint);
                return;
            }

            if(tempTime > 0)
                return;
            
            tempTime = weaponList[weaponIndex].fireRate;

            detectAnts = Physics2D.OverlapCircleAll(worldPoint, playerRange);
            var ants = detectAnts
                .Select(hit => hit.GetComponent<IEntity>())
                .Where(a => a is IEntity);

            Ant oneDeadAnt = null;
            foreach (var ant in ants)
            {
                ant.TakeDamage(currentWeapon.weaponDamage);

                if(ant is Ant)
                {
                    Ant _ant = ant as Ant;
                    if (_ant.CurrentState != AntState.Alive)
                    {
                        KillStreakManager.Inst.AddKillCount();
                        oneDeadAnt = _ant;
                    }
                }
            }

            if (oneDeadAnt)
                AntSpawner.Instance.TryStartFuneral(oneDeadAnt);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, playerRange);
    }
}
