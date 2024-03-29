using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BuildingPlacementPreview : MonoBehaviour
{
    public static BuildingPlacementPreview instance;

    private void Awake() { instance = this; }

    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private GameObject previewObject;
    [SerializeField] private GameObject sellObject;
    
    [SerializeField] private SpriteRenderer previewRend;
    [SerializeField] private Transform rangeDisplay;

    [SerializeField] private Color canPlaceBuildingColor;
    [SerializeField] private Color canNotPlaceBuildingColor;

    public UnityEvent<HexTile> onBuildingHighlighted = new UnityEvent<HexTile>();
    public UnityEvent onNoBuildingHighlighted = new UnityEvent();

    private bool buildingSelected;
    private bool sellSelected;
    
    void Start()
    {
        Vector3 scale = highlightObject.transform.localScale;
        highlightObject.transform.localScale = Settings.instance.tileScale * scale;
        sellObject.transform.localScale = Settings.instance.tileScale * scale;

        GridManager.instance.OnSelectedTileChange.AddListener(OnSelectedTileChange);
        ShopManager.instance.OnNewItemSelected.AddListener(OnBuildingSelected);
        ShopManager.instance.OnAllItemsDeselected.AddListener(OnBuildingDeselected);
        BuildingPlacement.instance.OnBuildingPlaced.AddListener(OnBuildingPlaced);
        
        previewObject.transform.localScale *= Settings.instance.tileScale;
        
        GameManager.OnPlayerWon.AddListener(() => {
            parentObject.SetActive(false);
            this.enabled = false;
        });
        GameManager.OnPlayerLose.AddListener(() => {
            parentObject.SetActive(false);
            this.enabled = false;
            Destroy(this);
        });
    }

    private void Update()
    {
        CheckMouseOverUI();
    }

    private void CheckMouseOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (parentObject.activeSelf)
            {
                parentObject.SetActive(false);
                onNoBuildingHighlighted.Invoke();
            }
        } 
        else if (!parentObject.activeSelf)
        {
            OnSelectedTileChange(recentTile);
        }
    }

    private HexTile recentTile = null;
    private void OnSelectedTileChange(HexTile selectedTile)
    {
        recentTile = selectedTile;
        if (selectedTile == null || selectedTile.type.voidTile)
        {
            parentObject.SetActive(false);
            onNoBuildingHighlighted.Invoke();
            return;
        }
        
        if (selectedTile.hasBuilding && buildingSelected) parentObject.SetActive(false);
        else
        {
            parentObject.SetActive(true);
            parentObject.transform.position = selectedTile.worldPos;
        }

        if (selectedTile.hasBuilding)
        {
            onBuildingHighlighted.Invoke(selectedTile);
        }

        if (!selectedTile.hasBuilding)
            onNoBuildingHighlighted.Invoke();

        if (sellSelected)
        {
            parentObject.transform.position = selectedTile.worldPos;
        }

        if (selectedTile.type.buildable)
            previewRend.color = canPlaceBuildingColor;
        else previewRend.color = canNotPlaceBuildingColor;
    }

    private void OnBuildingSelected(ShopItem building)
    {
        if (building == null)
        {
            OnBuildingDeselected();
            return;
        }
        
        if (!building.sellBuildings)
        {
            buildingSelected = true;
            sellSelected = false;
            previewRend.sprite = building.buildingType.previewSprite;

            if (building.buildingType.prefab.TryGetComponent(out EnemyFinder enemyFinder))
            {
                rangeDisplay.localScale = Vector3.one * (enemyFinder.range*4);
            }
            else rangeDisplay.localScale = Vector3.zero;
            
            highlightObject.SetActive(false);
            previewObject.SetActive(true);
            sellObject.SetActive(false);
        }
        else
        {
            sellSelected = true;
            buildingSelected = false;
            rangeDisplay.localScale = Vector3.zero;
            
            highlightObject.SetActive(false);
            previewObject.SetActive(false);
            sellObject.SetActive(true);
        }
    }
    
    private void OnBuildingDeselected()
    {
        buildingSelected = false;
        sellSelected = false;
        
        highlightObject.SetActive(true);
        previewObject.SetActive(false);
        sellObject.SetActive(false);
    }

    private void OnBuildingPlaced()
    {
        if (buildingSelected)
            parentObject.SetActive(false);
    }
}
