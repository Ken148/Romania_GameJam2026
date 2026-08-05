using UnityEngine;

public class PadlockedDoor : MonoBehaviour
{
    [SerializeField] private Padlock padlock;
    [SerializeField] private float doorLength = 1.5f;
    [SerializeField] private float step = 0.005f;

    private Vector3 startLocalPosition;

    void Awake()
    {
        padlock = GetComponentInChildren<Padlock>();   
        startLocalPosition = transform.localPosition;
    }

    public void Open()
    {
        if (padlock.IsLocked)
            return; 

        Debug.Log("Opening door");

        float distance = Vector3.Distance(startLocalPosition, transform.localPosition);

        if (distance < doorLength)
        {
            transform.localPosition += Vector3.forward * step;
            Debug.Log("Opening door");
        }
    }


}
