using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private void Awake() { instance = this; }
    
    public TextMeshProUGUI scoreText;
    private int score;

    private void Start()
    {
        scoreText.text = "0";
        GameManager.OnPlayerWin.AddListener(IncreaseScore);
    }

    private void IncreaseScore()
    {
        score ++;
        scoreText.text = score.ToString();
    }
}
