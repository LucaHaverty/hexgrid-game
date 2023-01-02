using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveTileTrail : MonoBehaviour
{
    public TileType tileType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile"))
            collision.GetComponent<HexTile>().SetType(tileType);
    }
}
