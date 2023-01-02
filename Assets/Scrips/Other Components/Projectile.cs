using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float contactDamage;
    public bool destroyOnContact;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<AbstractEnemy>(out var enemy))
        {
            col.GetComponent<Health>().Damage(contactDamage);
            if (destroyOnContact) Destroy(gameObject);
        }
    }
}
