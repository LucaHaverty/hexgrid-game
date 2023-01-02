using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyFinder))]
public class BuildingRangeVisualizetion : MonoBehaviour
{
    public LineRenderer lineRend;
    void Start()
    {
        Utils.GenerateLineRendCircle(lineRend, GetComponent<EnemyFinder>().range, Settings.instance.lineRendCircleNumPoints);
    }
}
