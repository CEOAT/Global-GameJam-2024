using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum MainMenuStage
{
    MainMenu,
    HowToPlay
    //เพื่อ Custom ตาม Panel ต่างๆ 
}

public class MainMenuController : MonoBehaviour
{
    MainMenuStage mainMenuStage = MainMenuStage.MainMenu;

    [SerializeField] CanvasGroup mainMenuGroup;
    [SerializeField] CanvasGroup howToPlayGroup;
    [SerializeField] CanvasGroup VegetaGround;
    [SerializeField] CanvasGroup CreditGroup;

    [Header("MainMenuPanel")]
    [SerializeField] MainButtonAnim mainMenu_startButton;
    [SerializeField] MainButtonAnim mainMenu_howToPlayButton;
    [SerializeField] MainButtonAnim mainMenu_Credit;

    [Header("HowToPlayPanel")]
    [SerializeField] MainButtonAnim howToPlay_backButton;
    [SerializeField] MainButtonAnim Credit_backButton;

    private void Start()
    {
        AddListener();
        EnableVegeta(true);
        EnableHowToPlay(false);
        EnableCredit(false);
    }

    void AddListener()
    {
        mainMenu_startButton.AddListener(OnStart);
        mainMenu_howToPlayButton.AddListener(OnHowToPlay);
        mainMenu_Credit.AddListener(OnCredit);

        howToPlay_backButton.AddListener(OnBack);
        Credit_backButton.AddListener(OnBackCredit);
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

    public void OnBackCredit()
    {
        mainMenuStage = MainMenuStage.MainMenu;
        EnableMainMenu(true);
        EnableCredit(false);
    }

    public void OnHowToPlay()
    {
        mainMenuStage = MainMenuStage.HowToPlay;
        EnableMainMenu(false);
        EnableHowToPlay(true);
    }
    public void OnCredit()
    {
        mainMenuStage = MainMenuStage.MainMenu;
        EnableMainMenu(false);
        EnableCredit(true);
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
    void EnableCredit(bool enable)
    {
        if (enable)
        {
            CreditGroup.alpha = 1;
            CreditGroup.blocksRaycasts = true;
            CreditGroup.interactable = true;
        }
        else
        {
            CreditGroup.alpha = 0;
            CreditGroup.blocksRaycasts = false;
            CreditGroup.interactable = false;

        }
    }
    void EnableVegeta(bool enable)
    {
        if (enable)
        {
            VegetaGround.alpha = 1;
            VegetaGround.blocksRaycasts = true;
            VegetaGround.interactable = true;
        }
        else
        {
            VegetaGround.alpha = 0;
            VegetaGround.blocksRaycasts = false;
            VegetaGround.interactable = false;

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
    
    public void pressClickToContinue(BaseEventData data)
    {
        DOTween.To(() => VegetaGround.alpha, x => VegetaGround.alpha = x, 0f, 1.5f)
            .OnComplete(() => { EnableMainMenu(true); EnableVegeta(false);});
    }
}
