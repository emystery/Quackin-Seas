using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    LogBook,
    Dead,
    MainMenu,
    EndScreen
}

public class GameManager : MonoBehaviour
{
    public string firstScene;
    public GameState state;

    [Header("References")]
    public static GameManager self;
    public PlayerController playerControllerScript;
    public string activeScene;
    [Space]

    [Space]
    [Header("Values")]
    public float sensitivity;
    public int audioVolume;
    public int treasuresLeft;
    [Space]

    [Space]
    [Header("Booleans")]
    public bool paused;
    public bool unlockTreasures;
    public bool gameEnded;
    
    public static GameManager GetGlobalInfo()
    {
        return(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>());
    }
    
    void Start()
    {
        gameEnded = false;
        unlockTreasures = false;
        DontDestroyOnLoad(gameObject);
        if(self == null)
        {
            self = this;
        }else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            StartCoroutine(InitializeGame());
        }

    }

    void Update()
    {
        if(state != GameState.Playing)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == GameState.Paused)
            {
                Resume();
            }
            else if (state == GameState.Playing)
            {
                Pause();
            }
            else if (state == GameState.LogBook)
            {
                CloseLogBook();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && state != GameState.Paused)
        {
            if (GameManager.self.state == GameState.LogBook)
            {
                CloseLogBook();
            }
            else
            {
                OpenLogBook();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && state == GameState.Dead)
        {
            playerControllerScript.TeleportToRudder();
        }

        if (gameEnded)
        {
            LoadScene("End Scene");
            gameEnded = false;
        }

        if (state == GameState.EndScreen && Input.GetKeyDown(KeyCode.U))
        {
            
            LoadScene("Start Menu");
            gameEnded = false;
        }
    }

    public IEnumerator InitializeGame()
    {
        yield return new WaitForSeconds(0.5f);
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        treasuresLeft = GameObject.FindObjectsOfType<TreasureScript>().Length;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (sceneName == "SampleScene")
        {
            gameEnded = false;
            unlockTreasures = false;

            state = GameState.Playing;
            StartCoroutine(InitializeGame());
            Resume();
        }
        if (sceneName == "Start Menu")
        {
            state = GameState.MainMenu;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        state = GameState.Playing;
    }

    void Pause()
    {
        Time.timeScale = 0f;
        state = GameState.Paused;
    }


    public void CloseLogBook()
    {
        state = GameState.Playing;
        Time.timeScale = 1.0f;

    }

    void OpenLogBook()
    {
        state = GameState.LogBook;
        Time.timeScale = 0f;
    }


    public void LoadMenu()
    {
        Time.timeScale = 0f;
        Debug.Log("Loading Menu...");
        //SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}