using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitTimed : MonoBehaviour
{
    public Transform spawnPoint;
    public float spawnDelay;
    public bool spawnImmediately;

    void Start() => InvokeRepeating(nameof(SpawnUnit), spawnImmediately ? 0 : spawnDelay, spawnDelay);

    void SpawnUnit() => EnemySpawner.SpawnSingleEnemy(EnemyName.TestEnemy, spawnPoint.position);
}
