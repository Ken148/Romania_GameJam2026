using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField, Min(0f)] private float walkSpeed = 5f;

    [Header("Crouch")]
    [SerializeField, Range(0.2f, 1f)] private float crouchMultiplier = 0.5f;
    [SerializeField, Min(0f)] private float crouchTransitionSpeed = 3f;
    private bool isCrouching;
    public bool IsCrouching => isCrouching;

    [SerializeField] private float gravity = 10f;

    private CharacterController controller;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction crouchAction;

    private float verticalSpeed;
    [SerializeField] private float standingHeight = 2.5f;

    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;

    private Vector3 horizontalVelocity;


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

        controller.height = standingHeight;
    }

    private bool ValidateDependencies()
    {
        return true;
    }

    private void Update()
    {

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        ApplyGravity();

        Move(moveInput);

        isCrouching = crouchAction.IsPressed();

        UpdateCrouch(isCrouching);
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }

        verticalSpeed -= gravity * Time.deltaTime;
    }

    private void Move(Vector2 input)
    {
        Vector3 forward =
            Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        Vector3 right =
            Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        Vector3 direction =
            (forward * input.y + right * input.x).normalized;

        Vector3 targetVelocity = direction * walkSpeed;

        float rate = direction.sqrMagnitude > 0f
            ? acceleration
            : deceleration;

        horizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity,
            targetVelocity,
            rate * Time.deltaTime
        );

        Vector3 velocity = horizontalVelocity;
        velocity.y = verticalSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateCrouch(bool crouching)
    {
        float scale = crouching ? crouchMultiplier : 1f;

        float targetHeight = standingHeight * scale;

        controller.height = Mathf.MoveTowards(
            controller.height,
            targetHeight,
            crouchTransitionSpeed * Time.deltaTime
        );

        controller.center = new Vector3(
        controller.center.x,
        controller.height * 0.5f,
        controller.center.z
        );
    }

    private void OnValidate()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            return;

        controller.height = standingHeight;
        controller.center = new Vector3(
            controller.center.x,
            standingHeight * 0.5f,
            controller.center.z
        );
    }
}