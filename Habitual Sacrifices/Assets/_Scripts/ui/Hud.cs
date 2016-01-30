using UnityEngine;
using UnityEngine.UI;

class Hud : MonoBehaviour {
    public Text scoreText = null;
    public Text timerText = null;

    void Start() {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        GameManager gameManager = ServiceLocator.GetGameManager();
        scoreText.text = string.Format("{0}", (int)gameManager.Score);
        timerText.text = string.Format("{0:0.00}", gameManager.LevelTimer);
    }
}
