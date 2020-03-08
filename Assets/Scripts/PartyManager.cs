using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoSingleton<PartyManager>
{
    [SerializeField] private CharacterPortrait[] portraits = default;

    private List<Unit> units = new List<Unit>();
    private List<CharacterPortrait> freePortraits;

    private void Awake()
    {
        freePortraits = new List<CharacterPortrait>(portraits);
        foreach (var portrait in portraits)
        {
            portrait.HidePanel();
            portrait.OnPortraitCleared += PortraitClearedHandler;
        }
    }

    public void AddUnitToParty(Unit unit)
    {
        if (freePortraits.Count < 1)
        {
            Debug.LogError($"Trying to add {unit.name} to party, but there are no enough character portraits left.");
            return;
        }

        CreateUnitPortrait(unit);
        units.Add(unit);
    }

    private void CreateUnitPortrait(Unit unit)
    {
        var portrait = freePortraits.First();
        portrait.ShowPanel(unit);
        freePortraits.Remove(portrait);
    }

    private void PortraitClearedHandler(CharacterPortrait portrait) => freePortraits.Add(portrait);
}