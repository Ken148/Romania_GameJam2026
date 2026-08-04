using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class WirePuzzleController : MonoBehaviour
{
    public static WirePuzzleController Instance;

    [Header("Setup")]
    public RectTransform wiresContainer;
    public GameObject insulationPrefab;
    public WireSocket[] leftSockets;
    public WireSocket[] rightSockets;
    public Color[] wireColors;

    public Action OnPuzzleSolved;

    public MonoBehaviour playerController;

    private WireSocket selectedSocket;
    private int connectedCount = 0;

    void Awake() => Instance = this;

    void OnEnable()
    {
        ResetPuzzle();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerController != null) playerController.enabled = false;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerController != null) playerController.enabled = true;
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

    void DrawInsulation(WireSocket left, WireSocket right)
    {
        RectTransform line = Instantiate(insulationPrefab, wiresContainer).GetComponent<RectTransform>();
        Vector3 start = left.GetComponent<RectTransform>().position;
        Vector3 end = right.GetComponent<RectTransform>().position;
        Vector3 dir = end - start;

        line.position = start;
        line.sizeDelta = new Vector2(dir.magnitude, line.sizeDelta.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.Euler(0, 0, angle);

        Image img = line.GetComponent<Image>();
        if (img != null && wireColors != null && wireColors.Length > left.colorId)
            img.color = wireColors[left.colorId];
    }

    public void TestShowPuzzle()
    {
        gameObject.SetActive(true);
    }


}