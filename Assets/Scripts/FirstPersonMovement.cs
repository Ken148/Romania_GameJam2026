using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField, Min(0f)] private float walkSpeed = 3f;

    [Header("Crouch")]
    [SerializeField, Range(0.2f, 1f)] private float crouchMultiplier = 0.3f;
    [SerializeField, Min(0f)] private float crouchTransitionSpeed = 3f;

    private CharacterController controller;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction crouchAction;

    private float verticalSpeed;
    private float standingHeight;

    [SerializeField] private Camera playerCamera;
    private Vector3 standingCenter;
    private Vector3 standingCameraPosition;

    private Transform cameraTransform;

    private void Awake()
    {
        if (!ValidateDependencies())
        {
            enabled = false;
            return;
        }

        controller = GetComponent<CharacterController>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        crouchAction = playerInput.actions["Crouch"];

        standingHeight = controller.height;
        standingCenter = controller.center;
        cameraTransform = playerCamera.transform;
        standingCameraPosition = cameraTransform.localPosition;
    }

    private bool ValidateDependencies()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned.", this);
            return false;
        }
        return true;
    }

    private void Update()
    {

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        Move(moveInput);

        UpdateCrouch(
            crouchAction.IsPressed()
        );
    }

    private void Move(Vector2 input)
    {
        Vector3 forward =
            Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        Vector3 right =
            Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        Vector3 direction =
            (forward * input.y + right * input.x).normalized;

        Vector3 velocity = direction * walkSpeed;
        velocity.y = verticalSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateCrouch(bool crouching)
    {
        float scale = crouching ? crouchMultiplier : 1f;

        controller.height = standingHeight * scale;

        Vector3 center = standingCenter;
        center.y *= scale;
        controller.center = center;

        Vector3 cameraPosition = standingCameraPosition;
        cameraPosition.y *= scale;

        cameraTransform.localPosition = Vector3.MoveTowards(
            cameraTransform.localPosition,
            cameraPosition,
            crouchTransitionSpeed * Time.deltaTime
        );
    }
}