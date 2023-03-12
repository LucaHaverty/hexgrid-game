using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindAndMove))]
public class InvisEnemy : AbstractEnemy
{
    [SerializeField] private SpriteRenderer baseSpriteRend;
    [SerializeField] private Color revealedColor;
    [SerializeField] private RotateSprite rotateSprite;
    protected override void Start()
    {
        base.Start();
        GetComponent<PathfindAndMove>().OnHitBuilding.AddListener(OnHitBuilding);
        GetComponent<AbstractAttack>().OnTargetDeath.AddListener(OnTargetDeath);

        visible = false;
    }

    private void OnHitBuilding(AbstractBuilding building)
    {
        GetComponent<AbstractAttack>().SetTarget(building.GetComponent<Health>());
        SetState(EnemyState.Attacking);

        if (!visible)
        {
            visible = true;
            baseSpriteRend.color = revealedColor;
            rotateSprite.RecalculateAlphas();
        }
    }

    private void OnTargetDeath()
    {
        SetState(EnemyState.Moving);
    }
}
