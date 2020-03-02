using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    private PlayerInputControl playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Fight.Select.performed += _ => MouseRayCast();
    }

    private void MouseRayCast()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            var unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                SelectionController.Instance.Select(unit);
                return;
            }
        }
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}