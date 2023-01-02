using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyType : ScriptableObject
{
    public EnemyName enemyName;
    public GameObject prefab;

    [HideInInspector]
    public AbstractEnemy behavior => prefab.GetComponent<AbstractEnemy>();
    [HideInInspector]
    public AbstractMovement movement => prefab.GetComponent<AbstractMovement>();
    [HideInInspector]
    public AbstractAttack attack => prefab.GetComponent<AbstractAttack>();
}

public enum EnemyName
{
    TestEnemy,
    FastEnemy,
    TankEnemy,
    BombEnemy
}
