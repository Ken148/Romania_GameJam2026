using UnityEngine;

[ExecuteAlways]
public class NormalizedTransform : MonoBehaviour
{
    public Vector2 normalizedPosition;
    public float rotationZ;
    public Vector2 normalizedSize;

    public RectTransform background;

    private RectTransform selfRT;

    void Awake()
    {
        selfRT = GetComponent<RectTransform>();
    }

    [ContextMenu("Capture Current Transform")]
    public void CaptureCurrentTransform()
    {
        if (selfRT == null) selfRT = GetComponent<RectTransform>();
        if (background == null) return;

        float width = background.rect.width;
        float height = background.rect.height;

        normalizedPosition = new Vector2(
            selfRT.anchoredPosition.x / width + 0.5f,
            selfRT.anchoredPosition.y / height + 0.5f
        );

        rotationZ = selfRT.localEulerAngles.z;

        normalizedSize = new Vector2(
            selfRT.sizeDelta.x / width,
            selfRT.sizeDelta.y / height
        );
    }

    [ContextMenu("Apply Normalized Transform")]
    public void ApplyNormalizedTransform()
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
        selfRT.localEulerAngles = new Vector3(0, 0, rotationZ);
        selfRT.sizeDelta = new Vector2(normalizedSize.x * width, normalizedSize.y * height);
    }
}