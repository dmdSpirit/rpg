using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PathVisualization : MonoBehaviour
{
    private void Update()
    {
        if (SelectionController.Instance.isSomethingSelected == false) return;
        var unit = SelectionController.Instance.selectedUnit;
        var navAgent = unit.GetComponent<NavMeshAgent>();
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            NavMeshPath path = new NavMeshPath();
            if (navAgent.CalculatePath(hit.transform.position, path))
            {
                Debug.DrawLine(unit.FloorTransform.position, path.corners[0], Color.red);
                for (var i = 1; i < path.corners.Length; ++i)
                {
                    Debug.DrawLine(path.corners[i - 1], path.corners[i], Color.red);
                }
            }
        }
    }
}