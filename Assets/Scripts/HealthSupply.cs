using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSupply : MonoBehaviour
{
    [SerializeField] private float healValue;
    // public player health script

    private void OnDestroy() 
    {
        Heal();
    }
    private void Heal()
    {
        // use function heal in player health script        
    }
}