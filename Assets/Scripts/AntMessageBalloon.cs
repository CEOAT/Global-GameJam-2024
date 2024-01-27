using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GGJ2024
{
    public class AntMessageBalloon : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float fadeDuration = 0.25f;

        void Awake()
        {
            canvasGroup.alpha = 0;
        }

        void Update()
        {
            transform.eulerAngles = Vector3.zero;
        }

        public void Play(string message, float duration)
        {
            text.text = message;
            canvasGroup.DOKill();
            var seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(1f, fadeDuration));
            seq.AppendInterval(duration);
            seq.Append(canvasGroup.DOFade(0f, fadeDuration));
            seq.SetTarget(canvasGroup);
        }
    }
}