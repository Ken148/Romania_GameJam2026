using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;

public class WirePuzzleController : MonoBehaviour
{
    public static WirePuzzleController Instance;

    public RectTransform wiresContainer;
    public GameObject insulationPrefab;
    public WireSocket[] leftSockets;
    public WireSocket[] rightSockets;
    public Color[] wireColors;

    public Action OnPuzzleSolved;

    public MonoBehaviour playerController;

    public Volume blurVolume;
    public float blurTransitionSpeed = 5f;
    private float targetWeight = 0f;

    public float insulationInsetFromSocket = 15f;

    public InsulationOverride[] insulationOverrides;

    private WireSocket selectedSocket;
    private int connectedCount = 0;

    void Awake() => Instance = this;

    void OnEnable()
    {
        ResetPuzzle();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerController != null) playerController.enabled = false;

        targetWeight = 1f;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerController != null) playerController.enabled = true;

        targetWeight = 0f;
        if (blurVolume != null) blurVolume.weight = 0f;
    }

    void Update()
    {
        if (blurVolume != null && blurVolume.weight != targetWeight)
        {
            blurVolume.weight = Mathf.MoveTowards(blurVolume.weight, targetWeight, blurTransitionSpeed * Time.unscaledDeltaTime);
        }
    }

    public void ResetPuzzle()
    {
        connectedCount = 0;
        selectedSocket = null;

        foreach (Transform child in wiresContainer) Destroy(child.gameObject);

        foreach (var s in leftSockets)
        {
            s.isConnected = false;
            s.SetHighlight(false);
        }
        foreach (var s in rightSockets)
        {
            s.isConnected = false;
            s.SetHighlight(false);
        }
    }

    public void OnSocketClicked(WireSocket socket)
    {
        if (socket.isConnected) return;

        if (selectedSocket == null)
        {
            selectedSocket = socket;
            socket.SetHighlight(true);
            return;
        }

        if (selectedSocket == socket)
        {
            socket.SetHighlight(false);
            selectedSocket = null;
            return;
        }

        if (selectedSocket.isLeftSide == socket.isLeftSide)
        {
            selectedSocket.SetHighlight(false);
            selectedSocket = socket;
            socket.SetHighlight(true);
            return;
        }

        TryConnect(selectedSocket, socket);
        selectedSocket.SetHighlight(false);
        selectedSocket = null;
    }

    void TryConnect(WireSocket a, WireSocket b)
    {
        WireSocket left = a.isLeftSide ? a : b;
        WireSocket right = a.isLeftSide ? b : a;

        if (left.colorId == right.colorId)
        {
            DrawInsulation(left, right);

            left.isConnected = true;
            right.isConnected = true;
            connectedCount++;

            if (connectedCount >= leftSockets.Length)
                OnPuzzleSolved?.Invoke();
        }
    }

    InsulationOverride GetOverride(int colorId)
    {
        if (insulationOverrides == null) return null;
        foreach (var o in insulationOverrides)
            if (o.colorId == colorId && o.useManualTransform) return o;
        return null;
    }

    void DrawInsulation(WireSocket left, WireSocket right)
    {
        RectTransform line = Instantiate(insulationPrefab, wiresContainer).GetComponent<RectTransform>();

        RectTransform leftRT = left.GetComponent<RectTransform>();
        RectTransform rightRT = right.GetComponent<RectTransform>();

        Canvas canvas = wiresContainer.GetComponentInParent<Canvas>();
        Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        Vector2 startScreen = RectTransformUtility.WorldToScreenPoint(cam, leftRT.position);
        Vector2 endScreen = RectTransformUtility.WorldToScreenPoint(cam, rightRT.position);

        Vector2 startLocal, endLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(wiresContainer, startScreen, cam, out startLocal);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(wiresContainer, endScreen, cam, out endLocal);

        Vector2 dir = endLocal - startLocal;
        float fullLength = dir.magnitude;
        Vector2 dirNormalized = fullLength > 0.001f ? dir.normalized : Vector2.right;

        Vector2 adjustedStart = startLocal + dirNormalized * insulationInsetFromSocket;
        float adjustedLength = fullLength - insulationInsetFromSocket * 2f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var ov = GetOverride(left.colorId);
        if (ov != null)
        {
            adjustedStart += ov.manualOffset;
            angle += ov.manualAngleOffset;
            adjustedLength *= ov.manualScale;
        }

        line.anchoredPosition = adjustedStart;
        line.sizeDelta = new Vector2(adjustedLength, line.sizeDelta.y);
        line.localRotation = Quaternion.Euler(0, 0, angle);

        Image img = line.GetComponent<Image>();
        if (img != null && wireColors != null && wireColors.Length > left.colorId)
            img.color = wireColors[left.colorId];
    }

    [ContextMenu("TEST: Show Puzzle")]
    public void TestShowPuzzle()
    {
        gameObject.SetActive(true);
    }
}

[System.Serializable]
public class InsulationOverride
{
    public int colorId;
    public bool useManualTransform = false;
    public Vector2 manualOffset;
    public float manualAngleOffset;
    public float manualScale = 1f;
}