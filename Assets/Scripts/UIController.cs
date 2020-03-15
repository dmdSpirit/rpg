using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoSingleton<UIController>, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnMouseEnterUI;
    public event Action OnMouseExitUI;

    [SerializeField] private NPCInfoPanel npcInfoPanel = default;
    [SerializeField] private InitiativePanel initiativePanel = default;
    [SerializeField] private SelectedUnitBarsPanel selectedUnitBarsPanel = default;

    private List<UIPanel> shownPanels = new List<UIPanel>();

    private void Start()
    {
        RegisterPanel(npcInfoPanel);
        RegisterPanel(initiativePanel);
        RegisterPanel(selectedUnitBarsPanel);
    }

    public void ShowInitiativePanel(InitiativeOrder initiativeOrder)
    {
        initiativePanel.ShowPanel(initiativeOrder);
        shownPanels.Add(initiativePanel);
    }

    public void ShowNPCInfoPanel(Unit unit)
    {
        npcInfoPanel.ShowPanel(unit);
        shownPanels.Add(npcInfoPanel);
    }

    public void ShowSelectedUnitBarsPanel(Unit unit)
    {
        selectedUnitBarsPanel.ShowPanel(unit);
        shownPanels.Add(selectedUnitBarsPanel);
    }

    public void HideSelectedUnitBarsPanel(Unit unit)
    {
        selectedUnitBarsPanel.HidePanel();
    }

    public void HideAllPanels()
    {
        foreach (var panel in shownPanels)
            panel.HidePanel();
        shownPanels.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterUI?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitUI?.Invoke();
    }

    private void RegisterPanel(UIPanel panel)
    {
        panel.HidePanel();
        panel.OnPanelHidden += OnPanelHiddenHandler;
    }

    private void OnPanelHiddenHandler(UIPanel panel)
    {
        if (shownPanels.Contains(panel))
            shownPanels.Remove(panel);
    }
}