using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum MainMenuStage
{
    MainMenu,
    HowToPlay
}

public class MainMenuController : MonoBehaviour
{
    MainMenuStage mainMenuStage = MainMenuStage.MainMenu;

    [SerializeField] CanvasGroup mainMenuGroup;
    [SerializeField] CanvasGroup howToPlayGroup;

    [Header("MainMenuPanel")]
    [SerializeField] Button mainMenu_startButton;
    [SerializeField] Button mainMenu_howToPlayButton;

    [Header("HowToPlayPanel")]
    [SerializeField] Button howToPlay_backButton;

    private void Start()
    {
        AddListener();
        EnableMainMenu(true);
        EnableHowToPlay(false);
    }

    void AddListener()
    {
        mainMenu_startButton.onClick.AddListener(OnStart);
        mainMenu_howToPlayButton.onClick.AddListener(OnHowToPlay);

        howToPlay_backButton.onClick.AddListener(OnBack);
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnBack()
    {
        mainMenuStage = MainMenuStage.MainMenu;
        EnableMainMenu(true);
        EnableHowToPlay(false);
    }

    public void OnHowToPlay()
    {
        mainMenuStage = MainMenuStage.HowToPlay;
        EnableMainMenu(false);
        EnableHowToPlay(true);
    }

    public void OnQuit()
    {
        if (mainMenuStage != MainMenuStage.MainMenu)
            return;
        Application.Quit();
    }

    void EnableMainMenu(bool enable)
    {
        if (enable)
        {
            mainMenuGroup.alpha = 1;
            mainMenuGroup.blocksRaycasts = true;
            mainMenuGroup.interactable = true;
        }
        else
        {
            mainMenuGroup.alpha = 0;
            mainMenuGroup.blocksRaycasts = false;
            mainMenuGroup.interactable = false;

        }
    }

    void EnableHowToPlay(bool enable)
    {
        if (enable)
        {
            howToPlayGroup.alpha = 1;
            howToPlayGroup.blocksRaycasts = true;
            howToPlayGroup.interactable = true;
        }
        else
        {
            howToPlayGroup.alpha = 0;
            howToPlayGroup.blocksRaycasts = false;
            howToPlayGroup.interactable = false;
        }
    }
}
