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

    private WireSocket selectedSocket;
    private int connectedCount = 0;

    void Awake() => Instance = this;

    void OnEnable()
    {
        ResetPuzzle();
        ShuffleSockets();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void ShuffleSockets()
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < leftSockets.Length; i++) ids.Add(i);

        for (int i = 0; i < leftSockets.Length; i++)
            leftSockets[i].colorId = ids[i];

        List<int> shuffled = new List<int>(ids);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        for (int i = 0; i < rightSockets.Length; i++)
            rightSockets[i].colorId = shuffled[i];

        for (int i = 0; i < leftSockets.Length; i++)
            leftSockets[i].GetComponent<Image>().color = wireColors[leftSockets[i].colorId];
        for (int i = 0; i < rightSockets.Length; i++)
            rightSockets[i].GetComponent<Image>().color = wireColors[rightSockets[i].colorId];
    }

    public void ResetPuzzle()
    {
        connectedCount = 0;
        selectedSocket = null;
        foreach (Transform child in wiresContainer) Destroy(child.gameObject);
        foreach (var s in leftSockets) { s.isConnected = false; s.SetHighlight(false); }
        foreach (var s in rightSockets) { s.isConnected = false; s.SetHighlight(false); }
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
        if (img != null) img.color = wireColors[left.colorId];
    }


}