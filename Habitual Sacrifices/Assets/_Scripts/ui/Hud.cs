using UnityEngine;
using UnityEngine.UI;

class Hud : MonoBehaviour {
    public Text scoreText = null;
    public Text timerText = null;

    public CanvasGroup winGroup = null;
    public CanvasGroup loseGroup = null;
    public CanvasGroup intermissionGroup = null;
    public CanvasGroup instructionsGroup = null;

    public Image[] deaths = null;

    void Start() {
        Object.DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < deaths.Length; i++) {
            Color color = deaths[i].color;
            color.a = 0.0f;
            deaths[i].color = color;
        }

        BeginLevel();
    }

    void Update() {
        GameManager manager = ServiceLocator.GetGameManager();

        scoreText.text = string.Format("{0}", (int)manager.Score);
        timerText.text = string.Format("{0:0.00}",
                manager.ModifiedLevelTime - manager.LevelTimer).Replace(".", ":");

        for (int i = 0; i < deaths.Length; i++) {
            if (i >= manager.Lives) {
                Color color = deaths[i].color;
                color.a = 1.0f;
                deaths[i].color = color;
            } else {
                Color color = deaths[i].color;
                color.a = 0.0f;
                deaths[i].color = color;
            }
        }
    }

    public void DisplayMinigameEnd(bool won) {
        if (won) {
            winGroup.alpha = 1.0f;
        } else {
            loseGroup.alpha = 1.0f;
        }
    }

    public void IntermissionBegin() {
        winGroup.alpha = 0.0f;
        loseGroup.alpha = 0.0f;
        intermissionGroup.alpha = 1.0f;
        ServiceLocator.GetSoundSystem().PlaySound("whiteNoise");
    }
    
    public void LoadLevel() {
        winGroup.alpha = 0.0f;
        loseGroup.alpha = 0.0f;
        intermissionGroup.alpha = 0.0f;
        instructionsGroup.alpha = 1.0f;
    }
    
    public void BeginLevel() {
        winGroup.alpha = 0.0f;
        loseGroup.alpha = 0.0f;
        intermissionGroup.alpha = 0.0f;
        instructionsGroup.alpha = 0.0f;
    }
}
