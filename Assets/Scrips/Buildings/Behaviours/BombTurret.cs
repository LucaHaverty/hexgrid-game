using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTurret : AbstractAttackBuilding
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    protected override void Start()
    {
        base.Start();
        partToRotate.transform.localScale *= Settings.instance.tileScale;
        firstAttackDelay = 0.25f;
    }
    
    protected override void Attack()
    {
        GameObject bomb = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bomb.transform.SetParent(Settings.instance.bulletContainer);
        bomb.GetComponent<Bomb>().SetTarget(target.transform);
        bomb.GetComponent<Bomb>().rotation = partToRotate.rotation.eulerAngles.z;

    }
}
