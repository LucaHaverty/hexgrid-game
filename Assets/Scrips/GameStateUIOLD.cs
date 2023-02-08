using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameStateUIOLD : MonoBehaviour
{
    public static GameStateUIOLD instance;
    private void Awake() { instance = this; }

    [SerializeField] private TextMeshProUGUI timerTextMesh;
    [SerializeField] private TextMeshProUGUI stateTextMesh;

    public IEnumerator StartCountdown(int seconds)
    {
        while (seconds >= 0)
        {
            timerTextMesh.text = seconds.ToString();
            seconds--;
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator StartCountdown(int seconds, string stateText)
    {
        stateTextMesh.text = stateText;
        yield return StartCountdown(seconds);
    }

    public void SetStateText(string stateText)
    {
        stateTextMesh.text = stateText;
    }

    public void HideTimer()
    {
        timerTextMesh.text = "";
    }

    public void HideStateText()
    {
        stateTextMesh.text = "";
    }
}
