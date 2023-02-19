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
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ||
            EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButton(0))
            BuildSelected();
    }

    private void BuildSelected()
    {
        if (ShopManager.instance.getSellSelected() && GridManager.instance.GetSelectedTile().hasBuilding)
        {
            AbstractBuilding toDestroy = GridManager.instance.GetSelectedTile().building;
            if (toDestroy.buildingName == BuildingName.EnemyTarget)
                return;

            MoneyManager.instance.AttemptAddMoney(
                Mathf.CeilToInt(Settings.instance.BuildingNameToType(toDestroy.buildingName).price * 0.75f));
            toDestroy.Destroy();
        }
        
        var building = ShopManager.instance.GetSelectedBuilding();
        if (building == null)
            return;
            
        bool succeeded = BobTheBuilder.AttemptBuild(building, cam.ScreenToWorldPoint(Input.mousePosition));
        if (succeeded) OnBuildingPlaced.Invoke();
    }
}
