using UnityEngine;

public class Fuse : MonoBehaviour, IPickup, ITool
{
    [SerializeField] private GameObject heldPrefab;
    [SerializeField] private GameObject pickupPrefab;

    private EquipmentManager equipmentManager;
    private FuseSlot currentSlot;

    public GameObject HeldPrefab => heldPrefab;
    public GameObject WorldObject => gameObject;
    public GameObject PickupPrefab => pickupPrefab;
    public ToolInput Input => ToolInput.Attack;

    private void Awake()
    {
        equipmentManager = FindAnyObjectByType<EquipmentManager>();
    }

    public void Use(RaycastHit hit)
    {
        FuseSlot slot = hit.collider.GetComponent<FuseSlot>();
        if (slot == null)
            return;

        slot.InsertFuse();
        equipmentManager.Unequip();
    }

    public void SetCurrentSlot(FuseSlot slot)
    {
        currentSlot = slot;
    }

    public void OnPickedUp()
    {
        currentSlot?.OnFusePickedUp();
        currentSlot = null;
    }
}