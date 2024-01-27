using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    public static KillStreakManager Inst;

    public int killCount = 0;
    public List<BaseKillStreak> killStreakConfig = new List<BaseKillStreak>();
    public AnimationCurve killProgression;
    public int killMultiply;
    public int killStreakCount;
    public bool isUseKillStreak = false;

    public BaseKillStreak currentKillStreak;

    public void Awake()
    {
        Inst = this;
    }

    public void AddKillCount()
    {
        killCount++;

        ReachKillStreak();
    }

    void ReachKillStreak()
    {
        int target = (int)killProgression.Evaluate(killStreakCount) * killMultiply;
        if(killCount >= target)
        {
            currentKillStreak = killStreakConfig[UnityEngine.Random.Range(0,killStreakConfig.Count)];
            isUseKillStreak = true;
            killCount = 0;
            currentKillStreak.Initilize();
        }
    }

    public void FireKillStreak(Vector2 mousePosition)
    {
        currentKillStreak.Fire(mousePosition);
    }

    public void OnOutOfAmmo()
    {
        currentKillStreak = null;
        isUseKillStreak = false;
    }
}
