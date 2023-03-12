using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;

    private void Update()
    {
        var tile = GridManager.instance.FindCloseTile(transform.position);
        if (tile == null || !GridManager.instance.FindCloseTile(transform.position).hasBuilding)
            return;
        
        GridManager.instance.FindCloseTile(transform.position).building.gameObject.GetComponent<Health>().Damage(damage);
        Destroy(gameObject);
    }
}