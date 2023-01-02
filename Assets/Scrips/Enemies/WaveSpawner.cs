using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private LevelData levelData;
    void Start()
    {
        levelData = GameManager.instance.levelData;
        GameManager.OnStartSpawning.AddListener(StartSpawning);
        GameManager.OnPlayerLose.AddListener(StopSpawning);
    }

    private Coroutine spawningTask;
    void StartSpawning()
    {
        spawningTask = StartCoroutine(TestSpawning());
    }

    void StopSpawning()
    {
        StopCoroutine(spawningTask);
        
    }

    IEnumerator TestSpawning()
    {
        SpawnGroup();
        for (int i = 1; i < levelData.startingEnemyGroups + GameManager.instance.currentWave; i++)
        {
            yield return new WaitForSeconds(levelData.timeBetweenSpawns);
            SpawnGroup();
        }

        // Wait for time it takes to spawn the last group
        yield return new WaitForSeconds(GameManager.instance.levelData.maxEnemyGroupSize*GameManager.instance.levelData.timeBetweenSpawns + Settings.instance.radiusTimeBeforeSpawn);
        GameManager.SetGameState(GameState.DoneSpawning);
    }

    void SpawnGroup()
    {
        Vector2 pos = new Vector2(-7f, Random.Range(3.6f, -3.6f));
        EnemySpawner.SpawnGroupInRadius(Settings.instance.EnemyNameToType(EnemyName.FastEnemy), pos, 1, Random.Range(levelData.minEnemyGroupSize, levelData.maxEnemyGroupSize+1));
    }
}
