using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField, Min(0f)] private float interactDistance = 1f;
    
    private PlayerInput playerInput;
    private InputAction interactAction;
    private InputAction dropAction;
    private InputAction attackAction;

    private EquipmentManager equipmentManager;
    private FirstPersonMovement movement;

    [SerializeField] private LayerMask interactMask;


    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        equipmentManager = GetComponentInParent<EquipmentManager>();
        movement = GetComponentInParent<FirstPersonMovement>();

        if (!ValidateDependencies())
        {
            enabled = false;
            return;
        }

        interactAction = playerInput.actions["Interact"];
        dropAction = playerInput.actions["Drop"];
        attackAction = playerInput.actions["Attack"];
    }

    private bool ValidateDependencies()
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found in parent.", this);
            return false;
        }
        if (equipmentManager == null)
        {
            Debug.LogError("EquipmentManager not found in parent.", this);
            return false;
        }
        if (movement == null)
        {
            Debug.LogError("FirstPersonMovement not found in parent.", this);
            return false;
        }

        return true;
    }

    private void Update()
    {
        HandleDrop();

        if (!TryGetHit(out RaycastHit hit))
        {
            Debug.Log("Hit miss");
            return;
        }

        Debug.Log(hit.collider.name);

        HandleToolUse(hit);
        HandlePickup(hit);
        HandlePush(hit);
    }

    private void HandleDrop()
    {
        if (dropAction.WasPressedThisFrame())
            equipmentManager.Drop();
    }

    private void HandlePush(RaycastHit hit)
    {
        if (!interactAction.IsPressed())
            return;
        
        Pushable pushable = hit.collider.GetComponentInParent<Pushable>();
        if (pushable != null)
        {
            Vector3 direction = transform.forward;
            direction.y = 0f;
            direction.Normalize();

            pushable.Push(direction);
        }
    }


    private void HandleToolUse(RaycastHit hit)
    {
        ITool tool = equipmentManager.CurrentTool;
        if (tool == null)
            return;

        if (IsToolActivated(tool))
            tool.Use(hit);
    }

    private bool IsToolActivated(ITool tool)
    {
        switch (tool.Input)
        {
            case ToolInput.Interact:
                return interactAction.IsPressed();

            case ToolInput.Attack:
                return attackAction.WasPressedThisFrame();

            default:
                return false;
        }
    }

    private void HandlePickup(RaycastHit hit)
    {
        if (!interactAction.WasPressedThisFrame())
            return;

        IPickup pickup = hit.collider.GetComponentInParent<IPickup>();
        if (pickup != null)
            equipmentManager.Equip(pickup);
    }

    private bool TryGetHit(out RaycastHit hit)
    {
        return Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            interactDistance,
            interactMask
        );
    }

    
    
}