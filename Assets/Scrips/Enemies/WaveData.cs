using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
    public Wave[] waves;
    public int numWaves { get { return waves.Length; } }
}

[System.Serializable]
public class Wave
{
    public EnemyGroup[] enemyGroups;
}

[System.Serializable]
public class EnemyGroup
{
    [SerializeField] private EnemyName[] enemyTypes = { EnemyName.BasicEnemy };

    public EnemyName GetEnemyType()
    {
        return enemyTypes[Random.Range(0, enemyTypes.Length)];
    }
    
    public int numEnemies = 1;
    public float timeBetweenSpawns = 0.25f;
    public float timeIdleAfterGroup = 1;
}
