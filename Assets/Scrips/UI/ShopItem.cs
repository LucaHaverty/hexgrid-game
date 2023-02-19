using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler 
{
    public BuildingType buildingType;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private string buildingName;
    public bool sellBuildings;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = ShopManager.instance;
    }

    public void OnPointerClick(PointerEventData eventData) => shopManager.OnItemSelected(this);
    public void OnPointerEnter(PointerEventData eventData) => shopManager.OnItemEnter(this);
    public void OnPointerExit(PointerEventData eventData) => shopManager.OnItemExit(this);

    public void SetBackgroundColor(Color color) => backgroundImage.color = color;
}

