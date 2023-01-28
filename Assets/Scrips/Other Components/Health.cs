using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health;
    
    [HideInInspector] public UnityEvent OnDeath = new UnityEvent();
    [HideInInspector] public UnityEvent<float> OnHealthUpdate = new UnityEvent<float>();
    
    private float startingHealth;


    private void Awake()
    {
        startingHealth = health;
    }

    public void Damage(float damage)
    {
        if (health <= 0) // Already dead
            return;

        health -= damage;
        if (health <= 0)
            OnDeath.Invoke();

        OnHealthUpdate.Invoke(health / startingHealth);
    }
    public void Heal(float healthGain) => Damage(-healthGain);

    public void InstaKill()
    {
        OnHealthUpdate.Invoke(0);
        Damage(health);
    }
}
