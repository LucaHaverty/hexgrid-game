using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BombAttack : AbstractAttack
{
    public float range;
    public float radialDamageFalloff;
    public GameObject radialExplosionEffect;

    private bool exploded = false;

    protected override void Attack()
    {
        if (exploded == true)
        {
            Debug.LogError("Exploded called more than once in BombAttack.cs");
            return;
        }

        exploded = true;

        foreach (Transform building in Settings.instance.buildingContainer)
        {
            float distance = Vector2.Distance(transform.position, building.transform.position);
            if (distance > range)
                continue;

            float appliedDamage = Mathf.Lerp(damage, damage / radialDamageFalloff, distance / range);
            building.GetComponent<Health>().Damage(damage);
        }

        GameObject effect = Instantiate(radialExplosionEffect, transform.position, Quaternion.identity);
        effect.transform.SetParent(Settings.instance.effectsContainer);
        effect.transform.localScale = new Vector2(range*2, range*2);
        Destroy(effect, 1f);
        
        GetComponent<Health>().InstaKill();
    }
}