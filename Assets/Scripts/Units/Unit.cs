using System;
using UnityEngine;

[RequireComponent(typeof(UnitSelection))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Transform floorTransform;

    public Transform FloorTransform => floorTransform;
    
    private UnitSelection selection;

    private void Awake()
    {
        selection = GetComponent<UnitSelection>();
    }

    public void Select()
    {
        selection.IsSelected = true;
    }

    public void UnSelect()
    {
        selection.IsSelected = false;
    }
}