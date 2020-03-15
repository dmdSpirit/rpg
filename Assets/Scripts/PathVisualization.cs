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
    private LayerMask terrainMask;
    private GameObjectPool pathDotsPool;
    private List<GameObject> usedDots = new List<GameObject>();
    private bool showPath;
    private bool mouseOverUI;

    private void Awake()
    {
        terrainMask = LayerMask.NameToLayer("Terrain");
        pathDotsPool = new GameObjectPool(pathDot, transform);
    }

    private void Start()
    {
        SelectionController.Instance.OnUnitSelected += UnitSelectedHandler;
        SelectionController.Instance.OnUnitUnselect += UnitUnSelectedHandler;
        UIController.Instance.OnMouseEnterUI += MouseEnterUIHandler;
        UIController.Instance.OnMouseExitUI += MouseExitUIHandler;
    }

    // IMPROVE: Calculate path only on actual mouse movement.
    // IMPROVE: Redraw only new parts of path.
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
        var firstCorner = true;
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
        RecalculateShowPath();
    }

    private void UnitUnSelectedHandler(Unit unit)
    {
        unitNavAgent = null;
        unitFloorPosition = Vector3.zero;
        RecalculateShowPath();
    }

    private void ClearDrawnPath()
    {
        if (usedDots.Count == 0) return;
        pathDotsPool.ReleaseObjects(usedDots);
        usedDots.Clear();
    }

    private void MouseExitUIHandler()
    {
        mouseOverUI = false;
        RecalculateShowPath();
    }

    private void MouseEnterUIHandler()
    {
        mouseOverUI = true;
        RecalculateShowPath();
    }

    private void RecalculateShowPath()
    {
        showPath = unitNavAgent != null && unitFloorPosition != null && mouseOverUI == false;
        if (showPath == false)
            ClearDrawnPath();
    }
}