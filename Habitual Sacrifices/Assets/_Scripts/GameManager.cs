using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private float _score;
    public float Score { get; set; }
    public void IncrementScore(float increase)
    {
        Score += increase;
    }

    private int _lives;
    public int Lives { get; set; }
    public void LoseLife()
    {
        Lives--;

        if (Lives <= 0)
        {
            GameOver();
        }
        else
        {
            winComboCount = 0;
            ChangeLevel();
        }
    }

    public float maxLevelTime = 1500f;
    private float modifiedLevelTime;
    public float timeDecrement = 100f;
    private int winComboCount = 0;

    private float _levelTimer;
    public float LevelTimer { get; set; }

    private float _gameTimer;
    public float GameTimer { get; set; }

    private string[] gameScenes = { "Summoning" };//, "Demon", "Virgin", "Morning", "Aztec", "Rune" };
    private bool gameActive;
    private bool levelActive;

	// Use this for initialization
	void Start ()
    {
        Object.DontDestroyOnLoad(this.gameObject);
        gameActive = false;
        levelActive = false;
        modifiedLevelTime = maxLevelTime;
        SceneManager.LoadScene("Main");

        Lives = 3;
	}

    void Update ()
    {
        if (Input.GetButtonDown("Start"))
        {
            switch(SceneManager.GetActiveScene().name)
            {
                case "Credits": SceneManager.LoadScene("Main"); break;
                case "Main": StartGame(); break;
                case "GameOver": ResetGame(); break;
                default: break;
            }
        }

        if (Input.GetButtonDown("Option"))
        {
            SceneManager.LoadScene("Credits");
        }

        if(gameActive)
        {
            GameTimer += Time.deltaTime;

            if(levelActive)
            {
                LevelTimer += Time.deltaTime;
            }

            if(LevelTimer >= modifiedLevelTime)
            {
                IncrementScore(1000f);
                winComboCount++;
                ChangeLevel();
            }
        }
    }

    private void StartGame()
    {
        gameActive = true;
        LoadRandomScene();
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void GameOver()
    {
        gameActive = false;
        levelActive = false;
        SceneManager.LoadScene("GameOver");
    }

    private void ResetGame()
    {
        //Reinitialize anything we need here
        GameTimer = 0;
        LevelTimer = 0;
        winComboCount = 0;
        modifiedLevelTime = maxLevelTime;

        SceneManager.LoadScene("Main");
    }

    private void ChangeLevel()
    {
        LoadRandomScene();
    }

    private void LoadRandomScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        int currentNum = 0;

        if (currentScene != "Main")
        {
            currentNum = System.Array.IndexOf(gameScenes, currentScene);
        }

        int num = 0;

        while(num == 0 || num == currentNum)
        {
            num = Random.Range(1, gameScenes.Length);
        }

        SceneManager.LoadScene(num);
    }

    void OnLevelWasLoaded(int level)
    {
        levelActive = true;
        modifiedLevelTime -= (timeDecrement * winComboCount * 0.5f);
    }
}
