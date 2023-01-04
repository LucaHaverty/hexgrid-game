using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurret : AbstractAttackBuilding
{
    public GameObject bulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePointMid;
    public float bulletSpeed;

    protected override void Start()
    {
        base.Start();
        partToRotate.transform.localScale *= Settings.instance.tileScale;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RotateTurret()
    {
        float idealBulletAngle = Utils.ProjectileTrajectoryPrediction(target.transform.position, target.GetComponent<AbstractMovement>().velocity, target.transform.rotation.eulerAngles.z, firePointMid.position, bulletSpeed, 0);
        Quaternion idealRotation = Quaternion.Euler(0, 0, idealBulletAngle);
        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, idealRotation, rotateSpeed*Time.deltaTime);
    }

    // Alternate shooting from left and right
    private bool justShot1;
    protected override void Attack()
    {
        AudioManager.instance.Play("GunFire");
        
        Vector2 firePos = justShot1 ? firePoint2.position : firePoint1.position;
        justShot1 = !justShot1;
        
        GameObject bullet = Instantiate(bulletPrefab, firePos, partToRotate.rotation);

        bullet.transform.SetParent(Settings.instance.bulletContainer);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(partToRotate.rotation.eulerAngles.z*Mathf.Deg2Rad),Mathf.Sin(partToRotate.rotation.eulerAngles.z*Mathf.Deg2Rad)) * bulletSpeed;
        
        Destroy(bullet, Settings.instance.autoDestroyBulletTime);
    }
}
