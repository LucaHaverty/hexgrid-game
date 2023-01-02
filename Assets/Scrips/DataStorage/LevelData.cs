using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelData : ScriptableObject
{
    public int buildTime;
    public int moneyGainPerWave;
    public int startingMoney;
    public int startingHealth;
    
    [Header("Enemy Spawning")]
    public int startingEnemyGroups;
    public int minEnemyGroupSize;
    public int maxEnemyGroupSize;
    public float timeBetweenSpawns;
}
