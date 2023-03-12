using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PathfindAndMove))]
public class HardBossBehavior : AbstractEnemy 
{
    public float timeBetweenAttack;
    public int[] numMinions;
    public EnemyName[] enemyToSpawn;
    public int numBullets;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
    private PathfindAndMove movement;
    protected override void Start()
    {
        base.Start();

        this.movement = GetComponent<PathfindAndMove>();
        GetComponent<PathfindAndMove>().OnHitBuilding.AddListener(OnHitBuilding);
        GetComponent<AbstractAttack>().OnTargetDeath.AddListener(OnTargetDeath);

        StartCoroutine(AttackCycle());
    }

    private void OnHitBuilding(AbstractBuilding building)
    {
        GetComponent<AbstractAttack>().SetTarget(building.GetComponent<Health>());
        SetState(EnemyState.Attacking);
    }

    private void OnTargetDeath()
    {
        SetState(EnemyState.Moving);
    }

    private void SpawnMinions()
    {
        int numMinion = Random.Range(numMinions[0], numMinions[1] + 1);
        for (int i = 0; i < numMinion; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            Vector2 pos = (Vector2)transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            HexTile spawnTile = GridManager.instance.FindCloseTile(pos);
            
            if (spawnTile == null) continue;
            if (spawnTile.hasBuilding || !spawnTile.type.walkable || spawnTile.type.voidTile) continue;

            EnemyName toSpawn = enemyToSpawn[Random.Range(0, enemyToSpawn.Length)];
            EnemySpawner.SpawnSingleEnemy(toSpawn, pos);    
        }
    }

    private void Shoot()
    {
        for (int i = 0; i < numBullets; i++)
        {
            float angle = i * (360f / numBullets);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle+90f));
            bullet.transform.SetParent(Settings.instance.bulletContainer);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up*bulletSpeed;
            
            Destroy(bullet, Settings.instance.autoDestroyBulletTime);
        }
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttack);
            
            movement.PauseMovement();
            yield return new WaitForSeconds(1f);
            SpawnMinions();
            yield return new WaitForSeconds(0.5f);
            movement.ResumeMovement();

            yield return new WaitForSeconds(timeBetweenAttack);
            movement.PauseMovement();
            yield return new WaitForSeconds(1f);
            Shoot();
            yield return new WaitForSeconds(0.2f);
            Shoot();
            yield return new WaitForSeconds(0.5f);
            movement.ResumeMovement();
        }
    }
}
