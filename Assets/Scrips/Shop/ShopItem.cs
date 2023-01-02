using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    //public int stockLeft; // Set in Inspector for Starting Stock
    public BuildingType type;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private string buildingName;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = ShopManager.instance;
        priceText.text = $"{type.price}";
        nameText.text = buildingName;
    }

    public void OnPointerClick(PointerEventData eventData) => shopManager.OnItemSelected(this);
    public void OnPointerEnter(PointerEventData eventData) => shopManager.OnItemEnter(this);
    public void OnPointerExit(PointerEventData eventData) => shopManager.OnItemExit(this);

    public void SetBackgroundColor(Color color) => backgroundImage.color = color;
}

