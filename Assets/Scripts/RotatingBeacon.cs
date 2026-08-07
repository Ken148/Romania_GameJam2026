using UnityEngine;

public class RotatingBeacon : MonoBehaviour
{
    public float rotationSpeed = 180f;

    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}