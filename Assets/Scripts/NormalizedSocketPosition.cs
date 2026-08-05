using UnityEngine;

[ExecuteAlways]
public class NormalizedSocketPosition : MonoBehaviour
{
    public Vector2 normalizedPosition;
    public RectTransform background;

    private RectTransform selfRT;

    void Awake()
    {
        selfRT = GetComponent<RectTransform>();
    }

    [ContextMenu("Apply Normalized Position")]
    public void ApplyPosition()
    {
        if (selfRT == null) selfRT = GetComponent<RectTransform>();
        if (background == null) return;

        float width = background.rect.width;
        float height = background.rect.height;

        Vector2 localPos = new Vector2(
            (normalizedPosition.x - 0.5f) * width,
            (normalizedPosition.y - 0.5f) * height
        );

        selfRT.anchoredPosition = localPos;
    }

    [ContextMenu("Capture Current Position as Normalized")]
    public void CaptureCurrentPosition()
    {
        if (selfRT == null) selfRT = GetComponent<RectTransform>();
        if (background == null) return;

        float width = background.rect.width;
        float height = background.rect.height;

        normalizedPosition = new Vector2(
            selfRT.anchoredPosition.x / width + 0.5f,
            selfRT.anchoredPosition.y / height + 0.5f
        );
    }
}