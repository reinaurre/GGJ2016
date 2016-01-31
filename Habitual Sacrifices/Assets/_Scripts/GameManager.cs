using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float scoreOnWin = 1000.0f;

    private string[] gameScenes = { "Simon", "Summoning" };//, "Demon", "Virgin", "Morning", "Aztec", "Rune" };


    private float _score;
    public float Score { get { return _score; } }
    public void IncrementScore(float increase)
    {
        _score += increase;
    }

    private int _lives;
    public int Lives { get { return _lives; } }
    public void LoseLife()
    {
        _lives--;

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

    private float _modifiedLevelTime;
    public float ModifiedLevelTime { get { return _modifiedLevelTime; } }

    public float minLevelTime = 3.0f;
    public float maxLevelTime = 15.0f;
    private int winComboCount = 0;

    private int levelsCompleted = 0;

    private float _levelTimer;
    public float LevelTimer { get { return _levelTimer; } }

    private float _gameTimer;
    public float GameTimer { get { return _gameTimer; } }

    private bool gameActive;
    private bool levelActive;

	// Use this for initialization
	void Start ()
    {
        Object.DontDestroyOnLoad(this.gameObject);
        gameActive = false;
        levelActive = false;
        _modifiedLevelTime = maxLevelTime;
        SceneManager.LoadScene("Main");

        _lives = 3;
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
            _gameTimer += Time.deltaTime;

            if(levelActive)
            {
                _levelTimer += Time.deltaTime;
            }

            if(LevelTimer >= _modifiedLevelTime)
            {
                IncrementScore(scoreOnWin);
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
        _gameTimer = 0;
        _levelTimer = 0;
        winComboCount = 0;
        _modifiedLevelTime = maxLevelTime;

        SceneManager.LoadScene("Main");
    }

    public void WinLevelEarly()
    {
        winComboCount++;
        ChangeLevel();
    }

    private void ChangeLevel()
    {
        levelsCompleted++;
        LoadRandomScene();
    }

    private void LoadRandomScene()
    {
        int currentScene = System.Array.IndexOf(gameScenes, SceneManager.GetActiveScene().name);

        int nextScene = Random.Range(0, gameScenes.Length);
        while (nextScene == currentScene) {
            nextScene = Random.Range(0, gameScenes.Length);
        }

        _levelTimer = 0.0f;
        SceneManager.LoadScene(gameScenes[nextScene]);
    }

    void OnLevelWasLoaded(int level)
    {
        levelActive = true;
        CalculateNewLevelTime();
    }

    void CalculateNewLevelTime() {
        _modifiedLevelTime = Mathf.Pow(1.2f, -levelsCompleted) * (maxLevelTime - minLevelTime) + minLevelTime;
    }

    public float GetTimeFractionLeft() {
        return 1.0f - (_modifiedLevelTime - minLevelTime) / (maxLevelTime - minLevelTime);
    }

    public float GetSpeedFactor(float maxSpeedUp) {
        return (1.0f + maxSpeedUp * GetTimeFractionLeft()) * Time.deltaTime;
    }
}
