using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveUIManager : MonoBehaviour
{
    public static WaveUIManager instance;

    private void Awake()
    {
        GameManager.OnWaitingForWave.AddListener(OnWaitingForWave);
        instance = this;
    }
    
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI startWaveButtonText;
    public GameObject startNextWaveButton;
    
    private int currentWave;
    private int numWaves;
    
    private void Start()
    {
        numWaves = GameManager.instance.levelData.waveData.numWaves;
        waveText.text = $"";
    }

    public void NextWaveStarted()
    {
        if (autoStartRoutine != null) StopCoroutine(autoStartRoutine);
        startNextWaveButton.SetActive(false);
        countdownText.gameObject.SetActive(false);
        currentWave++;
        waveText.text = $"{(currentWave)} / {numWaves}";
        
        GameManager.SetGameState(GameState.Spawning);
    }

    private Coroutine autoStartRoutine;
    private void OnWaitingForWave(bool firstWave)
    {
        startNextWaveButton.SetActive(true);
        if (firstWave) autoStartRoutine = StartCoroutine(AutoStartWave(Settings.instance.autoStartFirstWaveTime));
        else autoStartRoutine = StartCoroutine(AutoStartWave(Settings.instance.autoStartWaveTime));

        if (firstWave) startWaveButtonText.text = "Start Game";
        else startWaveButtonText.text = "Start Next Wave";

    }

    private IEnumerator AutoStartWave(int time)
    {
        countdownText.gameObject.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            countdownText.text = (time - i - 1).ToString();
            yield return new WaitForSeconds(1);
        }
        NextWaveStarted();
    }
}
