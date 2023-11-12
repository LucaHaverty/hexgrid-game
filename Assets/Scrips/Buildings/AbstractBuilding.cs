using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class AbstractBuilding : MonoBehaviour
{
    public Transform spriteTransform;
    public BuildingName buildingName;
    
    protected List<AbstractEnemy> enemies = new List<AbstractEnemy>();
    
    private HexTile parentTile;
    public void SetParentTile(HexTile parent) => parentTile = parent;

    protected virtual void Start()
    {
        spriteTransform.localScale *= Settings.instance.tileScale;
        GetComponent<Health>().OnDeath.AddListener(Destroy);
        
        GameObject particles = Instantiate(Settings.instance.buildingDestoryEffect, transform.position, quaternion.identity);
        particles.transform.SetParent(Settings.instance.effectsContainer);
        Destroy(particles, 1);
    }

    protected virtual void Update() { }

    public virtual void Destroy()
    {
        parentTile.RemoveBuilding();
        
        GameObject particles = Instantiate(Settings.instance.buildingDestoryEffect, transform.position, quaternion.identity);
        particles.transform.SetParent(Settings.instance.effectsContainer);
        Destroy(particles, 1);
    }

    private void FindEnemies()
    {
        
    }
}
