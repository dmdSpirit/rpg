using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private Unit unitInInitiative;
    private List<Unit> unitsInCombat;
    private Dictionary<Unit, int> unitsInitiative;
    private InitiativeOrder initiativeOrder;

    [ContextMenu("Start Combat")]
    public void StartCombat()
    {
        unitsInCombat = UnitManager.Instance.AllUnits;
        initiativeOrder = new InitiativeOrder(unitsInCombat);
        UIController.Instance.ShowInitiativePanel(initiativeOrder);
        SelectionController.Instance.OnUnitSelected += UnitSelectedHandler;
        SelectionController.Instance.OnUnitUnselect += UnitUnSelectedHandler;
        SelectionController.Instance.Select(initiativeOrder.ActiveUnit);
    }

    private void UnitSelectedHandler(Unit unit)
    {
        UIController.Instance.ShowSelectedUnitBarsPanel(unit);
    }

    private void UnitUnSelectedHandler(Unit unit)
    {
        UIController.Instance.HideSelectedUnitBarsPanel(unit);
    }
}