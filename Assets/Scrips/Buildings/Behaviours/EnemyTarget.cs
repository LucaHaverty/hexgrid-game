using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : AbstractBuilding
{
    public override void Destroy()
    {
        GameManager.SetGameState(GameState.PlayerLost);
        base.Destroy();
    }
}
