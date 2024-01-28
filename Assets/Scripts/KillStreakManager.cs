using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Kill Streak Logo")]
    [SerializeField] private GameObject logoGroupObject;
    [SerializeField] private Image logoImage;

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
            ShowKillStreakIcon();
        }
    }
    private void ShowKillStreakIcon()
    {
        logoGroupObject.SetActive(true);
        logoImage.sprite = currentKillStreak.logo;
    }
    private void HideKillSterakIcon()
    {
        logoGroupObject.SetActive(false);
    }

    public void FireKillStreak(Vector2 mousePosition)
    {
        currentKillStreak.Fire(mousePosition);
    }

    public void OnOutOfAmmo()
    {
        currentKillStreak = null;
        isUseKillStreak = false;
        Invoke("HideKillSterakIcon", 2f);
    }
}
