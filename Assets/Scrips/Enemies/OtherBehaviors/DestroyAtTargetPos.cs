using System.Collections;
using System.Collections.Generic;
using Scrips;
using UnityEngine;

[RequireComponent(typeof(AbstractMovement))]
public class DestroyAtTargetPos : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<AbstractMovement>().DistanceFromTarget() < Settings.instance.targetPosDistanceThreshold)
        {
            PlayerHealthManager.instance.SubtractHealth(1);
            GetComponent<AbstractEnemy>().TriggerDestroy();
        }
    }
}
