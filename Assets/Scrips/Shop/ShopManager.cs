using UnityEngine;
using UnityEngine.Events;


public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private Transform itemsContrainer;
    [SerializeField] private Color idleColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color selectedColor;

    public UnityEvent<ShopItem> OnNewItemSelected = new UnityEvent<ShopItem>();
    public UnityEvent OnAllItemsDeselected = new UnityEvent();

    private MoneyManager moneyManager;

    private ShopItem[] shopItems;
    private ShopItem activeItem;

    void Start()
    {
        moneyManager = MoneyManager.instance;

        // Populate items Array
        shopItems = new ShopItem[itemsContrainer.childCount];
        for (int i = 0; i < itemsContrainer.childCount; i++)
        {
            shopItems[i] = itemsContrainer.GetChild(i).GetComponent<ShopItem>();
        }

        ResetItems();
    }

    public void OnItemEnter(ShopItem item)
    {
        if (item == activeItem)
            return;

        ResetItems();
        item.SetBackgroundColor(hoverColor);
    }

    public void OnItemExit(ShopItem item)
    {
        ResetItems();
    }

    public void OnItemSelected(ShopItem item)
    {
        item.SetBackgroundColor(selectedColor);
        activeItem = item;
        OnNewItemSelected.Invoke(item);
        
        ResetItems();
    }

    void ResetItems()
    {
        foreach (var item in shopItems)
        {
            if (item == activeItem)
                continue;

            item.SetBackgroundColor(idleColor);
        }
    }

    public BuildingType GetSelectedBuilding()
    {
        if (activeItem == null)
            return Settings.instance.BuildingNameToType(BuildingName.Wall);

        return activeItem.type;
    }
}
