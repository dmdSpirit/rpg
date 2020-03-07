using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MouseController : MonoSingleton<MouseController>
{
    private PlayerInputControl playerInput;
    private new Camera camera;
    private bool inputIsBlocked;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Fight.Select.performed += MouseSelectHandler;
        camera = Camera.main;
        AnimationSystem.Instance.OnBlockingAnimationStateChange += BlockingAnimationStateChangeHandler;
    }


    // IMPROVE: should we cache this value?
    public bool TryGetPointedObject(out RaycastHit hit)
    {
        var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out hit);
    }

    private void BlockingAnimationStateChangeHandler(bool inputIsBlocked)
    {
        this.inputIsBlocked = inputIsBlocked;
    }

    private void MouseSelectHandler(InputAction.CallbackContext context)
    {
        if (inputIsBlocked) return;
        if (TryGetPointedObject(out var hit) == false) return;
        var unit = hit.transform.GetComponent<Unit>();
        if (unit != null)
            SelectionController.Instance.Select(unit);
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") && SelectionController.Instance.IsSomethingSelected)
            SelectionController.Instance.SelectedUnit.MoveTo(hit.point);
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}