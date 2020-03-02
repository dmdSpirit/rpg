using ThisIsThePresident;
using UnityEngine;

public class SelectionController : MonoSingleton<SelectionController>
{
    public enum SelectionType
    {
        Undefined,
        UnitSelect,
    }

    public bool isSomethingSelected { get; protected set; } = false;
    public Unit selectedUnit { get; protected set; } = null;
    public SelectionType selectionType { get; protected set; } = SelectionType.Undefined;

    public void Select(Unit unit)
    {
        if (unit == null || (isSomethingSelected && selectionType == SelectionType.UnitSelect && selectedUnit == unit))
            return;
        if (isSomethingSelected)
            UnSelectPrevious();
        selectedUnit = unit;
        selectedUnit.Select();
        selectionType = SelectionType.UnitSelect;
        isSomethingSelected = true;
    }

    private void UnSelectPrevious()
    {
        if (selectionType == SelectionType.UnitSelect)
        {
            selectedUnit.UnSelect();
            isSomethingSelected = false;
            return;
        }
    }
}