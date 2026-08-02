using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    [SerializeField] private Transform rightHandSocket;
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

        GameObject worldPickup = pickup.WorldObject;
        GameObject heldPrefab = pickup.HeldPrefab;
        
        Debug.Log($"Held prefab: {heldPrefab}", this);
        Unequip();
        equippedToolObject = Instantiate(heldPrefab);
        equippedToolObject.transform.SetParent(rightHandSocket, false);
        ITool tool = equippedToolObject.GetComponent<ITool>();
        if (tool == null)
        {
            Destroy(equippedToolObject);
            Debug.LogError("This object is not a tool.");
            return;
        }
        currentTool = tool;

        Destroy(worldPickup);
        
    }

    public void Unequip()
    {
        Destroy(equippedToolObject);
        equippedToolObject = null;
        currentTool = null;
        
        // use free physics to do world pickup
    }
}
