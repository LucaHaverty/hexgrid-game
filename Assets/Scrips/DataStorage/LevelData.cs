using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelData : ScriptableObject
{
    public int initialBuildTime;
    public int repairTime;
    public int moneyGainPerWave;
    public int startingMoney;
    public int startingHealth;
    public float randomPathfindWeight;
    
    [Header("Enemy Spawning")]
    public int startingEnemyGroups;
    public int minEnemyGroupSize;
    public int maxEnemyGroupSize;
    public float timeBetweenSpawns;
}
