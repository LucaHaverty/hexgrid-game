using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    [SerializeField] private CanvasGroup[] gameUI;
    [SerializeField] private CanvasGroup gameOverMenu;
    [SerializeField] private TextMeshProUGUI winOrLoseText;
    void Start()
    {
        gameOverMenu.alpha = 0;
        GameManager.OnPlayerLose.AddListener(OnLost);
        GameManager.OnPlayerWon.AddListener(OnWin);
    }
    
    private void OnLost()
    {
        winOrLoseText.text = "You Lose";
        StartCoroutine(FadeMenuIn());
    }
    
    private void OnWin() {
        winOrLoseText.text = "You Win";
        StartCoroutine(FadeMenuIn());
    }
    
    public IEnumerator FadeMenuIn()
    {
        float startTime = Time.time;
        while (Time.time - startTime < fadeTime)
        {
            float percent = (Time.time - startTime) / fadeTime;
            foreach (var group in gameUI)
                group.alpha = 1 - percent;
            gameOverMenu.alpha = percent;
            
            yield return new WaitForEndOfFrame();
        }
        
        foreach (var group in gameUI)
            group.alpha = 0;
        gameOverMenu.alpha = 1;
    }
}
