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
        startNextWaveButton.SetActive(false);
        currentWave++;
        waveText.text = $"{(currentWave)} / {numWaves}";
        
        GameManager.SetGameState(GameState.Spawning);
    }

    private void OnWaitingForWave()
    {
        startNextWaveButton.SetActive(true);
    }
}
