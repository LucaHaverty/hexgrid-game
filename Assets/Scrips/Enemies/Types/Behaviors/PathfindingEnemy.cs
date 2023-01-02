using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PathfindAndMove))]
public class PathfindingEnemy : AbstractEnemy
{
    protected override void Start()
    {
        base.Start();
        GetComponent<PathfindAndMove>().OnHitBuilding.AddListener(OnHitBuilding);
        GetComponent<AbstractAttack>().OnTargetDeath.AddListener(OnTargetDeath);
    }

    private void OnHitBuilding(AbstractBuilding building)
    {
        GetComponent<AbstractAttack>().SetTarget(building.GetComponent<Health>());
        SetState(EnemyState.Attacking);
    }

    private void OnTargetDeath()
    {
        SetState(EnemyState.Moving);
    }
}
