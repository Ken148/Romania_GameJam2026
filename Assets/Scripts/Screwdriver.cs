using UnityEngine;

public class Screwdriver : MonoBehaviour, ITool, IPickup
{
    [SerializeField] private GameObject heldPrefab;
    public GameObject HeldPrefab => heldPrefab;
    public GameObject WorldObject => gameObject;

    public void Use()
    {
    }
}