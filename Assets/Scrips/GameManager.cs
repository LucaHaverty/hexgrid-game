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
    public bool notInGame;
    public bool mainMenu;

    #region Event Functions
    void Awake()
    {
        if (LevelDataHolder.data != null) levelData = LevelDataHolder.data;
        instance = this;
        
        if (mainMenu)
            return;
        
        Camera.main.transform.position = new Vector3(levelData.camOffset.x, levelData.camOffset.y, -10);
        Camera.main.orthographicSize = levelData.camSize;
    }

    void Start()
    {
        if (mainMenu)
            return;

        if (notInGame)
            return;
        
        EnemySpawner.OnEnemySpawned.AddListener(OnEnemySpawned);

        state = GameState.None;
        BobTheBuilder.AttemptBuild(BuildingName.EnemyTarget, levelData.enemyTargetLocation);
        
        StartCoroutine(RunGameLoop());
    }

    void Update()
    {
        if (notInGame)
            return;
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartCurrentLevel();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitToMenu();
        }

        if (state == GameState.DoneSpawning && Settings.instance.enemyContainer.childCount == 0)
            SetGameState(GameState.WaveBeat);
    }
    #endregion

    #region Game State Management

    public static UnityEvent OnWaveBeat = new UnityEvent();
    public static UnityEvent OnPlayerLose = new UnityEvent();
    public static UnityEvent OnStartSpawning = new UnityEvent();
    public static UnityEvent<bool> OnWaitingForWave = new UnityEvent<bool>();
    public static UnityEvent OnPlayerWon = new UnityEvent();

    [HideInInspector] public int currentWave = 0;

    public static void SetGameState(GameState newState)
    {
        if (state == newState)
            return;

        state = newState;
        switch (state)
        {
            case(GameState.WaitingForWave):
                if (instance.currentWave == 0) OnWaitingForWave.Invoke(true);
                else OnWaitingForWave.Invoke(false);
                break;
            case(GameState.Spawning):
                OnStartSpawning.Invoke();
                break;
            case(GameState.DoneSpawning):
                break;
            case(GameState.PlayerLost):
                instance.KillAllEnemies();
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
    
    private void KillAllEnemies() {
        foreach (Transform enemy in Settings.instance.enemyContainer)
        {
            enemy.GetComponent<AbstractEnemy>().TriggerDestroy();
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
        while (currentWave < levelData.waveData.waves.Length)
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
            {
                yield break;
            }

            if (currentWave == levelData.waveData.waves.Length)
                SetGameState(GameState.PlayerWon);
            MoneyManager.instance.AttemptAddMoney(levelData.moneyGainPerWave);
        }
    }

    private void ResetGameManager()
    {
        StopCoroutine(RunGameLoop());
        enemiesAlive = 0;
        //SetGameState(GameState.None);
        currentWave = 0;
    }
    
    public void RestartCurrentLevel()
    {
        ResetGameManager();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartCoroutine(RunGameLoop());
    }

    public void QuitToMenu()
    {
        ResetGameManager();
        SceneManager.LoadScene(0);
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
