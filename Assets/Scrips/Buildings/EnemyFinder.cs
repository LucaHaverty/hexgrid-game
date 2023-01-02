using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFinder : MonoBehaviour
{
    public float range;
    public Settings.EnemyFinderMode mode;
    
    public UnityEvent<AbstractEnemy> OnNewEnemyFound = new UnityEvent<AbstractEnemy>();

    private AbstractEnemy currentTarget = null;

    private void Start()
    {
        InvokeRepeating(nameof(FindEnemy), 0, Settings.instance.towerFindEnemeisDelay);
    }

    /** Returns null if no enemy in range */
    public void FindEnemy()
    {
        Transform currentEnemy = null;
        float currentDistance = Mathf.Infinity;

        foreach (Transform enemy in Settings.instance.enemyContainer)
        {
            float distFromTower = Vector2.Distance(transform.position, enemy.position);
            float distFromTarget = GetDistFromTarget(enemy.transform.position, distFromTower);

            if (distFromTower > range)
                continue;
            if (distFromTarget >= currentDistance)
                continue;

            currentEnemy = enemy;
            currentDistance = distFromTarget;
        }

        AbstractEnemy newTarget = null;
        
        if (currentEnemy != null)
            newTarget = currentEnemy.GetComponent<AbstractEnemy>();

        if (newTarget == currentTarget) 
            return;
        
        currentTarget = newTarget;
        OnNewEnemyFound.Invoke(currentTarget);
    }

    /** Calculate the enemies distance based on the EnemyFinderMode */
    public float GetDistFromTarget(Vector2 enemyPos, float distFromTower)
    {
        switch (mode)
        {
            case(Settings.EnemyFinderMode.CloseToTower):
                return distFromTower;
            case(Settings.EnemyFinderMode.CloseToFlag):
                return Vector2.Distance(enemyPos, Settings.instance.flag.position);
            case(Settings.EnemyFinderMode.FarFromFlag):
                return -Vector2.Distance(enemyPos, Settings.instance.flag.position);
        }

        Debug.LogError($"EnemyFinderMode '{mode.ToString()}' Failed");
        return 0;
    }
}
