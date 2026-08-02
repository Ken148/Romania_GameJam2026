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

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        equipmentManager = GetComponentInParent<EquipmentManager>();

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

        return true;
    }

    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
            Interact();

        if (dropAction.WasPressedThisFrame())
            equipmentManager.Drop();
        
    }

    private void Interact()
    {
        Ray ray = new Ray(
            transform.position,
            transform.forward
        );

        Debug.DrawRay(
            ray.origin,
            ray.direction * interactDistance,
            Color.red,
            1f
        );

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactDistance))
        {
            return;
        }

        IPickup pickup = hit.collider.GetComponentInParent<IPickup>();
        if (pickup != null)
            equipmentManager.Equip(pickup);
    }
}