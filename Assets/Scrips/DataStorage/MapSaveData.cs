using UnityEngine;

    public static class MapSaveData
    {
        [System.Serializable]
        public class SaveData
        {
            public int test = 0;

            [System.Serializable]
            public class ArrayX
            {
                public int[] contents;
            }
            public ArrayX[] arrayY;

            public void Populate(HexTile[,] tiles)
            {
                int w = tiles.GetLength(0);
                int h = tiles.GetLength(1);

                arrayY = new ArrayX[h];
                for (int y = 0; y < h; y++)
                {
                    var arrayX = new ArrayX();
                    arrayX.contents = new int[w];
                    arrayY[y] = arrayX;
                    for (int x = 0; x < w; x++)
                    {
                        arrayX.contents[x] = tiles[x, y].type.id;
                    }
                }
            }

            public HexTile[,] ToHexTiles()
            {
                int w = arrayY[0].contents.Length;
                int h = arrayY.Length;
                
                Settings.instance.width = w;
                Settings.instance.height = h;
                GridManager.instance.offset = GridManager.instance.CalculateGridOffset();

                HexTile[,] tiles = new HexTile[w, h];
                for (int y = 0; y < h; y++)
                {
                    for(int x = 0; x < w; x++)
                    {
                        Vector2Int arrayPos = new Vector2Int(x, y);
                        int tileTypeID = arrayY[y].contents[x];
                        TileType type = Settings.instance.tileTypes[tileTypeID];
                        tiles[x, y] = GridManager.instance.InstantiateTile(arrayPos, type);
                    }
                }
                return tiles;
            }
        }

        /*public static void SaveMap(HexTile[,] tiles, int index)
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
        }*/

        public static void SaveMap(HexTile[,] tiles, string saveMapName)
        {
            var dataToSave = new SaveData();
            dataToSave.Populate(tiles);
            
            string jsonData = JsonUtility.ToJson(dataToSave);
            
            System.IO.File.WriteAllText("Assets/GridSaveData" + $"/GridData_{saveMapName}.json", jsonData);
        }

        public static void LoadMap(TextAsset mapJSON)
        {
            SaveData mapData = JsonUtility.FromJson<SaveData>(mapJSON.text);
            var grid = mapData.ToHexTiles();

            GridManager.instance.tiles = grid;
        }
    }