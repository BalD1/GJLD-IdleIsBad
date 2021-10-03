using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
                Debug.LogError("GameManger instance not found");

            return instance;
        }
    }


    public enum GameState
    {
        MainMenu,
        InGame,
        Pause,
        GameOver,
        Win,
    }
    [SerializeField] private GameState gameState;
    public GameState StateOfGame
    {
        get
        {
            return gameState;
        }
        set
        {
            switch(value)
            {
                case GameState.MainMenu:
                    ChangeScene("MainMenu");
                    Time.timeScale = 0;
                    break;
                case GameState.InGame:
                    if(gameState == GameState.MainMenu)
                        ChangeScene("MainScene");
                    if(player != null)
                        player.Gravity(1);
                    Time.timeScale = 1;
                    break;
                case GameState.Pause:
                    Time.timeScale = 0;
                    break;
                case GameState.GameOver:
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Time.timeScale = 0;
                    break;
                case GameState.Win:
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Time.timeScale = 1;

                    if(DataKeep.higherUnlockedLevel == loadedMap.index)
                    {
                        if(loadedMap.index < maps.Count - 1m)
                            if(maps[loadedMap.index + 1] != null)
                                DataKeep.higherUnlockedLevel += 1;
                        PlayerPrefs.SetInt(PlayerPrefKeys.HigherUnlockedLevel.ToString(), DataKeep.higherUnlockedLevel);
                    }
                    break;
            }
            gameState = value;
            UIManager.Instance.WindowManager(value);
        }
    }

    public enum PlayerPrefKeys
    {
        HigherUnlockedLevel,
    }

    public Player player;
    [SerializeField] private List<GameObject> maps;
#if UNITY_EDITOR
    [SerializeField] private bool resetProgress;
    [SerializeField] private bool loadMap = true;
    public bool canDestroy = true;
#endif
    public List<GameObject> Maps
    {
        get => maps;
    }
    public struct LoadedMap
    {
        public int index;
        public GameObject map;
    }
    private LoadedMap loadedMap;
    public LoadedMap GetLoadedMap
    {
        get => loadedMap;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {      
        if(loadedMap.map == null && StateOfGame != GameState.MainMenu)
        {
            loadedMap.map = GameObject.Instantiate(maps[DataKeep.mapToLoad - 1], Vector3.zero, Quaternion.identity);
            loadedMap.index = DataKeep.mapToLoad - 1;
#if UNITY_EDITOR
            if(!loadMap)
                Destroy(loadedMap.map);
#endif
        }

        if(PlayerPrefs.HasKey(PlayerPrefKeys.HigherUnlockedLevel.ToString()))
            DataKeep.higherUnlockedLevel = PlayerPrefs.GetInt(PlayerPrefKeys.HigherUnlockedLevel.ToString());
        else
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.HigherUnlockedLevel.ToString(), 0);
            DataKeep.higherUnlockedLevel = PlayerPrefs.GetInt(PlayerPrefKeys.HigherUnlockedLevel.ToString());
        }

#if UNITY_EDITOR
        if (resetProgress)
        {
            DataKeep.mapToLoad = 0;
            PlayerPrefs.SetInt(PlayerPrefKeys.HigherUnlockedLevel.ToString(), 0);
        }
#endif
    }

    public void ChangeScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReloadMap()
    {
        Destroy(loadedMap.map);
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(AudioManager.ClipsTags.MainTheme);
        loadedMap.map = GameObject.Instantiate(maps[loadedMap.index], Vector3.zero, Quaternion.identity);
        player.transform.position = Vector2.zero;
    }
    public void NextMap()
    {
        Destroy(loadedMap.map);
        loadedMap.index += 1;
        loadedMap.map = GameObject.Instantiate(maps[loadedMap.index], Vector3.zero, Quaternion.identity);
        player.transform.position = Vector2.zero;
    }
}
