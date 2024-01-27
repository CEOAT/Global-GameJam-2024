using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    public static KillStreakManager Inst;

    public int killCount = 0;
    public List<KillStreakConfig> killStreakConfig = new List<KillStreakConfig>();
    public AnimationCurve killProgression;
    public int killMultiply;
    public int killStreakCount;
    public int _target;

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
            killCount = 0;
            //can use killStreak
        }
    }

    private void OnDrawGizmos() {
        _target = (int)killProgression.Evaluate(killStreakCount) * killMultiply;
    }
}

[Serializable]
public class KillStreakConfig
{
    public int killCount;
    public BaseKillStreak killStreak;
}
