using UnityEngine;
using UnityEngine.InputSystem;

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

    private bool isPushing;
    public bool IsPushing => isPushing;



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
            //Debug.Log("Hit miss");
            return;
        }

        Debug.Log(hit.collider.name);

        if (HandleToolUse(hit)) return;
        HandlePickup(hit);
        HandlePush(hit);
        HandlePadlockedDoor(hit);
        HandleSceneDoor(hit);
        HandleBrokenFuse(hit);
        HandlePuzzlePlate(hit);
        HandleBoat(hit);
    }

    private void HandleDrop()
    {
        if (dropAction.WasPressedThisFrame())
            equipmentManager.Drop();
    }

    private void HandlePush(RaycastHit hit)
    {
        isPushing = false;

        if (!interactAction.IsPressed())
            return;
        
        Pushable pushable = hit.collider.GetComponentInParent<Pushable>();
        if (pushable != null)
        {
            isPushing = true;

            Vector3 direction = transform.forward;

            pushable.Push(GetCardinalDirection(direction));
        }

        
    }

    private void HandlePadlockedDoor(RaycastHit hit)
    {
        if (!interactAction.IsPressed())
            return;
        
        PadlockedDoor door = hit.collider.GetComponentInParent<PadlockedDoor>();
        if (door != null)
        {
            door.Open();
        }
    }

    private void HandleSceneDoor(RaycastHit hit)
    {
        if (!interactAction.WasPressedThisFrame())
            return;

        SceneDoor door = hit.collider.GetComponentInParent<SceneDoor>();
        if (door != null){
            door.Enter();
            Debug.Log("Switching scenes");
        }
    }

    private void HandleBrokenFuse(RaycastHit hit)
    {
        if (!interactAction.WasPressedThisFrame())
            return;

        BrokenFuse brokenFuse = hit.collider.GetComponentInParent<BrokenFuse>();
        if (brokenFuse != null){
            brokenFuse.removeFuse();
            Debug.Log("Removing broken fuse");
        }
    }

    private Vector3 GetCardinalDirection(Vector3 direction)
    {
        direction.y = 0;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            return new Vector3(Mathf.Sign(direction.x), 0, 0);

        return new Vector3(0, 0, Mathf.Sign(direction.z));
    }


    private bool HandleToolUse(RaycastHit hit)
    {
        ITool tool = equipmentManager.CurrentTool;
        if (tool == null)
            return false;

        if (IsToolActivated(tool))
        {
            tool.Use(hit);
            return true;
        }
        return false;
            
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

    private void HandlePuzzlePlate(RaycastHit hit)
    {
        if (!interactAction.WasPressedThisFrame())
            return;

        PuzzlePlate plate = hit.collider.GetComponentInParent<PuzzlePlate>();
        if (plate != null)
            plate.Interact();
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

    private void HandleBoat(RaycastHit hit)
    {
        if (!interactAction.WasPressedThisFrame())
            return;

        Boat boat = hit.collider.GetComponentInParent<Boat>();
        if (boat != null)
            boat.Interact();
    }

    
    
}