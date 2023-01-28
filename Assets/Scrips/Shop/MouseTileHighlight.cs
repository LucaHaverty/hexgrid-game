using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementPreview : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private GameObject previewObject;
    [SerializeField] private SpriteRenderer previewRend;
    
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
        if (selectedTile == null || (selectedTile.hasBuilding && buildingSelected)) parentObject.SetActive(false);
        else
        {
            parentObject.SetActive(true);
            parentObject.transform.position = selectedTile.worldPos;
        }
    }

    private void OnBuildingSelected(ShopItem buidling)
    {
        buildingSelected = true;
        previewRend.sprite = buidling.type.previewSprite;
        
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
