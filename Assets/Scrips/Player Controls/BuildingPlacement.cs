using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacement : MonoBehaviour
{
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !Input.GetKeyDown(KeyCode.LeftShift) && !EventSystem.current.IsPointerOverGameObject())
            BuildSelected();
    }

    private void BuildSelected()
    {
        BobTheBuilder.AttemptBuild(ShopManager.instance.GetSelectedBuilding(), cam.ScreenToWorldPoint(Input.mousePosition));
    }
}
