using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    void Awake() { instance = this; }

    [Header("Grid")]
    public HexTile tilePrefab;
    public int width;
    public int height;
    public float tileScale;
    public float animateColorSpeed;
    public float colorSwitchAnimSpeed;
    public Gradient gridColor;
    public TileType baseTile;
    public TileType[] tileTypes;
    
    public Gradient hexOutlineColor;

    [Header("Towers & Units")]
    public float targetPosDistanceThreshold;
    public float towerFindEnemeisDelay;
    public float autoDestroyBulletTime;
    public float autoMissileDestroyTime;
    public Gradient damageFlashColor;
    public Color enemyDamageColor;
    public float damageFlashTime;
    public enum EnemyFinderMode
    {
        CloseToTower,
        CloseToFlag,
        FarFromFlag
    }

    public BuildingType[] buildings;
    
    public BuildingType BuildingNameToType(BuildingName buildingName)
    {
        foreach (BuildingType building in buildings)
        {
            if (building.buildingName == buildingName)
                return building;
        }
        Debug.LogError($"Building type {buildingName} not found");
        return null;
    }
    
    public GameObject GetBuildingPrefab(BuildingName buildingName) { return BuildingNameToType(buildingName).prefab; }

    [Header("Transform Refs")]
    public Transform enemyContainer;
    public Transform buildingContainer;
    public Transform bulletContainer;
    public Transform effectsContainer;

    [Header("Enemy Spawner")]
    public EnemyType[] enemies;
 
    public EnemyType EnemyNameToType(EnemyName enemyName)
    {
        foreach (EnemyType enemy in enemies)
        {
            if (enemy.enemyName == enemyName)
                return enemy;
        }
        Debug.LogError($"Enemy type {enemyName} not found");
        return null;
    }
    
    public GameObject GetBuildingPrefab(EnemyName enemyName) { return EnemyNameToType(enemyName).prefab; }

    [Header("Group spawn in radius")] 
    public float radiusTimeBetweenSpawns;
    public float radiusTimeBeforeSpawn;
    public float radiusTimeAfterSpawn;
    public GameObject radiusCirclePrefab;
    
    [Header("Other")] 
    public int lineRendCircleNumPoints;
    public GameObject buildingDestoryEffect;
    public GameObject enemyDeathEffect;
    public int autoStartWaveTime;
    public int autoStartFirstWaveTime;

    [Header("Difficulty Scaling")] 
    public float enemyHealthDiffScale;
    public float enemySpeedDiffScale;
}
