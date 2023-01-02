using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPosition : AbstractMovement
{
    protected override void RunMovement()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, speed*Time.fixedDeltaTime);
        UpdatePosition(newPos);
    }
}
