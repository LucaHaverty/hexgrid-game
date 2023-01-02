using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TileType : ScriptableObject 
{
    public int id;
    public string tileName;
    public Gradient colorRange;
    
    public bool walkable;
    public bool buildable;
    
    public float walkWeight;
}
