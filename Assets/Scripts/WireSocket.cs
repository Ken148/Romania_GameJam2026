using UnityEngine;
using UnityEngine.EventSystems;

public class WireSocket : MonoBehaviour, IPointerClickHandler
{
    public int colorId;
    public bool isLeftSide;
    [HideInInspector] public bool isConnected = false;

    public WireRoundController roundController;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isConnected) return;

        if (roundController != null)
        {
            roundController.OnSocketClicked(this);
        }
        else if (WirePuzzleController.Instance != null)
        {
            WirePuzzleController.Instance.OnSocketClicked(this);
        }
    }

    public void SetHighlight(bool on)
    {
        transform.localScale = on ? Vector3.one * 1.15f : Vector3.one;
    }
}