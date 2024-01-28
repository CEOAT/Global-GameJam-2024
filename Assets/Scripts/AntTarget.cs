using System.Collections;
using System.Collections.Generic;
using GGJ2024;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class AntTarget : MonoBehaviour
{
    [SerializeField] int maxHitpoint;
    [SerializeField] Image healthImage;
    int targetHitpoint;
    
    void Start() 
    {
        targetHitpoint = maxHitpoint;
    }

    void TakeDamage(int damage)
    {
        if(GameManager.Inst.isGameOver)
            return;

        targetHitpoint -= damage;
        SetHealthUI();

        if(targetHitpoint <= 0)
        {
            GameManager.Inst.OnGameover();
        }
    }

    void SetHealthUI()
    {
        healthImage.fillAmount = (float)targetHitpoint / maxHitpoint;
    }

    public void Heal(int healValue)
    {
        targetHitpoint += healValue;

        if(targetHitpoint > maxHitpoint)
            targetHitpoint = maxHitpoint;

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(GameManager.Inst.isGameOver)
            return;

        if(other.tag == "Entity" && other.isTrigger)
        {
            TakeDamage(other.GetComponent<Ant>().damage);
            other.GetComponent<Ant>().TakeDamage(100f,false);
        }
    }
}
