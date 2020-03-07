using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInput;
    [SerializeField] private float cameraSpeed = 1f;

    private Vector2 movementDirection;

    private void Awake()
    {
        playerInput = new PlayerInputControl();
        playerInput.Fight.MoveCamera.performed += move => movementDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (movementDirection != Vector2.zero)
        {
            var movement = new Vector3(movementDirection.x * cameraSpeed * Time.fixedTime, 0, movementDirection.y * cameraSpeed * Time.fixedTime);
            transform.Translate(movement);
        }
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();
}