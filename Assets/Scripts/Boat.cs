using System.Collections;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private float moveDistance = -7f;
    [SerializeField] private float moveSpeed = 1f;

    private bool released;
    private Vector3 targetPosition;

    private void Awake()
    {
        targetPosition = transform.position + Vector3.up * moveDistance;
    }

    public void Release()
    {
        if (released)
            return;

        released = true;

        StopAllCoroutines();
        StartCoroutine(MoveRoutine());
    }

    public void Interact()
    {
        if (!released)
            return;

        Debug.Log("Game End");
        // TODO: Boat interaction.
    }

    private IEnumerator MoveRoutine()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}