using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInitiativePortrait : UIPanel, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public event Action<UnitInitiativePortrait> OnMouseEnter;
    public event Action<UnitInitiativePortrait> OnMouseExit;

    [SerializeField] private BarsPanel barsPanel = default;
    [SerializeField] private Image image = default;
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
        barsPanel.ShowPanel(unit);
        base.ShowPanel();
        image.sprite = unit.UnitPortraitImage;
    }

    public override void HidePanel()
    {
        barsPanel.HidePanel();
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
    }

    public void UnHighlightPortrait()
    {
        // isHighlighted = false;
        backgroundImage.color = baseColor;
    }
}