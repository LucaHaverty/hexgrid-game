using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTurret : AbstractAttackBuilding
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bombSpeed;

    protected override void Start()
    {
        base.Start();
        partToRotate.transform.localScale *= Settings.instance.tileScale;
        firstAttackDelay = 1;
    }
    
    protected override void Attack()
    {
        GameObject bomb = Instantiate(bulletPrefab, firePoint.position, partToRotate.rotation);
        bomb.transform.SetParent(Settings.instance.bulletContainer);
        bomb.GetComponent<Bomb>().StartMovement(target.transform.position, Vector2.Distance(transform.position, target.transform.position), bombSpeed);
    }
}
