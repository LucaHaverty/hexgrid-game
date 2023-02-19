using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class HexTile : MonoBehaviour
{
    public SpriteRenderer tileRend;
    public SpriteRenderer outlineRend;
    
    [HideInInspector]public Vector2Int arrayPos;
    [HideInInspector]public Vector2 worldPos;
    [HideInInspector]public TileType type;
    
    public float gCost;
    public float hCost;
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public HexTile pathfindParent;

    public bool hasBuilding;
    public AbstractBuilding building;

    public HexTile Init(Vector2Int arrayPos, TileType type)
    {
        worldPos = new Vector2((arrayPos.x + (arrayPos.y % 2) / 2f) * Settings.instance.tileScale, arrayPos.y * 0.75f * Settings.instance.tileScale*1.154701f) + GridManager.instance.offset;
        gameObject.transform.position = worldPos;
        gameObject.transform.SetParent(GridManager.instance.transform);
        transform.localScale = new Vector2(Settings.instance.tileScale, Settings.instance.tileScale);

        tileRend.color = type.colorRange.Evaluate(Random.Range(0f, 1f));
        outlineRend.color = Settings.instance.hexOutlineColor.Evaluate(Random.Range(0f, 1f));

        if (type.voidTile)
        {
            tileRend.gameObject.SetActive(false);
            outlineRend.gameObject.SetActive(false);
        }
        
        this.type = type;
        this.arrayPos = arrayPos;
        
        return this;
    }

    public void SetType(TileType newType)
    {
        if (newType == this.type) return;

        if (TryGetComponent(out AnimatedTileColor animColor))
        {
            animColor.SwitchTileColor(this.type.colorRange, newType.colorRange);
        }
        else
        {
            tileRend.color = newType.colorRange.Evaluate(Random.Range(0f, 1f));
        }

        this.type = newType;
        GridManager.instance.OnGridUpdate.Invoke();
    }

    public void SetColor(Color color)
    {
        if (tileRend != null)
            tileRend.color = color;
    }

    public void SetOutlineAlpha(float alpha)
    {
        if (outlineRend != null)
            outlineRend.color = new Color(outlineRend.color.r, outlineRend.color.g, outlineRend.color.b, alpha);
    }

    /** Select what building is assigned to this tile */
    public void AddBuilding(AbstractBuilding newBuilding)
    {
        if (hasBuilding)
            Debug.LogError("Tile already has building");
        
        hasBuilding = true;
        this.building = newBuilding;
    }

    /** Destroy building assigned to this tile */
    public void RemoveBuilding()
    {
        if (building != null)
            Destroy(building.gameObject);
        
        hasBuilding = false;
    }
}
