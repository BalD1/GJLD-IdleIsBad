using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("UIManager instance not found");

            return instance;
        }
    }

    [SerializeField] private GameObject levelsWindow;
    [SerializeField] private GameObject pauseWindow;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject gOWindow;

    public void WindowManager(GameManager.GameState gameState)
    {
        GameObject windowToActivate = null;
        switch(gameState)
        {
            case GameManager.GameState.MainMenu:
                break;
            case GameManager.GameState.InGame:

                break;
            case GameManager.GameState.Pause:
                windowToActivate = pauseWindow;
                break;
            case GameManager.GameState.GameOver:
                windowToActivate = gOWindow;
                break;
            case GameManager.GameState.Win:
                windowToActivate = winWindow;
                break;
        }
        if(pauseWindow != null)
            pauseWindow.SetActive(false);
        if(winWindow != null)
            winWindow.SetActive(false);
        if(gOWindow != null)
            gOWindow.SetActive(false);
        if(windowToActivate != null)
            windowToActivate.SetActive(true);
        
    }

    public void OnButtonPress(string button)
    {
        switch(button)
        {
            case "Play":
                levelsWindow.SetActive(!levelsWindow.activeSelf);
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Continue":
                GameManager.Instance.StateOfGame = GameManager.GameState.InGame;
                break;
            case "MainMenu":
                GameManager.Instance.StateOfGame = GameManager.GameState.MainMenu;
                break;
            case "Retry":
                GameManager.Instance.ReloadMap();
                GameManager.Instance.StateOfGame = GameManager.GameState.InGame;
                break;
            case "NextLevel":
                GameManager.Instance.NextMap();
                GameManager.Instance.StateOfGame = GameManager.GameState.InGame;
                break;
            default:
                Debug.LogError(button + " button not found in switch statement.");
                break;
        }
    }

    public void LoadLevel(int level)
    {
        DataKeep.mapToLoad = level;
        GameManager.Instance.StateOfGame = GameManager.GameState.InGame;
    }

    private void Awake()
    {
        instance = this;
    }

}
