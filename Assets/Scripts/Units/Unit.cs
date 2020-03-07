using System;
using UnityEngine;

[RequireComponent(typeof(UnitSelection))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform floorTransform = default;
    [SerializeField] private Animator animator = default;

    public Transform FloorTransform => floorTransform;

    private UnitSelection selection;
    private Movement movement;

    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int Idle = Animator.StringToHash("Idle");

    private void Awake()
    {
        selection = GetComponent<UnitSelection>();
        movement = GetComponent<Movement>();
        movement.OnUnitBeginMoving += BeginMovingHandler;
        movement.OnUnitEndMoving += EndMovingHandler;
    }

    public void Select()
    {
        selection.IsSelected = true;
    }

    public void UnSelect()
    {
        selection.IsSelected = false;
    }

    public void MoveTo(Vector3 destination)
    {
        movement.StartMoving(destination);
    }

    private void BeginMovingHandler(Unit unit)
    {
        animator.SetTrigger(Moving);
    }

    private void EndMovingHandler(Unit unit)
    {
        animator.SetTrigger(Idle);
    }
}