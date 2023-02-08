using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health),typeof(AbstractAttack),typeof(AbstractMovement))]
public abstract class AbstractEnemy : MonoBehaviour
{
    [SerializeField]
    private EnemyState currentState = EnemyState.Idle;

    [SerializeField] private EnemyType enemyType;

    protected virtual void Start()
    {
        GetComponent<AbstractMovement>()
            .SetTargetPos(GridManager.instance.FindCloseTile(Settings.instance.enemyTarget.position).worldPos);
        currentState = EnemyState.Moving;
        
        GetComponent<Health>().OnDeath.AddListener(OnDeath);
    }
    protected virtual  void Update()
    {
        
    }

    public void TriggerDestroy()
    {
        Destroy(gameObject);
    }

    public void OnDeath()
    {
        GameManager.OnEnemyKilled(enemyType, transform.position);
        TriggerDestroy();
    }

    protected void SetState(EnemyState newState)
    {
        if (newState == currentState)
            return;

        switch (currentState)
        {
            case (EnemyState.Moving):
                GetComponent<AbstractMovement>().PauseMovement();
                break;
            case (EnemyState.Attacking):
                GetComponent<AbstractAttack>().PauseAttacking();
                break;
        }
        
        switch (newState)
        {
            case (EnemyState.Moving):
                GetComponent<AbstractMovement>().ResumeMovement();
                break;
            case(EnemyState.Attacking):
                GetComponent<AbstractAttack>().ResumeAttacking();
                break;
        }
        currentState = newState;
    }

    public Vector2 GetVelocity()
    {
        return GetComponent<AbstractMovement>().velocity;
    }

    public enum EnemyState
    {
        Moving,
        Attacking,
        Idle
    }
}
