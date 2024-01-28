using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIKillStreak : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Image progress;

    public void SetKillStreakUI(int KillTarget,int currentKill)
    {
        text.text = $"Kill {KillTarget} Ants ({currentKill}/{KillTarget})";
        progress.fillAmount = (float)currentKill/KillTarget;
    }
}
