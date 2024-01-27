using System.Collections;
using System.Collections.Generic;
using GGJ2024;
using UnityEngine;

public class AntTarget : MonoBehaviour
{
    [SerializeField] int targetHitpoint;
    

    void TakeDamage(int damage)
    {
        if(GameManager.Inst.isGameOver)
            return;

        targetHitpoint -= damage;

        if(targetHitpoint <= 0)
        {
            GameManager.Inst.OnGameover();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(GameManager.Inst.isGameOver)
            return;

        if(other.tag == "Ant" && other.isTrigger)
        {
            TakeDamage(other.GetComponent<Ant>().damage);
            other.GetComponent<Ant>().TakeDamage(100f,false);
        }
    }
}
