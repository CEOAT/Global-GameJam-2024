using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameover : MonoBehaviour
{
    [SerializeField] TMP_Text gameoverText;
    [SerializeField] Button retryButton;
    [SerializeField] Button mainmenuButton;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        retryButton.onClick.AddListener(OnClickRetry);
        mainmenuButton.onClick.AddListener(OnClickMainmenu);
    }

    public void OnGameover()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    void OnClickRetry()
    {
        SceneManager.LoadScene(1);
    }

    void OnClickMainmenu()
    {
        SceneManager.LoadScene(2);
    }
}
