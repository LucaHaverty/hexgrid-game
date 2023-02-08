using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingStatsDisplay : MonoBehaviour 
{
    public static BuildingStatsDisplay instance;
    public GameObject rangeDisplay;
    public GameObject statsDisplay;
    public Image healthBar;
    
    private void Awake() { instance = this; }

    private bool statsActive;
    private Health currentBuildingHealth;

    private void Start()
    {
        BuildingPlacementPreview.instance.onBuildingHighlighted.AddListener(UpdateTowerStatsDisplay);
        BuildingPlacementPreview.instance.onNoBuildingHighlighted.AddListener(HideTowerStatsDiaplay);
    }

    private void Update()
    {
        if (!statsActive)
            return;

        if (currentBuildingHealth == null)
        {
            HideTowerStatsDiaplay();
            return;
        }
        
        healthBar.fillAmount = currentBuildingHealth.GetPercentHealth();
    }

    public void UpdateTowerStatsDisplay(HexTile tile)
    {
        statsActive = true;
        
        statsDisplay.SetActive(true);

        transform.position = tile.transform.position;
        currentBuildingHealth = tile.building.GetComponent<Health>();
        healthBar.fillAmount = currentBuildingHealth.GetPercentHealth();

        if (tile.building.TryGetComponent(out EnemyFinder enemyFinder))
        {
            rangeDisplay.SetActive(true);
            float range = enemyFinder.range;
            rangeDisplay.transform.localScale = new Vector2(range*2, range*2);
        }
        else rangeDisplay.SetActive(false);
    }

    public void HideTowerStatsDiaplay()
    {
        statsActive = false;

        rangeDisplay.SetActive(false);
        statsDisplay.SetActive(false);
    }
}
