using UnityEngine;
using UnityEngine.UI;

class Hud : MonoBehaviour {
    public Text scoreText = null;
    public Text timerText = null;

    public Image[] lives = null;
    public Sprite aliveSprite = null;
    public Sprite deadSprite = null;

    void Start() {
        Object.DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < lives.Length; i++) {
            lives[i].sprite = aliveSprite;
        }
    }

    void Update() {
        GameManager manager = ServiceLocator.GetGameManager();

        scoreText.text = string.Format("{0}", (int)manager.Score);
        timerText.text = string.Format("{0:0.00}",
                manager.ModifiedLevelTime - manager.LevelTimer).Replace(".", ":");

        for (int i = 0; i < lives.Length; i++) {
            if (i < manager.Lives) {
                lives[i].sprite = aliveSprite;
            } else {
                lives[i].sprite = deadSprite;
            }
        }
    }
}
