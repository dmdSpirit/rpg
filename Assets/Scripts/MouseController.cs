using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoSingleton<MouseController>
{
    private PlayerInputControl playerInput;
    private new Camera camera;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Fight.Select.performed += MouseSelectHandler;
        camera = Camera.main;
    }

    // IMPROVE: should we cache this value?
    public bool TryGetPointedObject(out RaycastHit hit)
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out hit);
    }

    private void MouseSelectHandler(InputAction.CallbackContext context)
    {
        if (TryGetPointedObject(out var hit) == false) return;
        var unit = hit.transform.GetComponent<Unit>();
        if (unit == null) return;
        SelectionController.Instance.Select(unit);
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}