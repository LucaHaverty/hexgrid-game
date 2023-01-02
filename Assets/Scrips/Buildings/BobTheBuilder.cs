using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobTheBuilder : MonoBehaviour
{
    public static void AttemptBuild(BuildingType type, Vector2 pos)
    {
        HexTile parentTile = GridManager.instance.FindCloseTile(pos);
        if (parentTile == null)
            return;

        if (parentTile.hasBuilding)
            return;
        
        if (!MoneyManager.instance.AttemptSubtractMoney(type.price))
        {
            return;
        }
        
        AbstractBuilding newBuilding = Instantiate(type.prefab, parentTile.worldPos, Quaternion.identity).GetComponent<AbstractBuilding>();
        newBuilding.transform.SetParent(Settings.instance.buildingContainer);

        parentTile.AddBuilding(newBuilding);
        newBuilding.SetParentTile(parentTile);
    }
}
