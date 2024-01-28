using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainButtonAnim : MonoBehaviour
{
    UnityAction clickAction;
    [SerializeField] Animator buttonAnimator;
    // Start is called before the first frame update
    private string[] animState = new[] { "IDLE","Select" };
    void Start()
    {
        buttonAnimator.Play(animState[0],-1,0);
    }

    public void OnHover(BaseEventData data)
    {
        buttonAnimator.Play(animState[1],-1,0);
    }
    public void OnExit(BaseEventData data)
    {
        buttonAnimator.Play(animState[0],-1,0);
    }
    public void OnPointerClick(BaseEventData eventData)
    {
        buttonAnimator.Play(animState[0],-1,0);
        clickAction?.Invoke();
        buttonAnimator.Rebind();
        buttonAnimator.Update(0f);
    }
    public void AddListener(UnityAction action)
    {
        
        clickAction += action;
    }
}
