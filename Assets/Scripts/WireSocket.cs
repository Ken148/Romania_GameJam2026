using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireSocket : MonoBehaviour, IPointerClickHandler
{
    public int colorId;
    public bool isLeftSide;
    [HideInInspector] public bool isConnected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isConnected) return;
        WirePuzzleController.Instance.OnSocketClicked(this);
    }

    public void SetHighlight(bool on)
    {
        transform.localScale = on ? Vector3.one * 1.15f : Vector3.one;
    }
}