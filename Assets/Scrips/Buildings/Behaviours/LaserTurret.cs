using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LaserTurret : AbstractAttackBuilding
{
    public Transform firePoint;
    public Transform laserSprite;
    public ParticleSystem laserParticles;
    ParticleSystem.ShapeModule particlesShape;
    ParticleSystem.EmissionModule particlesEmission;
    
    public LayerMask layerMask;
    public float damagePerSecond;
    
    private float range;

    public Health toDamage;

    protected override void Start()
    {
        base.Start();

        range = GetComponent<EnemyFinder>().range;
        partToRotate.transform.localScale *= Settings.instance.tileScale;
        
        particlesShape = laserParticles.shape;
        particlesEmission = laserParticles.emission;
    }

    protected override void Update()
    {
        base.Update();

        RaycastForTarget();
        
        if (toDamage != null)
            toDamage.Damage(damagePerSecond*Time.deltaTime);
    }

    private void RaycastForTarget()
    {
        if (target == null)
        {
            laserSprite.gameObject.SetActive(false);
            toDamage = null;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, firePoint.transform.position-transform.position, range, layerMask);

        if (hit.transform == null)
        {
            //UpdateLaserSprite(new Vector2(Mathf.Cos(partToRotate.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(partToRotate.eulerAngles.z * Mathf.Deg2Rad)) * range + (Vector2)transform.position, range);
            laserSprite.gameObject.SetActive(false);
            toDamage = null;
            return;
        }
        UpdateLaserSprite(hit.point, hit.distance);
        toDamage = hit.transform.GetComponent<Health>();
    }

    private void UpdateLaserSprite(Vector2 targetPos, float length)
    {
        if (!laserSprite.gameObject.activeSelf)
        {
            AudioManager.instance.Play("LaserActive");
            laserSprite.gameObject.SetActive(true);
        }

        laserSprite.position = Vector2.Lerp(firePoint.position, targetPos, 0.5f);
        laserSprite.localScale = new Vector2(length, laserSprite.localScale.y);
        laserSprite.rotation = partToRotate.rotation;
        
        // Particles
        particlesShape.radius = length/2;
        particlesEmission.rateOverTime = length * 30f;
    }
    protected override void Attack() { }
}
