using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(AbstractEnemy))]
public abstract class AbstractAttack : MonoBehaviour
{
    public float attackCooldown;
    public bool startAttackingOnInit;
    private bool currentlyAttacking;
    public float damage;

    public UnityEvent OnTargetDeath = new UnityEvent();
    
    protected Health target;

    public virtual void Start()
    {
        if (startAttackingOnInit)
            currentlyAttacking = startAttackingOnInit;
    }
    public virtual void Update() { }

    private void AttemptAttack()
    {
        if (target == null)
            TargetJustDied();
        
        Attack();
        
        if (target.health <= 0)
            TargetJustDied();
    }
    protected abstract void Attack();

    public void PauseAttacking()
    {
        currentlyAttacking = false;
        CancelInvoke(nameof(AttemptAttack));
    }

    public void ResumeAttacking()
    {
        currentlyAttacking = true;
        InvokeRepeating(nameof(AttemptAttack), attackCooldown/4f, attackCooldown);
    }

    public bool IsAttacking() { return this.currentlyAttacking; }
    public void SetTarget(Health newTarget) => target = newTarget;

    private void TargetJustDied()
    {
        OnTargetDeath.Invoke();
        PauseAttacking();
    }
}
