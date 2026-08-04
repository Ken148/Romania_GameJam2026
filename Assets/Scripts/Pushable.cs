using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float pushSpeed = 1.5f;

    private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    public void Push(Vector3 direction)
    {
        Debug.Log("Moving pushable");
        rb.linearVelocity = direction * pushSpeed;
    }
}
