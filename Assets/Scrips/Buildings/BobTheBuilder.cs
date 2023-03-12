using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobTheBuilder : MonoBehaviour
{
    public static bool AttemptBuild(BuildingType type, Vector2 pos)
    {
        HexTile parentTile = GridManager.instance.FindCloseTile(pos);
        if (parentTile == null)
            return false;

        if (parentTile.hasBuilding || !parentTile.type.buildable)
            return false;

        if (!MoneyManager.instance.AttemptSubtractMoney(type.price))
        {
            return false;
        }
        
        AbstractBuilding newBuilding = Instantiate(type.prefab, parentTile.worldPos, Quaternion.identity).GetComponent<AbstractBuilding>();
        newBuilding.transform.SetParent(Settings.instance.buildingContainer);
        newBuilding.buildingName = type.buildingName;
        
        parentTile.AddBuilding(newBuilding);
        newBuilding.SetParentTile(parentTile);
        
        if (type.buildingName != BuildingName.EnemyTarget) AudioManager.instance.Play("PlaceBuilding");
        return true;
    }

    public static bool AttemptBuild(BuildingName name, Vector2 pos)
    {
        return AttemptBuild(Settings.instance.BuildingNameToType(name), pos);
    }
}
