using UnityEngine;

public class FuseSlot : MonoBehaviour
{
    [SerializeField] private ElectricityLight light;
    [SerializeField] private GameObject fusePrefab;

    [SerializeField] private GameObject heldFuse;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        boxCollider.enabled = heldFuse == null;

        if (heldFuse != null)
        {
            Fuse fuse = heldFuse.GetComponent<Fuse>();
            if (fuse != null)
                fuse.SetCurrentSlot(this);
        }
    }

    public void InsertFuse()
    {
        if (heldFuse != null)
            return;

        heldFuse = Instantiate(fusePrefab, transform, false);
        heldFuse.transform.localPosition = Vector3.zero;
        heldFuse.transform.localRotation = Quaternion.identity;

        Rigidbody rb = heldFuse.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Fuse fuse = heldFuse.GetComponent<Fuse>();
        if (fuse != null)
            fuse.SetCurrentSlot(this);

        boxCollider.enabled = false;
        light.TurnGreen();
    }

    public void OnFusePickedUp()
    {
        heldFuse = null;
        boxCollider.enabled = true;
        light.TurnRed();
    }

    public void RemoveFuse()
    {
        if (heldFuse != null)
            Destroy(heldFuse);

        heldFuse = null;
        boxCollider.enabled = true;
        light.TurnRed();
    }
}