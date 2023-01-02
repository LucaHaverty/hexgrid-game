using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        stateUI = GameStateUI.instance;
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
            SetGameState(GameState.PlayerWon);
    }
    #endregion

    #region Game State Management
    private GameStateUI stateUI;

    public static UnityEvent OnPlayerWin = new UnityEvent();
    public static UnityEvent OnPlayerLose = new UnityEvent();
    public static UnityEvent OnStartSpawning = new UnityEvent();
    public static UnityEvent OnStartBuilding = new UnityEvent();

    public int currentWave = 0;

    public static void SetGameState(GameState newState)
    {
        if (state == newState)
            return;

        state = newState;
        switch (state)
        {
            case(GameState.Building):
                OnStartBuilding.Invoke();
                SetStateText("Next Attack In");
                break;
            case(GameState.Spawning):
                OnStartSpawning.Invoke();
                SetStateText("Spawning");
                break;
            case(GameState.DoneSpawning):
                SetStateText("Done Spawning");
                break;
            case(GameState.PlayerLost):
                OnPlayerLose.Invoke();
                SetStateText("You Lose");
                break;
            case(GameState.PlayerWon):
                OnPlayerWin.Invoke();
                SetStateText("Player Win");
                break;
        }
    }

    private static void SetStateText(string text) => instance.stateUI.SetStateText(text);

    private static int enemiesAlive;

    public static void OnEnemySpawned()
    {
        enemiesAlive++;
    }
    public static void OnEnemyKilled()
    {
        enemiesAlive -= 1;
        if (state == GameState.DoneSpawning && enemiesAlive <= 1)
            SetGameState(GameState.PlayerWon);
    }
    #endregion

    IEnumerator RunGameLoop()
    {
        while (true)
        {
            yield return stateUI.StartCountdown(levelData.buildTime, "Next Attack In");
            
            SetGameState(GameState.Spawning);
            stateUI.HideTimer();
            while (state == GameState.Spawning || state == GameState.DoneSpawning)
            {
                yield return new WaitForEndOfFrame();
            }

            if (state == GameState.PlayerLost)
                yield break;
            
            SetGameState(GameState.Building);
            currentWave++;
            MoneyManager.instance.AttemptAddMoney(levelData.moneyGainPerWave);
        }
    }
}

public enum GameState
{
    None,
    Building,
    Spawning,
    DoneSpawning,
    PlayerLost,
    PlayerWon
}
