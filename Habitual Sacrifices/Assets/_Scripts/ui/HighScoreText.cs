using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(TextMesh))]
public class HighScoreText : MonoBehaviour
{
    void Start() {
        TextMesh text = GetComponent<TextMesh>();
        string highScoreStr = PlayerPrefs.GetString("HighScores");
        IEnumerable<string> scores = highScoreStr.Split(',').OrderBy(v => int.Parse(v));
        text.text = "";
        foreach (string score in scores.Reverse()) {
            text.text += score + System.Environment.NewLine;
        }
        if (text.text.Equals("")) {
            text.text = "No scores yet";
        }
    }
}
