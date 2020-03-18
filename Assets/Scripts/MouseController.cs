using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MouseController : MonoSingleton<MouseController>
{
    private PlayerInputControl playerInput;
    private new Camera camera;
    private bool inputIsBlocked;
    private bool raycastIsBlocked;

    private Unit hoveredUnit;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Fight.Select.performed += MouseSelectHandler;
        playerInput.Fight.RightClick.performed += MouseRightClickHandler;
        camera = Camera.main;
        AnimationSystem.Instance.OnBlockingAnimationStateChange += BlockingAnimationStateChangeHandler;
        UIController.Instance.OnMouseEnterUI += MouseEnterUIHandler;
        UIController.Instance.OnMouseExitUI += MouseExitUIHandler;
    }

    // IMPROVE: use new input system callback on mouse move OR on camera move.
    private void Update()
    {
        if (TryGetPointedObject(out RaycastHit hit))
        {
            var unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                hoveredUnit = unit;
                UIController.Instance.ShowHoveredUnitBarsPanel(unit);
                return;
            }
        }

        UIController.Instance.HideHoveredUnitBarsPanel();
        hoveredUnit = null;
    }


    // IMPROVE: should we cache this value?
    // IMPROVE: check layer mask.
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
        if (inputIsBlocked || raycastIsBlocked) return;
        if (TryGetPointedObject(out var hit) == false) return;
        var unit = hit.transform.GetComponent<Unit>();
        if (unit != null)
            SelectionController.Instance.Select(unit);
        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") &&
                 SelectionController.Instance.IsSomethingSelected)
            SelectionController.Instance.SelectedUnit.MoveTo(hit.point);
    }

    private void MouseRightClickHandler(InputAction.CallbackContext context)
    {
        if (inputIsBlocked || raycastIsBlocked) return;
        if (TryGetPointedObject(out var hit) == false) return;
        var unit = hit.transform.GetComponent<Unit>();
        if (unit != null && unit.UnitMaster == UnitMaster.NPC)
            SelectionController.Instance.ShowNPCInfo(unit);
    }

    private void MouseEnterUIHandler() => raycastIsBlocked = true;
    private void MouseExitUIHandler() => raycastIsBlocked = false;

    private void OnEnable() => playerInput?.Enable();


    private void OnDisable() => playerInput?.Disable();
}