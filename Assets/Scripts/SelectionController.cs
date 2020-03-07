using System;

public class SelectionController : MonoSingleton<SelectionController>
{
    public event Action<Unit> OnUnitSelected;
    public event Action OnUnitUnselect;

    public Unit SelectedUnit { get; private set; } = null;
    public bool IsSomethingSelected => SelectedUnit != null;

    public void Select(Unit unit)
    {
        if (unit == null || (IsSomethingSelected && SelectedUnit == unit))
            return;
        if (IsSomethingSelected)
            UnSelectPrevious();
        SelectedUnit = unit;
        SelectedUnit.Select();
        OnUnitSelected?.Invoke(unit);
    }

    private void UnSelectPrevious()
    {
        if (IsSomethingSelected == false) return;
        SelectedUnit.UnSelect();
        OnUnitUnselect?.Invoke();
    }
}