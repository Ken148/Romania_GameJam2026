using UnityEngine;

public class Hammer : MonoBehaviour, ITool, IPickup
{
    [SerializeField] private GameObject heldPrefab;
    [SerializeField] private GameObject pickupPrefab;

    public GameObject HeldPrefab => heldPrefab;
    public GameObject WorldObject => gameObject;
    public GameObject PickupPrefab => pickupPrefab;
    public ToolInput Input => ToolInput.Attack;

    public void Use(RaycastHit hit)
    {
        Debris debris = hit.collider.GetComponentInParent<Debris>();

        if (debris == null)
            return;

        Debug.Log("Hitting " + debris.gameObject.name);

        debris.Hit(transform.forward);
    }

    public void OnPickedUp() {}
}