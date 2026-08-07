using UnityEngine;
using UnityEngine.Events;

public class FuseGroup : MonoBehaviour
{
    [SerializeField] private int requiredFuses = 2;
    [SerializeField] private UnityEvent onAllFusesInserted;

    private int inserted;

    public void FuseInserted()
    {
        inserted++;
        Debug.Log($"Fuse inserted: {inserted}/{requiredFuses}");



        if (inserted >= requiredFuses)
        {
            onAllFusesInserted.Invoke();
            Debug.Log("All fuses inserted. Invoking event.");
        }
    }

    public void FuseRemoved()
    {
        inserted = Mathf.Max(0, inserted - 1);
        Debug.Log($"Fuse removed: {inserted}/{requiredFuses}");

    }
}