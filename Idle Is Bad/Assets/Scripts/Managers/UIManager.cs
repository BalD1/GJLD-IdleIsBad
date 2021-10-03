using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [SerializeField] private GameObject title;
    [SerializeField] private GameObject buttonsGrid;
    [SerializeField] private GameObject levelsWindow;
    private List<GameObject> levelButtons;

    [Header("In Game")]

    [SerializeField] private GameObject pauseWindow;
    [SerializeField] private GameObject pauseFirstSelected;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private GameObject winFirstSelected;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject gOWindow;
    [SerializeField] private GameObject gOFirstSelected;

    [Header("All")]

    [SerializeField] private GameObject optionsWindow;

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
                EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
                break;

            case GameManager.GameState.GameOver:
                DeactivateAll();
                gOWindow.SetActive(true);
                EventSystem.current.SetSelectedGameObject(gOFirstSelected);
                break;

            case GameManager.GameState.Win:
                DeactivateAll();
                winWindow.SetActive(true);

                List<GameObject> maps = GameManager.Instance.Maps;

                if(GameManager.Instance.GetLoadedMap.index == maps.Count - 1 || maps[GameManager.Instance.GetLoadedMap.index + 1] == null)
                    nextLevelButton.SetActive(false);
                else
                    nextLevelButton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(winFirstSelected);
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
        AudioManager.Instance.Play2DSound(AudioManager.ClipsTags.Clic);

        switch(button)
        {
            case "Play":
                levelsWindow.SetActive(!levelsWindow.activeSelf);

                if(optionsWindow.activeSelf)
                    optionsWindow.SetActive(false);
                else
                    title.SetActive(!title.activeSelf);
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

            case "Options":
                optionsWindow.SetActive(!optionsWindow.activeSelf);

                if (levelsWindow.activeSelf)
                    levelsWindow.SetActive(false);
                else
                    title.SetActive(!title.activeSelf);
                break;

            case "OptionsInGame":
                optionsWindow.SetActive(!optionsWindow.activeSelf);
                break;

            case "Retry":
                GameManager.Instance.player.ChangeSprite(Player.State.Normal);
                GameManager.Instance.ReloadMap();
                GameManager.Instance.StateOfGame = GameManager.GameState.InGame;
                break;

            case "NextLevel":
                GameManager.Instance.player.ChangeSprite(Player.State.Normal);
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
            int index = 0;
            foreach(GameObject button in levelButtons)
            {
                if(index > higherUnlockedLevel)
                    return;
                button.transform.GetChild(0).gameObject.SetActive(false);
                button.GetComponent<Button>().interactable = true;
                index++;

            }
        }
    }

}
