using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditing : MonoBehaviour
{
    public string saveMapName;
    int selection;

    void Update()
    {
        UpdateSelection();

        if (Input.GetKey(KeyCode.B))
            SetSelectedTile();

        if (Input.GetKeyDown(KeyCode.S))
            MapSaveData.SaveMap(GridManager.instance.tiles, saveMapName);
    }

    void UpdateSelection()
    {
        if (GridManager.instance.GetSelectedTile() == null) 
            return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) selection = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) selection = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) selection = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) selection = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5)) selection = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6)) selection = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha7)) selection = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha8)) selection = 7;
        else if (Input.GetKeyDown(KeyCode.Alpha8)) selection = 9;
    }

    void SetSelectedTile()
    {
        if (Settings.instance.tileTypes.Length <= selection)
            return;

        if (GridManager.instance.GetSelectedTile() != null)
            GridManager.instance.GetSelectedTile().SetType(Settings.instance.tileTypes[selection]);
    }
}
