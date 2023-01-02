using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class AbstractBuilding : MonoBehaviour
{
    public Transform spriteTransform;
    
    protected List<AbstractEnemy> enemies = new List<AbstractEnemy>();
    
    private HexTile parentTile;
    public void SetParentTile(HexTile parent) => parentTile = parent;

    protected virtual void Start()
    {
        spriteTransform.localScale *= Settings.instance.tileScale;
        GetComponent<Health>().OnDeath.AddListener(Destroy);
    }

    protected virtual void Update() { }

    public void Destroy()
    {
        parentTile.RemoveBuilding();
    }

    private void FindEnemies()
    {
        
    }
}
