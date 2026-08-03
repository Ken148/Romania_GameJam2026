using UnityEngine;

public interface ITool
{
    GameObject PickupPrefab { get; }
    ToolInput Input { get; }

    void Use(RaycastHit hit);

    
}