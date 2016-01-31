﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float scoreOnWin = 1000.0f;
    public float pauseTimeOnLevelEnd = 2.0f;
    public float pauseTimeOnLevelBegin = 2.0f;
    public float intermissionTime = 2.0f;

    private string[] gameScenes = { "Simon", "Summoning", "Sacrifice" };//, "Demon", "Virgin", "Morning", "Aztec", "Rune" };
    private bool endPaused;
    private bool intermission;
    private bool beginPaused;
    private float pauseTimer = 0.0f;

    private AsyncOperation loadNextSceneOperation;

    private float _score;
    public float Score { get { return _score; } }

    private int _lives;
    public int Lives { get { return _lives; } }

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

    [System.Serializable]
    public class LevelEndEvent : UnityEvent<bool> {};
    public LevelEndEvent OnLevelEnd = new LevelEndEvent();

    [System.Serializable]
    public class IntermissionEvent : UnityEvent {};
    public IntermissionEvent OnIntermissionEvent = new IntermissionEvent();

    [System.Serializable]
    public class LevelLoadEvent : UnityEvent {};
    public LevelLoadEvent OnLevelLoad = new LevelLoadEvent();

    [System.Serializable]
    public class LevelBeginEvent : UnityEvent {};
    public LevelBeginEvent OnLevelBegin = new LevelBeginEvent();

    private bool IsPaused() {
        return endPaused || intermission || beginPaused;
    }

    public void IncrementScore(float increase)
    {
        if (IsPaused()) {
            return;
        }
        
        _score += increase;
    }

    public void LoseLife()
    {
        if (IsPaused()) {
            return;
        }

        _lives--;

        if (Lives <= 0)
        {
            GameOver();
        }
        else
        {
            winComboCount = 0;
            endPaused = true;
            OnLevelEnd.Invoke(false);
        }
    }

	// Use this for initialization
	void Start ()
    {
        Object.DontDestroyOnLoad(this.gameObject);
        gameActive = false;
        levelActive = false;
        endPaused = false;
        intermission = false;
        beginPaused = false;
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

        if (endPaused)
        {
            Time.timeScale = 0.0f;
            _levelTimer = _modifiedLevelTime;
            pauseTimer += Time.unscaledDeltaTime;
            if (pauseTimer >= pauseTimeOnLevelEnd) {
                pauseTimer = 0.0f;
                endPaused = false;
                intermission = true;
                LoadRandomScene();
                OnIntermissionEvent.Invoke();
            }
        }
        else if(intermission)
        {
            Time.timeScale = 0.0f;
            pauseTimer += Time.unscaledDeltaTime;
            if (pauseTimer >= intermissionTime) {
                _levelTimer = 0.0f;
                pauseTimer = 0.0f;
                intermission = false;
                beginPaused = true;
                loadNextSceneOperation.allowSceneActivation = true;
                OnLevelLoad.Invoke();
            }
        }
        else if(beginPaused)
        {
            Time.timeScale = 0.0f;
            pauseTimer += Time.unscaledDeltaTime;
            if (pauseTimer >= pauseTimeOnLevelBegin) {
                Time.timeScale = 1.0f;
                pauseTimer = 0.0f;
                beginPaused = false;
                OnLevelBegin.Invoke();
            }
        }
        else if(gameActive)
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
                levelsCompleted++;
                OnLevelEnd.Invoke(true);
                endPaused = true;
            }
        }
    }

    private void StartGame()
    {
        gameActive = true;
        beginPaused = true;
        LoadRandomScene(true);
        OnLevelLoad.Invoke();

        SoundSystem ss = ServiceLocator.GetSoundSystem();
        ss.PlaySound("startGame");
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void GameOver()
    {
        endPaused = false;
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
        if (IsPaused()) {
            return;
        }

        winComboCount++;
        endPaused = true;
        OnLevelEnd.Invoke(true);
    }

    private void LoadRandomScene(bool immediately = false)
    {
        int currentScene = System.Array.IndexOf(gameScenes, SceneManager.GetActiveScene().name);

        int nextScene = Random.Range(0, gameScenes.Length);
        while (nextScene == currentScene) {
            nextScene = Random.Range(0, gameScenes.Length);
        }

        if (!immediately) {
            loadNextSceneOperation = SceneManager.LoadSceneAsync(gameScenes[nextScene]);
            loadNextSceneOperation.allowSceneActivation = false;
        } else {
            SceneManager.LoadScene(gameScenes[nextScene]);
        }
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
