using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UnitInitiativePortrait : UIPanel, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public event Action<UnitInitiativePortrait> OnMouseEnter;
    public event Action<UnitInitiativePortrait> OnMouseExit;

    [SerializeField] private bool isFirstPortrait = false;

    [SerializeField] private UnitStatsPanel unitStatsPanel = default;
    [SerializeField] private Color firstBorderColor = Color.white;
    [SerializeField] private Color partyBorderColor = default;
    [SerializeField] private Color enemyBorderColor = default;

    [SerializeField] private Image hoverBorder = default;
    [SerializeField] private Color firstHoverColor = default;
    [SerializeField] private Color partyHoverColor = default;
    [SerializeField] private Color enemyHoverColor = default;

    public Unit Unit { get; private set; }

    private Image backgroundImage;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void ShowPanel(Unit unit)
    {
        Unit = unit;
        base.ShowPanel();
        backgroundImage.color = isFirstPortrait ? firstBorderColor : (unit.UnitMaster == UnitMaster.Player ? partyBorderColor : enemyBorderColor);
        unitStatsPanel.ShowPanel(unit);
        unitStatsPanel.ShowName = isFirstPortrait;
        HideHoverBorder();
    }

    public override void HidePanel()
    {
        unitStatsPanel.HidePanel();
        HideHoverBorder();
        base.HidePanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightPortrait();
        OnMouseEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnHighlightPortrait();
        OnMouseExit?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectionController.Instance.Select(Unit);
    }

    public void HighlightPortrait() => ShowHoverBorder();

    public void UnHighlightPortrait() => HideHoverBorder();

    private void ShowHoverBorder()
    {
        if (Unit == null)
        {
            hoverBorder.gameObject.SetActive(false);
            return;
        }

        hoverBorder.color = CombatController.Instance.CheckUnitIsCurrentInitiative(Unit) ? firstHoverColor : (Unit.UnitMaster == UnitMaster.Player ? partyHoverColor : enemyHoverColor);
    }

    private void HideHoverBorder() => hoverBorder.gameObject.SetActive(false);
}