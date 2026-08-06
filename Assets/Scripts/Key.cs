using UnityEngine;

public class Key : MonoBehaviour, ITool, IPickup
{
    [SerializeField] private GameObject heldPrefab;
    [SerializeField] private GameObject pickupPrefab;

    public GameObject HeldPrefab => heldPrefab;
    public GameObject WorldObject => gameObject;
    public GameObject PickupPrefab => pickupPrefab;

    public ToolInput Input => ToolInput.Interact;

    public void Use(RaycastHit hit)
    {
        Padlock padlock = hit.collider.GetComponentInParent<Padlock>();

        if (padlock != null)
            padlock.Unlock();
    }

    public void OnPickedUp() {}

}
