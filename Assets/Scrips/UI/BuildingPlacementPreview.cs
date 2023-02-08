using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingPlacementPreview : MonoBehaviour
{
    public static BuildingPlacementPreview instance;

    private void Awake() { instance = this; }

    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private GameObject previewObject;
    [SerializeField] private SpriteRenderer previewRend;
    [SerializeField] private Transform rangeDisplay;

    public UnityEvent<HexTile> onBuildingHighlighted = new UnityEvent<HexTile>();
    public UnityEvent onNoBuildingHighlighted = new UnityEvent();

    private bool buildingSelected;
    
    void Start()
    {
        GridManager.instance.OnSelectedTileChange.AddListener(OnSelectedTileChange);
        ShopManager.instance.OnNewItemSelected.AddListener(OnBuildingSelected);
        ShopManager.instance.OnAllItemsDeselected.AddListener(OnBuildingDeselected);
        BuildingPlacement.instance.OnBuildingPlaced.AddListener(OnBuildingPlaced);
        
        previewObject.transform.localScale *= Settings.instance.tileScale;
    }

    private void OnSelectedTileChange(HexTile selectedTile)
    {
        if (selectedTile == null)
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
    }

    private void OnBuildingSelected(ShopItem building)
    {
        if (building == null)
        {
            OnBuildingDeselected();
            return;
        }
        buildingSelected = true;
        previewRend.sprite = building.buildingType.previewSprite;

        if (building.buildingType.prefab.TryGetComponent(out EnemyFinder enemyFinder))
        {
            rangeDisplay.localScale = Vector2.one * (enemyFinder.range*4);
        }
        else rangeDisplay.localScale = Vector2.zero;
        
                                       highlightObject.SetActive(false);
        previewObject.SetActive(true);
    }
    
    private void OnBuildingDeselected()
    {
        buildingSelected = false;
        
        highlightObject.SetActive(true);
        previewObject.SetActive(false);
    }

    private void OnBuildingPlaced()
    {
        if (buildingSelected)
            parentObject.SetActive(false);
    }
}
