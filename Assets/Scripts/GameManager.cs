using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2024;

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
        AntSpawner.Instance.EnableSwarm();
        StartCoroutine(ShowUIGameOver());

    }

    IEnumerator ShowUIGameOver()
    {
        yield return new WaitForSeconds(5f);
        UIGameover.OnGameover();
    }
}
