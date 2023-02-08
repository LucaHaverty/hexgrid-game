using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private LevelData levelData;
    private WaveData waveData;
    
    private void Start()
    {
        levelData = GameManager.instance.levelData;
        waveData = levelData.waveData;
        
        GameManager.OnStartSpawning.AddListener(StartSpawning);
        GameManager.OnPlayerLose.AddListener(StopSpawning);
    }

    private Coroutine spawningTask;
    private void StartSpawning()
    {
        spawningTask = StartCoroutine(SpawnWave(GameManager.instance.currentWave));
    }

    private void StopSpawning()
    {
        StopCoroutine(spawningTask);
    }

    private IEnumerator TestSpawning()
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

    private IEnumerator SpawnWave(int waveNum)
    {
        Wave wave = waveData.waves[waveNum];
        foreach (var group in wave.enemyGroups)
        {
            for (int enemy = 0; enemy < group.numEnemies; enemy++)
            {
                EnemySpawner.SpawnSingleEnemy(group.GetEnemyType(), new Vector2(Random.Range(-6.5f, -7.5f), Random.Range(3.6f, -3.6f)));
                yield return new WaitForSeconds(group.timeBetweenSpawns);
            }

            yield return new WaitForSeconds(group.timeIdleAfterGroup);
        }
        GameManager.SetGameState(GameState.DoneSpawning);
    }

    private void SpawnGroup()
    {
        Vector2 pos = new Vector2(-7f, Random.Range(3.6f, -3.6f));
        EnemySpawner.SpawnGroupInRadius(Settings.instance.EnemyNameToType(EnemyName.FastEnemy), pos, 1, Random.Range(levelData.minEnemyGroupSize, levelData.maxEnemyGroupSize+1));
    }
}
