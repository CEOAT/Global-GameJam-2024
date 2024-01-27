using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    [SerializeField] UIGameover UIGameover;
    public bool isGameOver = false;

    void Awake() 
    {
        Inst = this;
    }
    
    public void OnGameover()
    {
        isGameOver = true;

        //SwarmController

        UIGameover.OnGameover();
    }
}
