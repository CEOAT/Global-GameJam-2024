using System.Collections;
using System.Collections.Generic;
using GGJ2024;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    public void SetDamage(float damageWeapon)
    {
        damage = damageWeapon;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Ant" && other.isTrigger)
        {
            GetComponent<Ant>().TakeDamage(damage);
        }
    }
}
