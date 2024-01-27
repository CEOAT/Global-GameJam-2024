using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseKillStreak : MonoBehaviour
{
    public int maxAmmo;
    public int currentAmmo;
    public int damage;
    public float fireRate;
    public float tempTime;

    public Action onOutOfAmmo; 

    public void Initilize()
    {
        maxAmmo = currentAmmo;
    }
    public virtual void Fire(Vector2 mousePosition)
    {
        if(tempTime < fireRate)
            return;
        
        tempTime = 0;
        currentAmmo--;

        if(currentAmmo <= 0)
            KillStreakManager.Inst.OnOutOfAmmo();
    }

    void Update() 
    {
        tempTime += Time.deltaTime;
    }
}
