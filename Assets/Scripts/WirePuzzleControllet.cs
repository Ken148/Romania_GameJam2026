using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using System.Collections;

public class WirePuzzleController : MonoBehaviour
{
    public static WirePuzzleController Instance;

    public Image[] insulationSlots;
    public WireSocket[] leftSockets;
    public WireSocket[] rightSockets;

    public Action OnPuzzleSolved;

    public MonoBehaviour playerController;

    public Volume blurVolume;
    public float blurTransitionSpeed = 5f;
    private float targetWeight = 0f;

    public GameObject darkenOverlay;

    public Image successGlow;
    public float closeDelay = 0.5f;

    private WireSocket selectedSocket;
    private int connectedCount = 0;


    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonMovement playerMovement;
    [SerializeField] private PlayerInteractor playerInteractor;

    [SerializeField] private Canvas puzzleCanvas;


    void Awake()
    {
        Instance = this;
        OnPuzzleSolved += HandlePuzzleSolved;

        playerCamera = FindAnyObjectByType<FirstPersonCamera>();
        playerMovement = FindAnyObjectByType<FirstPersonMovement>();
        playerInteractor = FindAnyObjectByType<PlayerInteractor>();
    }

    void OnEnable()
    {
        ResetPuzzle();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerController != null) playerController.enabled = false;

        targetWeight = 1f;
        if (darkenOverlay != null) darkenOverlay.SetActive(true);

        playerMovement.enabled = false;
        playerCamera.enabled = false;
        playerInteractor.enabled = false;
    }

    void OnDisable()
    {
        playerMovement.enabled = true;
        playerCamera.enabled = true;
        playerInteractor.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerController != null) playerController.enabled = true;

        targetWeight = 0f;
        if (blurVolume != null) blurVolume.weight = 0f;
        if (darkenOverlay != null) darkenOverlay.SetActive(false);
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

        if (insulationSlots != null)
        {
            foreach (var slot in insulationSlots)
            {
                if (slot != null)
                {
                    Color c = slot.color;
                    c.a = 0f;
                    slot.color = c;
                }
            }
        }

        if (successGlow != null)
        {
            Color c = successGlow.color;
            c.a = 0f;
            successGlow.color = c;
        }

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
            ShowInsulation(left.colorId);

            left.isConnected = true;
            right.isConnected = true;
            connectedCount++;

            if (connectedCount >= leftSockets.Length)
                OnPuzzleSolved?.Invoke();
        }
    }

    void ShowInsulation(int colorId)
    {
        if (insulationSlots == null || colorId < 0 || colorId >= insulationSlots.Length) return;
        if (insulationSlots[colorId] == null) return;

        Color c = insulationSlots[colorId].color;
        c.a = 1f;
        insulationSlots[colorId].color = c;
    }

    void HandlePuzzleSolved()
    {
        if (successGlow != null)
        {
            Color c = successGlow.color;
            c.a = 1f;
            successGlow.color = c;
        }
        StartCoroutine(CloseAfterDelay());
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(closeDelay);
        puzzleCanvas.gameObject.SetActive(false);
    }

    [ContextMenu("TEST: Show Puzzle")]
    public void TestShowPuzzle()
    {
        gameObject.SetActive(true);
    }
}