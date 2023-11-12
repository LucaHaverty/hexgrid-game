using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health),typeof(AbstractAttack),typeof(AbstractMovement))]
public abstract class AbstractEnemy : MonoBehaviour
{
    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    [SerializeField] private EnemyType enemyType;

    [HideInInspector] public bool visible = true;

    protected virtual void Start()
    {
        visible = true;
        GetComponent<AbstractMovement>()
            .SetTargetPos(GridManager.instance.FindCloseTile(GameManager.instance.levelData.enemyTargetLocation).worldPos);
        currentState = EnemyState.Moving;
        
        GetComponent<Health>().OnDeath.AddListener(OnDeath);
        
        GetComponent<Health>().ApplyDifficulty();
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
        
        GameObject particles = Instantiate(Settings.instance.enemyDeathEffect, Settings.instance.effectsContainer, true);
        Destroy(particles, 2);
        
        particles.transform.position = transform.position;
        particles.transform.localScale *= GetComponentInChildren<RotateSprite>().spritesParent.transform.localScale.x;
        
        var mModule = particles.GetComponent<ParticleSystem>().main;
        Color color = GetComponentInChildren<RotateSprite>().spritesParent
            .GetChild(0).GetComponent<SpriteRenderer>().color;
        mModule.startColor = new ParticleSystem.MinMaxGradient(color, color - new Color(0.2f, 0.2f, 0.2f));
        
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
