using UnityEngine;

[CreateAssetMenu]
public class BuildingType : ScriptableObject
{
    public BuildingName buildingName;
    public GameObject prefab;
    public int price;
    public Sprite previewSprite;

    [HideInInspector]
    public AbstractBuilding behavior
    {
        get
        {
            return prefab.GetComponent<AbstractBuilding>();
        }
    }
}

public enum BuildingName
{
    Wall,
    GunTurret,
    LaserTurret,
    BombTurret
}

