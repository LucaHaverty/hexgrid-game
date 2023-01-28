using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    public static void FindPath(HexTile start, HexTile end, System.Action<List<HexTile>, List<HexTile>, bool> callback, float buildingWalkWeight)
    {
        HexTile[,] tiles = GridManager.instance.tiles;

        bool pathFound = false;
        List<HexTile> path = new List<HexTile>();
        List<HexTile> waypoints = new List<HexTile>();

        List<HexTile> openSet = new List<HexTile>();
        HashSet<HexTile> closedSet = new HashSet<HexTile>();

        openSet.Add(start);
        while (openSet.Count > 0)
        {
            HexTile currentTile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentTile.fCost ||
                    openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == end)
            {

                path = RetracePath(start, end);
                waypoints = SimplifyPath(path);
                waypoints.Add(end);
                pathFound = true;
            }

            foreach (HexTile neighbor in GridManager.instance.GetNeighbors(currentTile))
            {
                if (!neighbor.type.walkable /* || neighbor.hasBuilding */ || closedSet.Contains(neighbor)) continue;
                float newMovementCostToNeighbor =
                    currentTile.gCost + GetGridDistance(currentTile.arrayPos, neighbor.arrayPos);

                newMovementCostToNeighbor += currentTile.type.walkWeight;
                if (neighbor.hasBuilding)
                    newMovementCostToNeighbor += buildingWalkWeight;
                newMovementCostToNeighbor += Random.Range(0, GameManager.instance.levelData.randomPathfindWeight);

                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetGridDistance(neighbor.arrayPos, end.arrayPos);
                    neighbor.pathfindParent = currentTile;
                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
        
        if (path.Count > 0)
            path.RemoveAt(path.Count-1); 
        callback(path, waypoints, pathFound);
    }

    public static void FindPath(Vector2 start, Vector2 end, System.Action<List<HexTile>, List<HexTile>, bool> callback, float buildingWalkWeight)
    {
        FindPath(GridManager.instance.FindCloseTile(start), GridManager.instance.FindCloseTile(end), callback, buildingWalkWeight);
    }

    static List<HexTile> RetracePath(HexTile start, HexTile end)
    {
        List<HexTile> path = new List<HexTile>();
        HexTile currentTile = end;

        path.Add(start);
        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.pathfindParent;
        }

        path.Reverse();
        return path;
    }

    // Return Only Waypoints at Turns
    static List<HexTile> SimplifyPath(List<HexTile> path)
    {
        List<HexTile> waypoints = new List<HexTile>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count - 1; i++)
        {
            Vector2 directionNew = new Vector2(path[i].worldPos.x - path[i + 1].worldPos.x,
                path[i].worldPos.y - path[i + 1].worldPos.y);
            if (directionNew == directionOld)
                continue;

            waypoints.Add(path[i]);
            directionOld = directionNew;
        }
        return waypoints;
    }

    static float GetGridDistance(Vector2Int start, Vector2Int end)
    {
        return Vector2.Distance(start, end) / Settings.instance.tileScale * 2;
    }
}