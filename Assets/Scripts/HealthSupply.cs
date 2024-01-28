using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSupply : MonoBehaviour, IEntity
{
    public int healValue;
    public Action<int> onHeal;

    public void TakeDamage(float damage, bool addKillCount = true)
    {
        Die(false);
    }

    public void Die(bool addKillCount)
    {
        onHeal?.Invoke(healValue);
        Destroy(this.gameObject);
    }
}
