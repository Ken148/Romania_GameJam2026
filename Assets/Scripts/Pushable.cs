using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float pushSpeed = 1.5f;

    private bool isBeingPushed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Push(Vector3 direction)
    {
        rb.linearVelocity = direction * pushSpeed;

        if (!isBeingPushed)
        {
            isBeingPushed = true;

            // Start ONLY the object/box water sound.
            AudioManager.Instance.StartObjectWaterPush();
        }
    }

    public void StopPush()
    {
        rb.linearVelocity = Vector3.zero;

        if (isBeingPushed)
        {
            isBeingPushed = false;

            // Stop ONLY the object/box water sound.
            AudioManager.Instance.StopObjectWaterPush();
        }
    }
}
