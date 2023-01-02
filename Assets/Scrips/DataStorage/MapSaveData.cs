using UnityEngine;

namespace Scrips
{
    public static class MapSaveData
    {
        public static void SaveMap(HexTile[,] tiles, int index)
        {
            for (int y = 0; y < Settings.instance.height; y++)
            {
                for (int x = 0; x < Settings.instance.width; x++)
                {
                    HexTile entry = GridManager.instance.tiles[x, y];
                    PlayerPrefs.SetInt($"SaveTile{index}{entry.arrayPos.ToString()}", entry.type.id);

                }
            }
        }

        public static HexTile[,] LoadMap(int index)
        {
        foreach (HexTile tile in GridManager.instance.tiles) {
            GridManager.Destroy(tile.gameObject);
        }
        
            HexTile[,] tiles = new HexTile[Settings.instance.width, Settings.instance.height];
            for (int y = 0; y < Settings.instance.height; y++)
            {
                for(int x = 0; x < Settings.instance.width; x++)
                {
                    Vector2Int arrayPos = new Vector2Int(x, y);
                    int tileTypeID = PlayerPrefs.GetInt($"SaveTile{index}{arrayPos.ToString()}");
                    TileType type = Settings.instance.tileTypes[tileTypeID];
                    tiles[x, y] = GridManager.instance.InstantiateTile(arrayPos, type);
                }
            }
            return tiles;
        }
    }
}
