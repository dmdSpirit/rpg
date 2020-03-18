using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UnitInitiativePortrait : UIPanel, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public event Action<UnitInitiativePortrait> OnMouseEnter;
    public event Action<UnitInitiativePortrait> OnMouseExit;

    [SerializeField] private UnitStatsPanel unitStatsPanel = default;
    [SerializeField] private Color highlightedColor = Color.white;

    public Unit Unit { get; private set; }

    private Image backgroundImage;

    private Color baseColor;
    // private bool isHighlighted;

    private void Awake()
    {
        backgroundImage = GetComponent<Image>();
        baseColor = backgroundImage.color;
    }

    public void ShowPanel(Unit unit)
    {
        Unit = unit;
        base.ShowPanel();
        unitStatsPanel.ShowPanel(unit);
        unitStatsPanel.ShowName = false;
    }

    public override void HidePanel()
    {
        unitStatsPanel.HidePanel();
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

    public void HighlightPortrait()
    {
        // isHighlighted = true;
        backgroundImage.color = highlightedColor;
        unitStatsPanel.ShowName = true;
    }

    public void UnHighlightPortrait()
    {
        // isHighlighted = false;
        backgroundImage.color = baseColor;
        unitStatsPanel.ShowName = false;
    }
}