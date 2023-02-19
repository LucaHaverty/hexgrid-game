using System.Collections;
using System.Collections.Generic;
using Scrips;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    void Awake() { instance = this; }

    public GameObject highlightSprite;
    public HexTile tilePrefab;
    public HexTile selectedTile;

    public HexTile[,] tiles;

    public Vector2 offset;

    public UnityEvent OnGridUpdate = new UnityEvent();
    public UnityEvent<HexTile> OnSelectedTileChange = new UnityEvent<HexTile>();

    void Start()
    {
        offset = CalculateGridOffset();
        CreateGrid();
        
    }

    private void Update()
    {
        TileSelection();
    }
    public void TileSelection()
    {
        var prevTile = selectedTile;
        selectedTile = FindCloseTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        if (selectedTile != prevTile) OnSelectedTileChange.Invoke(selectedTile);
    }

    void CreateGrid()
    {
        tiles = new HexTile[Settings.instance.width, Settings.instance.height];
        for (int y = 0; y < Settings.instance.height; y++)
        {
            for (int x = 0; x < Settings.instance.width; x++)
            {
                HexTile tile = InstantiateTile(new Vector2Int(x, y), Settings.instance.baseTile);
                tiles[x, y] = tile;
            }
        }
    }

    public HexTile GetSelectedTile()
    {
        return selectedTile;
    }

    public HexTile InstantiateTile(Vector2Int pos, TileType type)
    {
        return Instantiate(tilePrefab).Init(pos, type);
    }

    public Vector2 CalculateGridOffset()
    {
        return new Vector2(-(Settings.instance.width) / (1/Settings.instance.tileScale * 2f) + 0.25f, -(Settings.instance.height - 1f) / (1/Settings.instance.tileScale*1.154701f * 2f));
    }

    public List<HexTile> GetNeighbors(Vector2Int center)
    {
        List<HexTile> neighbors = new List<HexTile>();

        void AddTile(int x, int y) { 
            if (x >= 0 && x < Settings.instance.width && y >= 0 && y < Settings.instance.height)
                neighbors.Add(tiles[x, y]); }

        AddTile(center.x - 1, center.y); // Left
        AddTile(center.x + 1, center.y); // Right
        AddTile(center.x, center.y + 1); // Up
        AddTile(center.x, center.y - 1); // Down
        AddTile(center.x + ((center.y % 2 == 1) ? 1 : -1), center.y + 1); // Up Diagonal
        AddTile(center.x + ((center.y % 2 == 1) ? 1 : -1), center.y - 1); // Down Diagonal

        return neighbors;
    }
    public List<HexTile> GetNeighbors(HexTile center) { return GetNeighbors(center.arrayPos); }

    public HexTile FindCloseTile(Vector2 inputPos)
    {
        Vector2 pos = inputPos - offset;
        pos = new Vector2(pos.x / Settings.instance.tileScale, pos.y / 0.86602575f / Settings.instance.tileScale);

        Vector2Int roundedTilePos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        HexTile roundedTile = (roundedTilePos.x >= 0 && roundedTilePos.x < Settings.instance.width && roundedTilePos.y >= 0 && roundedTilePos.y < Settings.instance.height) ? tiles[roundedTilePos.x, roundedTilePos.y] : null;

        if (roundedTile == null) return null;

        float closeDist = Vector2.Distance(inputPos, roundedTile.worldPos);
        HexTile closeTile = roundedTile;

        foreach (HexTile tile in GetNeighbors(roundedTilePos))
        {
            float dist = Vector2.Distance(inputPos, tile.worldPos);
            if (dist < closeDist) { closeDist = dist; closeTile = tile; }
        }

        return closeTile;
    }
}
