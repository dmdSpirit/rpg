using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class InitiativePanel : UIPanel
{
    [SerializeField] private UnitInitiativePortrait[] portraits = default;
    [SerializeField] private GameObject turnSplitter=default;

    private List<UnitInitiativePortrait> shownPortraits = new List<UnitInitiativePortrait>();
    private InitiativeOrder initiativeOrder;
    private Dictionary<Unit, List<UnitInitiativePortrait>> unitPortraits;

    private void Start()
    {
        foreach (var portrait in portraits)
        {
            portrait.OnMouseEnter += PortraitHighlightedHandler;
            portrait.OnMouseExit += PortraitUnHighlightedHandler;
        }

        HidePanels();
    }

    public void ShowPanel(InitiativeOrder initiativeOrder)
    {
        base.ShowPanel();
        unitPortraits = new Dictionary<Unit, List<UnitInitiativePortrait>>();
        var canShowNextTurn = initiativeOrder.CurrentTurnInitiative.Count < portraits.Length;
        var turnPortraitsShown = Mathf.Min(initiativeOrder.CurrentTurnInitiative.Count, portraits.Length);
        for (var i = 0; i < turnPortraitsShown; ++i)
            ShowPortrait(i, initiativeOrder.CurrentTurnInitiative[i]);
        turnSplitter.SetActive(canShowNextTurn);
        turnSplitter.transform.SetSiblingIndex(portraits[initiativeOrder.CurrentTurnInitiative.Count - 1].transform
            .GetSiblingIndex() + 1);
        if (canShowNextTurn == false) return;
        turnPortraitsShown = Mathf.Min(initiativeOrder.NextTurnInitiative.Count,
            portraits.Length - initiativeOrder.CurrentTurnInitiative.Count);
        for (var i = 0; i < turnPortraitsShown; ++i)
            ShowPortrait(i + initiativeOrder.CurrentTurnInitiative.Count, initiativeOrder.NextTurnInitiative[i]);
    }

    public override void HidePanel()
    {
        HidePanels();
        base.HidePanel();
    }

    private void ShowPortrait(int index, Unit unit)
    {
        if (unitPortraits.ContainsKey(unit))
            unitPortraits[unit].Add(portraits[index]);
        else
            unitPortraits.Add(unit, new List<UnitInitiativePortrait> {portraits[index]});
        portraits[index].ShowPanel(unit);
    }

    private void HidePanels()
    {
        foreach (var portrait in portraits)
            portrait.HidePanel();
        shownPortraits.Clear();
        turnSplitter.transform.SetAsLastSibling();
        turnSplitter.SetActive(false);
    }

    private void PortraitHighlightedHandler(UnitInitiativePortrait portrait)
    {
        var portraitsToHighlight = unitPortraits[portrait.Unit];
        foreach (var p in portraitsToHighlight)
        {
            if (portrait == p) continue;
            p.HighlightPortrait();
        }
    }

    private void PortraitUnHighlightedHandler(UnitInitiativePortrait portrait)
    {
        var portraitsToUnHighlight = unitPortraits[portrait.Unit];
        foreach (var p in portraitsToUnHighlight)
        {
            if (portrait == p) continue;
            p.UnHighlightPortrait();
        }
    }
}