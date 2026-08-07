using System.Collections;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float openDistance = 1f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private bool openLeft;

    private Vector3 openPos;

    private void Awake()
    {
        Vector3 direction = openLeft ? Vector3.left : Vector3.right;
        openPos = door.localPosition + direction * openDistance;
        Debug.Log($"Door target position: {openPos}");


    }

    public void Open()
    {
        Debug.Log("TriggerDoor.Open() called.");
        StopAllCoroutines();
        StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        while (Vector3.Distance(door.localPosition, openPos) > 0.001f)
        {
            door.localPosition = Vector3.MoveTowards(
                door.localPosition,
                openPos,
                speed * Time.deltaTime);

            yield return null;
        }
    }
}