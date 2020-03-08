using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum UnitMaster
{
    Player,
    NPC
}

[RequireComponent(typeof(UnitSelection))]
[RequireComponent(typeof(Movement))]
public class Unit : MonoBehaviour
{
    public event Action<Unit> OnUnitDestroyed;

    [SerializeField] private Transform floorTransform = default;
    [SerializeField] private Animator animator = default;
    [SerializeField] private UnitMaster unitMaster = UnitMaster.Player;
    [SerializeField] private CharacterStats stats = new CharacterStats();

    [SerializeField] private Sprite t_UnitPortraitImage = default;

    public Transform FloorTransform => floorTransform;
    public UnitMaster UnitMaster => unitMaster;
    public Sprite UnitPortraitImage => t_UnitPortraitImage;
    public CharacterStats Stats => stats;

    private UnitSelection selection;
    private Movement movement;

    private int Moving = Animator.StringToHash("Moving");
    private int Idle = Animator.StringToHash("Idle");

    private void Awake()
    {
        selection = GetComponent<UnitSelection>();
        movement = GetComponent<Movement>();
        movement.OnUnitBeginMoving += BeginMovingHandler;
        movement.OnUnitEndMoving += EndMovingHandler;
    }

    private void Start()
    {
        UnitManager.Instance.SubscribeUnit(this);
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

    private void OnDestroy()
    {
        OnUnitDestroyed?.Invoke(this);
    }
}