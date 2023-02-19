using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInfoDisplay : MonoBehaviour 
{
    public static ShopInfoDisplay instance;

    private void Awake()
    {
        instance = this; 
        HideInfoDiaply();
    }

    [SerializeField] private GameObject parent;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;

    public void UpdateInfoDisplay(ShopItem item)
    {
        parent.SetActive(true);

        if (!item.sellBuildings)
        {
            nameText.text = item.buildingType.shopName;
            priceText.text = item.buildingType.price.ToString();
        }
        else
        {
            nameText.text = "Sell";
            priceText.text = "75%";
        }
        transform.position = new Vector2(item.transform.position.x, transform.position.y);
    }

    public void HideInfoDiaply()
    {
        parent.SetActive(false);
    }
}
