using UnityEngine;

public class GlassGroup : MonoBehaviour
{
    private Glass[] glassPieces;

    private void Awake()
    {
        glassPieces = GetComponentsInChildren<Glass>();
        SetDebrisEnabled(false);
    }

    public void SetDebrisEnabled(bool enabled)
    {
        foreach (Glass glass in glassPieces)
        {
            Debris debris = glass.GetComponent<Debris>();

            if (debris != null)
                debris.enabled = enabled;
        }
    }
}