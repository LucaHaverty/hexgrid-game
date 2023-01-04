using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditing : MonoBehaviour
{
    int selection;

    void Update()
    {
        UpdateSelection();

        /*if (Input.GetMouseButton(0))
            SetSelectedTile();*/
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
    }

    void SetSelectedTile()
    {
        if (Settings.instance.tileTypes.Length <= selection)
            return;

        if (GridManager.instance.GetSelectedTile() != null)
            GridManager.instance.GetSelectedTile().SetType(Settings.instance.tileTypes[selection]);
    }
}
