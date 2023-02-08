using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRangeVisualizetion : MonoBehaviour
{
    public LineRenderer lineRend;
    void Start()
    {
        Utils.GenerateLineRendCircle(lineRend, 0.5f, Settings.instance.lineRendCircleNumPoints);
    }
}
