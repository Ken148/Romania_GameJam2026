using System.Collections;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float openDistance = 1f;
    [SerializeField] private float speed = 0.5f;

    private Vector3 openPos;

    private void Awake()
    {
        openPos = door.localPosition + Vector3.right * openDistance;
    }

    public void Open()
    {
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