using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;
using System;
using System.Collections;
using UnityEngine.InputSystem;

public class WirePuzzleControllerMultiRound : MonoBehaviour
{
    public static WirePuzzleControllerMultiRound Instance;

    public FirstPersonCamera cameraController;

    public PlayerInput playerInput;

    [Header("Rounds")]
    public GameObject[] roundObjects;
    public WireRoundController[] roundControllers;
    private int currentRoundIndex = 0;

    public Action OnPuzzleSolved;

    public FirstPersonMovement playerController;

    public Volume blurVolume;
    public float blurTransitionSpeed = 5f;
    private float targetWeight = 0f;

    public GameObject darkenOverlay;

    public Image successGlow;
    public float closeDelay = 2f;

    [Header("Timer")]
    public TMP_Text timerText;
    public CircularTimer circularTimer;
    public float timeLimit = 60f;
    private float currentTime;
    private bool timerActive = false;

    void Awake()
    {
        Instance = this;
        OnPuzzleSolved += HandlePuzzleSolved;
    }

    void OnEnable()
    {
        currentRoundIndex = 0;
        StartRound(currentRoundIndex);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        if (cameraController != null) cameraController.enabled = false;

        targetWeight = 1f;
        if (darkenOverlay != null) darkenOverlay.SetActive(true);

        currentTime = timeLimit;
        timerActive = true;

        if (circularTimer != null) circularTimer.SetTime(currentTime, timeLimit);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerController != null) playerController.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        if (cameraController != null) cameraController.enabled = true;

        targetWeight = 0f;
        if (blurVolume != null) blurVolume.weight = 0f;
        if (darkenOverlay != null) darkenOverlay.SetActive(false);

        timerActive = false;

        for (int i = 0; i < roundObjects.Length; i++)
            roundObjects[i].SetActive(false);
    }

    void Update()
    {
        if (blurVolume != null && blurVolume.weight != targetWeight)
        {
            blurVolume.weight = Mathf.MoveTowards(blurVolume.weight, targetWeight, blurTransitionSpeed * Time.unscaledDeltaTime);
        }

        if (timerActive)
        {
            currentTime -= Time.unscaledDeltaTime;

            if (timerText != null)
                timerText.text = Mathf.Max(0, Mathf.CeilToInt(currentTime)).ToString();

            if (circularTimer != null)
                circularTimer.SetTime(currentTime, timeLimit);

            if (currentTime <= 0f)
            {
                timerActive = false;
                StartCoroutine(TimeUpSequence());
            }
        }
    }

    void StartRound(int index)
    {
        Debug.Log($"StartRound called with index: {index}, roundObjects.Length: {roundObjects.Length}");

        for (int i = 0; i < roundObjects.Length; i++)
        {
            Debug.Log($"Setting roundObjects[{i}] ({roundObjects[i].name}) active = {i == index}");
            roundObjects[i].SetActive(i == index);
        }

        Debug.Log($"Calling ResetRound on roundControllers[{index}]: {roundControllers[index].gameObject.name}");
        roundControllers[index].ResetRound();
        roundControllers[index].OnRoundSolved = OnRoundSolved;
    }

    IEnumerator TimeUpSequence()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        currentRoundIndex = 0;
        StartRound(currentRoundIndex);

        currentTime = timeLimit;
        timerActive = true;

        if (circularTimer != null) circularTimer.SetTime(currentTime, timeLimit);
    }

    public void OnRoundSolved()
    {
        Debug.Log($"OnRoundSolved called! currentRoundIndex was: {currentRoundIndex}");
        currentRoundIndex++;
        Debug.Log($"New currentRoundIndex: {currentRoundIndex}, roundObjects.Length: {roundObjects.Length}");

        if (currentRoundIndex >= roundObjects.Length)
        {
            Debug.Log("Puzzle fully solved!");
            timerActive = false;
            OnPuzzleSolved?.Invoke();
        }
        else
        {
            Debug.Log($"Starting next round: {currentRoundIndex}");
            StartRound(currentRoundIndex);
        }
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
        gameObject.SetActive(false);
    }

    [ContextMenu("TEST: Show Puzzle")]
    public void TestShowPuzzle()
    {
        gameObject.SetActive(true);
    }
}