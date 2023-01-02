using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PathfindAndMove : AbstractMovement
{
    [SerializeField] private float buildingWalkWeight;
    [HideInInspector] public UnityEvent<AbstractBuilding> OnHitBuilding = new UnityEvent<AbstractBuilding>();

    private List<HexTile> path;
    private bool hasPath;
    private int currentWaypoint;

    protected override void Start()
    {
        LateStart();
    }

    async void LateStart()
    {
        await Task.Yield();
        FindPath();
        GridManager.instance.OnGridUpdate.AddListener(FindPath);
    }

    void FindPath() 
    {
        void callback(List<HexTile> path, List<HexTile> waypoints, bool success) { this.path = path; hasPath = success; };
        Pathfinding.FindPath(transform.position, targetPos, callback, buildingWalkWeight);

        currentWaypoint = 0;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected override void RunMovement()
    {
        if (!hasPath)
            return;

        UpdatePosition(Vector2.MoveTowards(transform.position, path[currentWaypoint].worldPos, speed*Time.fixedDeltaTime));

        if (!(Vector2.Distance(transform.position, path[currentWaypoint].worldPos) < Settings.instance.targetPosDistanceThreshold))
            return;
        
        currentWaypoint++;
        if (currentWaypoint >= path.Count)
        {
            hasPath = false;
            return;
        }
        if (path[currentWaypoint].hasBuilding)
        {
            OnHitBuilding.Invoke(path[currentWaypoint].building.GetComponent<AbstractBuilding>());
            PauseMovement();
        }
    }
}
