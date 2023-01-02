using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyFinder))]
public abstract class AbstractAttackBuilding : AbstractBuilding
{
    public float attackCooldown;
    public bool disableTimedAttack;
    public float rotateSpeed;
    public Transform partToRotate;

    protected AbstractEnemy target;
    protected float firstAttackDelay;
    
    private bool idle;

    protected override void Start()
    {
        base.Start();
        GetComponent<EnemyFinder>().OnNewEnemyFound.AddListener(SetTarget);
        if (!disableTimedAttack) 
            InvokeRepeating(nameof(AttemptAttack), attackCooldown, attackCooldown);
    }

    protected override void Update()
    {
        base.Update();
        
        if (target != null)
            RotateTurret();
    }

    private void SetTarget(AbstractEnemy newTarget)
    {
        target = newTarget;
        
        if (!idle || disableTimedAttack) 
            return;
        
        idle = false;
        InvokeRepeating(nameof(AttemptAttack), firstAttackDelay, attackCooldown);
    }

    private void AttemptAttack()
    {
        if (target == null)
        {
            idle = true;
            CancelInvoke(nameof(AttemptAttack));
            return;
        }
        Attack();
    }

    protected virtual void Attack() { }

    protected virtual void RotateTurret()
    {
        Quaternion targetRotation = Utils.rotateTowardsQuaternion2D(target.transform.position, transform.position);
        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, targetRotation, rotateSpeed*Time.deltaTime);
    }
}
