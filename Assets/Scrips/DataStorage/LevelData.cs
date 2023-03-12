using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu()]
public class LevelData : ScriptableObject
{
    [Header("General Settings")] 
    public int levelID;
    public int initialBuildTime;
    public int repairTime;
    public int moneyGainPerWave;
    public int startingMoney;
    public int startingHealth;
    public float randomPathfindWeight;

    [Header("Enemy Spawning")] 
    public WaveData waveData;
    public int startingEnemyGroups;
    public int minEnemyGroupSize;
    public int maxEnemyGroupSize;
    public float timeBetweenSpawns;
    [SerializeField] private Vector3[] spawnMin;
    [SerializeField] private Vector3[] spawnMax;

    public Vector2 GetSpawnLocation()
    {
        int zone = Random.Range(0, spawnMin.Length);
        Vector2 min = spawnMin[zone];
        Vector2 max = spawnMax[zone];
        Vector2 position = new Vector2(Mathf.Lerp(min.x, max.x, Random.Range(0f, 1f)),
            Mathf.Lerp(min.y, max.y, Random.Range(0f, 1f)));

        var tile = GridManager.instance.FindCloseTile(position);
        if (tile != null && tile.type.walkable)
            return position;
        else return GetSpawnLocation();
    }

    [Header("Map Data")] 
    public TextAsset mapToLoad;
    public Vector2 enemyTargetLocation;
    public Vector2 camOffset;
    public Vector2 bossSpawnLocation;
    public float camSize;
}
