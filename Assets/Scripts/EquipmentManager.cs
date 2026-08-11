using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    [SerializeField] private Transform rightHandSocket;
    [SerializeField] private Transform dropSocket;

    private GameObject equippedToolObject;
    private ITool currentTool;
    public ITool CurrentTool => currentTool;

    void Awake()
    {

        if (!ValidateDependencies())
        {
            enabled = false;
            return;
        }
    }

    private bool ValidateDependencies()
    {
        if (rightHandSocket == null)
        {
            Debug.LogError("RightHandSocket is missing.", this);
            enabled = false;
            return false;
        } 
        return true;
    }

    public void Equip(IPickup pickup)
    {
        if (pickup == null)
        {
            Debug.LogError("EquipmentManager: pickup is NULL.");
            return;
        }

        // -----------------------------------------------------
        // DROP CURRENT TOOL FIRST
        // -----------------------------------------------------

        Drop();

        // -----------------------------------------------------
        // GET PICKUP INFORMATION
        // -----------------------------------------------------

        GameObject worldPickup = pickup.WorldObject;
        GameObject heldPrefab = pickup.HeldPrefab;

        if (heldPrefab == null)
        {
            Debug.LogError("EquipmentManager: HeldPrefab is NULL.");
            return;
        }

        // -----------------------------------------------------
        // PICKUP ACTION
        // -----------------------------------------------------

        pickup.OnPickedUp();

        // -----------------------------------------------------
        // CREATE HELD TOOL
        // -----------------------------------------------------

        Debug.Log($"Held prefab: {heldPrefab}", this);

        equippedToolObject = Instantiate(heldPrefab);

        equippedToolObject.transform.SetParent(
            rightHandSocket,
            false
        );

        ITool tool =
            equippedToolObject.GetComponent<ITool>();

        if (tool == null)
        {
            Destroy(equippedToolObject);

            Debug.LogError(
                "This object is not a tool."
            );

            return;
        }

        currentTool = tool;

        // -----------------------------------------------------
        // DESTROY WORLD PICKUP
        // -----------------------------------------------------

        Destroy(worldPickup);

        // -----------------------------------------------------
        // PLAY CORRECT PICKUP SOUND
        // -----------------------------------------------------

        if (currentTool is Key)
        {
            AudioManager.Instance.PlayKeyPickup();
        }
        else if (currentTool is Screwdriver)
        {
            AudioManager.Instance.PlayScrewdriverPickup();
        }
    }

    public void Unequip()
    {
        Destroy(equippedToolObject);
        equippedToolObject = null;
        currentTool = null;

    }

    public void Drop()
    {
        if (currentTool == null)
            return;

        CharacterController controller =
            GetComponent<CharacterController>();

        GameObject dropped = Instantiate(
            currentTool.PickupPrefab,
            dropSocket.position,
            Quaternion.identity
        );

        Collider droppedCollider =
            dropped.GetComponent<Collider>();

        if (droppedCollider != null && controller != null)
        {
            Physics.IgnoreCollision(
                droppedCollider,
                controller
            );
        }

        // -----------------------------------------------------
        // DROP SOUND
        // -----------------------------------------------------

        if (currentTool is Key)
        {
            AudioManager.Instance.PlayKeyDrop();
        }
        else if (currentTool is Screwdriver)
        {
            AudioManager.Instance.PlayScrewdriverDrop();
        }

        Unequip();
    }


}
