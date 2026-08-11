using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float pushSpeed = 1.5f;

    // How long the object can go without receiving
    // another Push() call before the push is considered finished.
    [SerializeField] private float pushTimeout = 0.1f;

    private bool isBeingPushed = false;
    private float lastPushTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // If Push() has not been called recently,
        // consider the pushing action finished.
        if (isBeingPushed &&
            Time.time - lastPushTime > pushTimeout)
        {
            StopPush();
        }
    }

    public void Push(Vector3 direction)
    {
        rb.linearVelocity = direction * pushSpeed;

        // Remember when Push() was last called.
        lastPushTime = Time.time;

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

    private void OnDisable()
    {
        // Safety: never leave the water sound playing
        // if this object is disabled or destroyed.
        if (isBeingPushed)
        {
            isBeingPushed = false;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopObjectWaterPush();
            }
        }
    }
}