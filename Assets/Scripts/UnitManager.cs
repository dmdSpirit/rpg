using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class UnitManager : MonoSingleton<UnitManager>
{
    public List<Unit> AllUnits { get; private set; } = new List<Unit>();
    
    private List<Unit> playerUnits = new List<Unit>();
    private List<Unit> npcUnits = new List<Unit>();

    public void SubscribeUnit(Unit unit)
    {
        AllUnits.Add(unit);
        switch (unit.UnitMaster)
        {
            case UnitMaster.Player:
                playerUnits.Add(unit);
                PartyManager.Instance.AddUnitToParty(unit);
                break;
            case UnitMaster.NPC:
                npcUnits.Add(unit);
                break;
        }

        unit.OnUnitDestroyed += UnitDestroyedHandler;
    }

    private void UnitDestroyedHandler(Unit unit)
    {
        AllUnits.Remove(unit);
        switch (unit.UnitMaster)
        {
            case UnitMaster.Player:
                playerUnits.Remove(unit);
                break;
            case UnitMaster.NPC:
                npcUnits.Remove(unit);
                break;
        }
    }
}