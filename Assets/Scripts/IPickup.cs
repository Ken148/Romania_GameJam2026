using UnityEngine;

public interface IPickup
{
    GameObject WorldObject { get; }
    GameObject HeldPrefab { get; }
}