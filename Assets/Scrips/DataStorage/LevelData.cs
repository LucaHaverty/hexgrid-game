using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelData : ScriptableObject
{
    [Header("General Settings")]
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

    [Header("Map Data")] 
    public TextAsset mapToLoad;
    public Vector2 enemyTargetLocation;
}
