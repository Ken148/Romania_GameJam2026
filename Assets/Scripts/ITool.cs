using UnityEngine;

public interface ITool
{
    GameObject PickupPrefab { get; }
    void Use(RaycastHit hit);
}