using System;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public event Action<bool> OnSelectionChanged;

    [SerializeField] private GameObject selectionCircle = default;

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            selectionCircle.SetActive(value);
            OnSelectionChanged?.Invoke(value);
        }
    }

    private bool isSelected = false;

    private void Awake()
    {
        if (selectionCircle != null)
            selectionCircle.SetActive(false);
    }
}