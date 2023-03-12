using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AbstractAttack
{
    protected override void Attack()
    {
        target.Damage(damage);
        AudioManager.instance.Play("HitBuilding");
    }
}
