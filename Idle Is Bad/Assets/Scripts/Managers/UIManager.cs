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

    [Header("Main Menu")]

    [SerializeField] private GameObject buttonsGrid;
    private List<GameObject> levelButtons;

    [Header("In Game")]

    [SerializeField] private GameObject levelsWindow;
    [SerializeField] private GameObject pauseWindow;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject gOWindow;


    public void WindowManager(GameManager.GameState gameState)
    {
        switch(gameState)
        {
            case GameManager.GameState.MainMenu:
                break;

            case GameManager.GameState.InGame:
                DeactivateAll();
                break;

            case GameManager.GameState.Pause:
                DeactivateAll();
                pauseWindow.SetActive(true);
                break;

            case GameManager.GameState.GameOver:
                DeactivateAll();
                gOWindow.SetActive(true);
                break;

            case GameManager.GameState.Win:
                DeactivateAll();
                winWindow.SetActive(true);

                List<GameObject> maps = GameManager.Instance.Maps;

                if(GameManager.Instance.GetLoadedMap.index == maps.Count - 1)
                    nextLevelButton.SetActive(false);
                else
                    nextLevelButton.SetActive(true);
                break;
        }

    }
    private void DeactivateAll()
    {
        if(pauseWindow != null)
            pauseWindow.SetActive(false);
        if(winWindow != null)
            winWindow.SetActive(false);
        if(gOWindow != null)
            gOWindow.SetActive(false);
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

    private void Start()
    {
        if(GameManager.Instance.StateOfGame == GameManager.GameState.MainMenu)
        {

            levelButtons = new List<GameObject>();
            foreach(Transform child in buttonsGrid.transform)
                levelButtons.Add(child.gameObject);

            int higherUnlockedLevel = PlayerPrefs.GetInt(GameManager.PlayerPrefKeys.HigherUnlockedLevel.ToString());
            Debug.Log(higherUnlockedLevel);
            int index = 0;
            foreach(GameObject button in levelButtons)
            {
                if(index > higherUnlockedLevel)
                    return;
                button.transform.GetChild(0).gameObject.SetActive(false);
                index++;

            }
        }
    }

}
