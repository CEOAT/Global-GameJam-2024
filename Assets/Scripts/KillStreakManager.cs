using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    public UIKillStreak UIKillStreak;
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

    private void Update() {
        UIKillStreak.SetKillStreakUI(GetTargetKill(),killCount);
    }

    public void AddKillCount()
    {
        if(currentKillStreak)
            return;

        killCount++;
        ReachKillStreak();
    }   

    int GetTargetKill()
    {
        int target = (int)killProgression.Evaluate(killStreakCount) * killMultiply;
        return target;
    }

    [Button]
    void GetKillStreak()
    {
        killCount += GetTargetKill();
        ReachKillStreak();
    }

    void ReachKillStreak()
    {
        if(killCount >= GetTargetKill())
        {
            currentKillStreak = killStreakConfig[UnityEngine.Random.Range(0,killStreakConfig.Count)];
            isUseKillStreak = true;
            killCount = 0;
            killStreakCount++;
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
