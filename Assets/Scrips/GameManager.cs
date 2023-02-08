using System.Collections;
using Scrips;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameState state { get; private set; } = GameState.None;

    public LevelData levelData;

    #region Event Functions
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        EnemySpawner.OnEnemySpawned.AddListener(OnEnemySpawned);

        BobTheBuilder.AttemptBuild(BuildingName.EnemyTarget, Settings.instance.enemyTarget.position);
        
        StartCoroutine(RunGameLoop());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopCoroutine(RunGameLoop());
            enemiesAlive = 0;
            SetGameState(GameState.None);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (state == GameState.DoneSpawning && Settings.instance.enemyContainer.childCount == 0)
            SetGameState(GameState.WaveBeat);
    }
    #endregion

    #region Game State Management

    public static UnityEvent OnWaveBeat = new UnityEvent();
    public static UnityEvent OnPlayerLose = new UnityEvent();
    public static UnityEvent OnStartSpawning = new UnityEvent();
    public static UnityEvent OnWaitingForWave = new UnityEvent();
    public static UnityEvent OnPlayerWon = new UnityEvent();

    public int currentWave = 0;

    public static void SetGameState(GameState newState)
    {
        if (state == newState)
            return;

        state = newState;
        switch (state)
        {
            case(GameState.WaitingForWave):
                OnWaitingForWave.Invoke();
                break;
            case(GameState.Spawning):
                OnStartSpawning.Invoke();
                break;
            case(GameState.DoneSpawning):
                break;
            case(GameState.PlayerLost):
                OnPlayerLose.Invoke();
                break;
            case(GameState.WaveBeat):
                OnWaveBeat.Invoke();
                break;
            case(GameState.PlayerWon):
                OnPlayerWon.Invoke();
                break;
        }
    }
    
    private static int enemiesAlive;
    public static void OnEnemySpawned()
    {
        enemiesAlive++;
    }
    public static void OnEnemyKilled(EnemyType enemyType, Vector2 pos)
    {
        MoneyManager.instance.SpawnAnimatedCoins(pos, enemyType.coinsDropped);
        
        enemiesAlive -= 1;
        if (state == GameState.DoneSpawning && enemiesAlive <= 0)
            SetGameState(GameState.WaveBeat);
    }
    #endregion

    IEnumerator RunGameLoop()
    {
        while (currentWave < levelData.waveData.waves.Length-1)
        {
            SetGameState(GameState.WaitingForWave);

            while (state != GameState.Spawning)
                yield return null;
            
            currentWave++;

            while (state == GameState.Spawning || state == GameState.DoneSpawning)
            {
                yield return new WaitForEndOfFrame();
            }

            if (state == GameState.PlayerLost)
                yield break;
            
            SetGameState(GameState.WaitingForWave);
            MoneyManager.instance.StartCoroutine(
                MoneyManager.instance.WaveOverCoinAnim(Mathf.FloorToInt(levelData.moneyGainPerWave / 5f), 5));
        }
        
        
    }
}

public enum GameState
{
    None,
    WaitingForWave,
    Spawning,
    DoneSpawning,
    PlayerLost,
    WaveBeat,
    PlayerWon
}
