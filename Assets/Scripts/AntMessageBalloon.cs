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
            transform.GetChild(0).localScale = Vector3.zero;
            var seq = DOTween.Sequence();
            seq.Append(canvasGroup.DOFade(1f, fadeDuration));
            seq.Join(transform.GetChild(0).DOShakePosition(0.5f, 10f, 30));
            seq.Join(transform.GetChild(0).DOScale(1, 0.25f).SetEase(Ease.InOutBack));
            seq.AppendInterval(duration);
            seq.Append(canvasGroup.DOFade(0f, fadeDuration));
            seq.AppendCallback(Hide);
            seq.SetTarget(canvasGroup);
        }

        public void Hide()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0;
        }
    }
}