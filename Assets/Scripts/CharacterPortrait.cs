using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : UIPanel
{
    public event Action<CharacterPortrait> OnPortraitCleared;

    [SerializeField] private Unit unit = default;
    [SerializeField] private Image image = default;

    private Button button;
    private Image portraitBorder;
    private Color selectedColor = Color.green;
    private Color unselectedColor;

    private void Awake()
    {
        portraitBorder = GetComponent<Image>();
        unselectedColor = portraitBorder.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(SelectUnit);
    }

    public void ShowPanel(Unit unit)
    {
        gameObject.SetActive(true);
        this.unit = unit;
        image.sprite = unit.UnitPortraitImage;
        var selection = unit.GetComponent<UnitSelection>();
        if (selection != null)
            selection.OnSelectionChanged += OnSelectedChangedHandler;
        OnSelectedChangedHandler(false);
        base.ShowPanel();
    }

    public void ClearPortrait()
    {
        var selection = unit.GetComponent<UnitSelection>();
        if (selection != null)
            selection.OnSelectionChanged -= OnSelectedChangedHandler;
        unit = null;
        OnPortraitCleared?.Invoke(this);
    }

    private void OnSelectedChangedHandler(bool isSelected)
    {
        portraitBorder.color = isSelected ? selectedColor : unselectedColor;
    }

    private void SelectUnit()
    {
        if (unit == null) return;
        SelectionController.Instance.Select(unit);
    }
}