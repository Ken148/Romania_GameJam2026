using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField, Min(0f)] private float interactDistance = 1f;
    
    private PlayerInput playerInput;
    private InputAction interactAction;
    private InputAction dropAction;

    private EquipmentManager equipmentManager;
    private FirstPersonMovement movement;

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
        

        if (dropAction.WasPressedThisFrame())
            equipmentManager.Drop();

        if (TryGetHit(out RaycastHit hit))
        {
            if (interactAction.IsPressed())
            {
                if (equipmentManager.CurrentTool != null)
                    equipmentManager.CurrentTool.Use(hit);
                else if (interactAction.WasPressedThisFrame())
                    Interact(hit);
            }
        }
        else Debug.Log("Hit miss");
    }



    private bool TryGetHit(out RaycastHit hit)
    {
        return Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            interactDistance
        );
    }

    private void Interact(RaycastHit hit)
    {
        IPickup pickup = hit.collider.GetComponentInParent<IPickup>();
        if (pickup != null)
            equipmentManager.Equip(pickup);
    }
    
    private void Push(RaycastHit hit)
    {
        Pushable pushable = hit.collider.GetComponentInParent<Pushable>();
        if (pushable != null)
        {
            Vector3 direction = transform.forward;
            direction.y = 0f;
            direction.Normalize();

            pushable.Push(direction);
        }
    }

    private void Unscrew(RaycastHit hit)
    {
        Screw screw = hit.collider.GetComponentInParent<Screw>();
        if (screw != null)
        {
            screw.Unscrew();
        }
    }
}