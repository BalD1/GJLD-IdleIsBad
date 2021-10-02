using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
                    Time.timeScale = 0;
                    break;
            }
            gameState = value;
            UIManager.Instance.WindowManager(value);
        }
    }

    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> maps;
    private struct LoadedMap
    {
        public int index;
        public GameObject map;
    }
    private LoadedMap loadedMap;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (loadedMap.map == null && StateOfGame != GameState.MainMenu)
        {
            loadedMap.map = GameObject.Instantiate(maps[DataKeep.mapToLoad - 1], Vector3.zero, Quaternion.identity);
            loadedMap.index = DataKeep.mapToLoad - 1;
        }
    }

    public void ChangeScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReloadMap()
    {
        Destroy(loadedMap.map);
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
