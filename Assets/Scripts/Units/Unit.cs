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
    public event Action OnHPChanged;
    public event Action OnPhysicalArmorChanged;
    public event Action OnMagicalArmorChanged;

    [SerializeField] private Transform floorTransform = default;
    [SerializeField] private Animator animator = default;
    [SerializeField] private UnitMaster unitMaster = UnitMaster.Player;
    [SerializeField] private CharacterStats stats = new CharacterStats();

    [SerializeField] private Sprite t_UnitPortraitImage = default;

    public int MaxHP => stats.hp;
    public int MaxPhysicalArmor => stats.physicalArmor;
    public int MaxMagicArmor => stats.magicalArmor;

    public int HP
    {
        get => hp;
        set
        {
            if (value == hp) return;
            hp = value;
            OnHPChanged?.Invoke();
            if (hp <= 0)
                Die();
        }
    }

    public int MagicalArmor
    {
        get => magicalArmor;
        set
        {
            if (value == magicalArmor) return;
            if (value < 0)
            {
                hp += value;
                magicalArmor = 0;
            }
            else
                magicalArmor = value;

            OnMagicalArmorChanged?.Invoke();
        }
    }

    public int PhysicalArmor
    {
        get => physicalArmor;
        set
        {
            if (value == physicalArmor) return;
            if (value < 0)
            {
                hp += value;
                physicalArmor = 0;
            }
            else
                physicalArmor = value;

            OnPhysicalArmorChanged?.Invoke();
        }
    }

    public Transform FloorTransform => floorTransform;
    public UnitMaster UnitMaster => unitMaster;
    public Sprite UnitPortraitImage => t_UnitPortraitImage;
    public CharacterStats Stats => stats;
    public int Initiative => Mathf.Max(stats.wits, 10);

    private UnitSelection selection;
    private Movement movement;

    private int Moving = Animator.StringToHash("Moving");
    private int Idle = Animator.StringToHash("Idle");

    private int hp;
    private int physicalArmor;
    private int magicalArmor;

    private void Awake()
    {
        selection = GetComponent<UnitSelection>();
        movement = GetComponent<Movement>();
        movement.OnUnitBeginMoving += BeginMovingHandler;
        movement.OnUnitEndMoving += EndMovingHandler;
    }

    private void Start()
    {
        HP = MaxHP;
        MagicalArmor = MaxMagicArmor;
        PhysicalArmor = MaxPhysicalArmor;
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

    private void Die()
    {
        // TODO: Handle unit death.
        Destroy(gameObject);
    }
}