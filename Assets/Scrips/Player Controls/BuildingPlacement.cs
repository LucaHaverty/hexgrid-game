using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BuildingPlacement : MonoBehaviour
{
    public static BuildingPlacement instance;
    private void Awake() { instance = this; }

    public UnityEvent OnBuildingPlaced = new UnityEvent();
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
        var building = ShopManager.instance.GetSelectedBuilding();
        if (!building)
            return;
            
        bool succeeded = BobTheBuilder.AttemptBuild(building, cam.ScreenToWorldPoint(Input.mousePosition));
        if (succeeded) OnBuildingPlaced.Invoke();
    }
}
