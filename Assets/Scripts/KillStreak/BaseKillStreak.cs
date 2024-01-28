using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseKillStreak : MonoBehaviour
{
    public int maxAmmo;
    protected int currentAmmo;
    public int damage;
    public float fireRate;
    protected float tempTime;
    public Sprite logo;

    public Action onOutOfAmmo; 

    public void Initilize()
    {
        currentAmmo = maxAmmo;
    }
    public void Fire(Vector2 mousePosition)
    {
        if(tempTime > 0)
            return;
        
        OnFire(mousePosition);

        tempTime = fireRate;
        currentAmmo--;

        if(currentAmmo <= 0)
            KillStreakManager.Inst.OnOutOfAmmo();
    }

    public abstract void OnFire(Vector2 mousePosition);

    protected virtual void Update() 
    {
        tempTime -= Time.deltaTime;
    }
}
