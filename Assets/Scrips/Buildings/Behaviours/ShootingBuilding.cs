using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBuilding : AbstractAttackBuilding
{
    public GameObject bulletPrefab; 
    public Transform firePoint;
    public float bulletSpeed;

    protected override void Start()
    {
        base.Start();
        partToRotate.transform.localScale *= Settings.instance.tileScale;
    }
    protected override void Attack()
    {
        AudioManager.instance.Play("GunFire");
        
        Vector2 bulletVelocity = (target.transform.position - transform.position).normalized;

        float idealBulletAngle = Utils.ProjectileTrajectoryPrediction(target.transform.position, target.GetComponent<AbstractMovement>().velocity, target.transform.rotation.eulerAngles.z, firePoint.position, bulletSpeed, 0);
        partToRotate.transform.rotation = Quaternion.Euler(0, 0, idealBulletAngle);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, idealBulletAngle));

        bullet.transform.SetParent(Settings.instance.bulletContainer);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(idealBulletAngle*Mathf.Deg2Rad),Mathf.Sin(idealBulletAngle*Mathf.Deg2Rad)) * bulletSpeed;
        
        Destroy(bullet, Settings.instance.autoDestroyBulletTime);
    }
}
