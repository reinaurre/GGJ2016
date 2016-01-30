using UnityEngine;
using UnityEngine.UI;

class Hud : MonoBehaviour {
    public Text scoreText = null;
    public Text timerText = null;

    public Image[] deaths = null;

    void Start() {
        Object.DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < deaths.Length; i++) {
            Color color = deaths[i].color;
            color.a = 0.0f;
            deaths[i].color = color;
        }
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
}
