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
        roundController.OnSocketClicked(this);
    }

    public void SetHighlight(bool on)
    {
        transform.localScale = on ? Vector3.one * 1.15f : Vector3.one;
    }
}