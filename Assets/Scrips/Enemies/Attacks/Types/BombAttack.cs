using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BombAttack : AbstractAttack
{
    protected override void Attack()
    {
        target.Damage(damage);
        GetComponent<Health>().InstaKill();
    }
}