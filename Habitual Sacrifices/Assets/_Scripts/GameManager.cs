using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public float scoreOnWin = 1000.0f;
    public float pauseTimeOnLevelEnd = 2.0f;
    public float pauseTimeOnLevelBegin = 2.0f;
    public float intermissionTime = 5.0f;

    /* Debugging use only */
    public string firstLevel = "";

    private string[] gameScenes = { "Simon", "Summoning", "Virgin Sacrifice", "MorningRitual" };//, "Demon", "Virgin", "Morning", "Aztec", "Rune" };
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

    public bool winOnTimeOut;

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
            EndLevel(false);
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

        /* Debugging */
        if (firstLevel.Length > 0) {
            StartGame(firstLevel);
        }
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
                SoundSystem ss = ServiceLocator.GetSoundSystem();
                ss.StopSound("whiteNoise");
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
                EndLevel(winOnTimeOut);
            }
        }
    }

    private void StartGame(string levelName = null)
    {
        gameActive = true;
        beginPaused = true;
        if (levelName != null) {
            /* For debugging */
            SceneManager.LoadScene(levelName);
        } else {
            /* Normal path */
            LoadRandomScene(true);
        }
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
        SoundSystem ss = ServiceLocator.GetSoundSystem();
        ss.StopBackgroundMusic();
        ss.PlaySound("scream");
        SceneManager.LoadScene("GameOver");
    }

    private void ResetGame()
    {
        SaveScore();

        //Reinitialize anything we need here
        _gameTimer = 0;
        _levelTimer = 0;
        winComboCount = 0;
        _modifiedLevelTime = maxLevelTime;
        levelsCompleted = 0;
        _score = 0;
        _lives = 3;

        SceneManager.LoadScene("Main");
    }

    private void SaveScore()
    {
        string highScoreStr = string.Empty;
        highScoreStr = PlayerPrefs.GetString("HighScores");

        List<int> scoresList = new List<int>();

        if(!string.IsNullOrEmpty(highScoreStr))
        {
            IEnumerable<int> scores = highScoreStr.Split(',').Select<string,int>(int.Parse); 
            scoresList = scores.OrderBy(v => v).ToList();
        }

        int insertLoc = -1;
        foreach (int i in scoresList)
        {
            if (_score >= i)
            {
                insertLoc++;
            }
            else
            {
                break;
            }
        }

        if(insertLoc == scoresList.Count)
        {
            scoresList.Insert(insertLoc, System.Convert.ToInt32(_score));
        }
        else if(insertLoc < 0)
        {
            return;
        }

        if(scoresList.Count > 5)
        {
            scoresList.RemoveAt(0);
        }

        PlayerPrefs.SetString("HighScores", highScoreStr);
    }

    public void WinLevelEarly()
    {
        if (IsPaused()) {
            return;
        }

        EndLevel(true);
    }

    public void EndLevel(bool won) {
        if (won) {
            winComboCount++;
        } else {
            winComboCount = 0;
        }

        levelsCompleted++;
        endPaused = true;
        OnLevelEnd.Invoke(won);
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
