using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BombAttack : AbstractAttack
{
    public float range;

    private bool exploded = false;

    protected override void Attack()
    {
        if (exploded == true)
        {
            Debug.LogError("Exploded called more than once in BombAttack.cs");
            return;
        }

        foreach (Transform building in Settings.instance.buildingContainer)
        {
            if (Vector2.Distance(transform.position, building.transform.position) > range)
                continue;
            
            building.GetComponent<Health>().Damage(damage);
        }
        GetComponent<Health>().InstaKill();
    }
}