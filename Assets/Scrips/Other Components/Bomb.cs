using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRange;
    public float damage;
    public float rotateSpeed;
    public float moveSpeed;
    public GameObject radialExplosionEffect;
    public Transform sprite;

    private Transform target;
    public float rotation;
    
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Start()
    {
        sprite.rotation = Quaternion.Euler(0, 0, rotation);
        Invoke(nameof(Explode), Settings.instance.autoMissileDestroyTime);
    }

    private void Update()
    {
        if (target == null)
        {
            FindNewTarget();
            return;
        }

        // Turn towards target
        float targetAngle = Utils.RotateTowardsDegrees2D(target.transform.position, transform.position);
        rotation = Quaternion.Slerp(quaternion.Euler(0, 0, rotation*Mathf.Deg2Rad), quaternion.Euler(0, 0, targetAngle*Mathf.Deg2Rad), rotateSpeed * Time.deltaTime).eulerAngles.z;
        sprite.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void FixedUpdate()
    {
        // Movement
        transform.Translate(Utils.AngleToVectorDegrees(rotation) * (moveSpeed*Time.fixedDeltaTime));
    }

    private void FindNewTarget()
    {
        Transform currentEnemy = null;
        float currentDistance = Mathf.Infinity;

        foreach (Transform enemy in Settings.instance.enemyContainer)
        {
            if (!enemy.GetComponent<AbstractEnemy>().visible)
                continue;
        
            float distFromTower = Vector2.Distance(transform.position, enemy.position);
            float distFromTarget = distFromTower;

            if (distFromTarget >= currentDistance)
                continue;

            currentEnemy = enemy;
            currentDistance = distFromTarget;
        }

        target = currentEnemy;
    }

    private void Explode()
    {
        AudioManager.instance.Play("Explosion");
        CameraShaker.Instance.ShakeOnce(1.6f, 5f, .1f, .5f);

        DamageEnemiesInRange();
        
        GameObject effect = Instantiate(radialExplosionEffect, transform.position, Quaternion.identity);
        effect.transform.SetParent(Settings.instance.effectsContainer);
        effect.transform.localScale = new Vector2(explosionRange, explosionRange);
        Destroy(effect, 1f);
        
        Destroy(gameObject);
    }
    
    private void DamageEnemiesInRange()
    {
        List<Health> enemies = new List<Health>();
        foreach (Transform enemy in Settings.instance.enemyContainer)
        {
            float dist = Vector2.Distance(transform.position, enemy.position);
            
            if (dist < explosionRange)
                enemy.GetComponent<Health>().Damage(damage);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<AbstractEnemy>(out var enemy))
        {
            Explode();
        }
    }
}
