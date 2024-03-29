using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    private void Awake() { instance = this; }

    public static UnityEvent OnEnemySpawned = new UnityEvent();

    //public EnemyName[] testEnemies;

    private static void SpawnSingleEnemy(EnemyType type, Vector2 position)
    {
        if (GridManager.instance.FindCloseTile(position).type.tileName == "Void")
        {
            Debug.LogError("Tried To Spawn Enemy In Void");
            return;
        }
        Instantiate(type.prefab, position, Quaternion.identity).transform.SetParent(Settings.instance.enemyContainer);
        OnEnemySpawned.Invoke();
        AudioManager.instance.Play("EnemySpawn");
    }

    public static void SpawnSingleEnemy(EnemyName enemyName, Vector2 position)
    {
        if (enemyName == EnemyName.EasyBoss || enemyName == EnemyName.MedBoss || enemyName == EnemyName.HardBoss)
            position = GameManager.instance.levelData.bossSpawnLocation;
        SpawnSingleEnemy(Settings.instance.EnemyNameToType(enemyName), position);
    }

    public static void SpawnGroupInRadius(EnemyType type, Vector2 origin, float radius, int numEnemies)
    {
        instance.StartCoroutine(SpawnGroupInRadiusRoutine(type, origin, radius, numEnemies));
    }    
    private static IEnumerator SpawnGroupInRadiusRoutine(EnemyType type, Vector2 origin, float radius, int numEnemies)
    {
        GameObject circleClone = Instantiate(Settings.instance.radiusCirclePrefab, origin, Quaternion.identity);
        circleClone.transform.SetParent(Settings.instance.effectsContainer);
        circleClone.transform.localScale = new Vector3(radius*2+0.5f, radius*2+0.5f);

        yield return new WaitForSeconds(Settings.instance.radiusTimeBeforeSpawn);

        Vector2 GetPosition()
        {
            // Radial coords
            float r = radius * Mathf.Sqrt(Random.Range(0f, 1f));
            float theta = Random.Range(0f, 1f) * 2 * Mathf.PI;

            // Convert radial to cartesian coords
            float x = origin.x + r * Mathf.Cos(theta);
            float y = origin.y + r * Mathf.Sin(theta);
            
            return new Vector2(x, y);
        }

        for (int i = 0; i < numEnemies; i++)
        {
            SpawnSingleEnemy(type, GetPosition());
            yield return new WaitForSeconds(Settings.instance.radiusTimeBetweenSpawns);
        }
        yield return new WaitForSeconds(Settings.instance.radiusTimeAfterSpawn);
        circleClone.GetComponent<SpawnRadiusVisualization>().Destroy();
    }
}