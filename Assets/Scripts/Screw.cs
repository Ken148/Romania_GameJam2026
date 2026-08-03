using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Screw : MonoBehaviour
{
    [SerializeField] private float screwLength = 0.1f;
    [SerializeField] private float step = 0.0001f;
    [SerializeField] private float rotationStep = 2f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isScrewedIn = true;

    public bool IsScrewedIn => isScrewedIn;
    private Vector3 startLocalPosition;


     private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        startLocalPosition = transform.localPosition;

    }

    public void Unscrew()
    {
        if (!isScrewedIn)
        return;

        float distance = Vector3.Distance(startLocalPosition, transform.localPosition);

        if (distance < screwLength)
        {
            transform.localPosition -= Vector3.forward * step;
            transform.Rotate(Vector3.back, rotationStep, Space.Self);
            Debug.Log("Unscrewing screw");
        }
        else
        {
            isScrewedIn = false;

            transform.SetParent(null);

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log($"This collider: {contact.thisCollider.name}");
            Debug.Log($"Other collider: {contact.otherCollider.name}");
        }
    }
}