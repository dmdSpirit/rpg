using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathVisualization : MonoBehaviour
{
    [SerializeField] private float minimalRadius = 0.1f;
    [Header("Path dots")] [SerializeField] private GameObject pathDot = default;
    [SerializeField] private float firstDotDistance = 1f;
    [SerializeField] private float pathDotDistance = 1f;

    private NavMeshAgent unitNavAgent;
    private Vector3 unitFloorPosition;
    private bool showPath;
    private LayerMask terrainMask;
    private GameObjectPool pathDotsPool;
    private List<GameObject> usedDots = new List<GameObject>();

    private void Awake()
    {
        terrainMask = LayerMask.NameToLayer("Terrain");
        pathDotsPool = new GameObjectPool(pathDot, transform);
    }

    private void Start()
    {
        SelectionController.Instance.OnUnitSelected += UnitSelectedHandler;
        SelectionController.Instance.OnUnitUnselect += UnitUnSelectedHandler;
    }

    private void Update()
    {
        if (showPath == false ||
            MouseController.Instance.TryGetPointedObject(out var hit) == false ||
            hit.collider.gameObject.layer != terrainMask)
            return;
        var pointOnTerrain = hit.point;
        if (Vector3.Distance(pointOnTerrain, unitNavAgent.transform.position) <= minimalRadius) return;
        var path = new NavMeshPath();
        if (unitNavAgent.CalculatePath(pointOnTerrain, path) == false) return;
        DrawPath(path);
    }

    private void DrawPath(NavMeshPath path)
    {
        ClearDrawnPath();
        bool firstCorner = true;
        for (var i = 1; i < path.corners.Length; ++i)
        {
            var firstPoint = path.corners[i - 1];
            var lastPoint = path.corners[i];
            var distance = Vector3.Distance(firstPoint, lastPoint);
            if (firstCorner)
            {
                firstPoint = Vector3.Lerp(firstPoint, lastPoint, firstDotDistance / distance);
                distance -= firstDotDistance;
                firstCorner = false;
            }

            var dotsCount = (int) Math.Round(distance / pathDotDistance);
            var dots = pathDotsPool.GetObjects(dotsCount);
            for (var j = 0; j < dotsCount; ++j)
            {
                dots[j].SetActive(true);
                dots[j].transform.position = Vector3.Lerp(firstPoint, lastPoint, j * pathDotDistance / distance);
                usedDots.Add(dots[j]);
            }
        }
    }

    private void UnitSelectedHandler(Unit unit)
    {
        unitNavAgent = unit.GetComponent<NavMeshAgent>();
        unitFloorPosition = unit.FloorTransform.position;
        showPath = true;
    }

    private void UnitUnSelectedHandler()
    {
        unitNavAgent = null;
        unitFloorPosition = Vector3.zero;
        showPath = false;
        ClearDrawnPath();
    }

    private void ClearDrawnPath()
    {
        if (usedDots.Count == 0) return;
        pathDotsPool.ReleaseObjects(usedDots);
        usedDots.Clear();
    }
}