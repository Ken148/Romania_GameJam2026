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

    [Header("Player")]
    [SerializeField] private FirstPersonCamera playerCamera;
    [SerializeField] private FirstPersonMovement playerMovement;
    [SerializeField] private PlayerInteractor playerInteractor;

    [Header("Rounds")]
    public GameObject[] roundObjects;
    public WireRoundController[] roundControllers;
    private int currentRoundIndex = 0;

    public Action OnPuzzleSolved;

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

    [SerializeField] private PuzzlePlate puzzlePlate;

    [SerializeField] private GameObject puzzle;

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
        currentRoundIndex = 0;
        StartRound(currentRoundIndex);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerCamera != null) playerCamera.enabled = false;
        if (playerInteractor != null) playerInteractor.enabled = false;

        targetWeight = 1f;
        if (darkenOverlay != null)
            darkenOverlay.SetActive(true);

        currentTime = timeLimit;
        timerActive = true;

        if (circularTimer != null)
            circularTimer.SetTime(currentTime, timeLimit);
    }

    void OnDisable()
    {
        Debug.Log("Puzzle OnDisable");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCamera != null) playerCamera.enabled = true;
        if (playerInteractor != null) playerInteractor.enabled = true;

        targetWeight = 0f;

        if (blurVolume != null)
            blurVolume.weight = 0f;

        if (darkenOverlay != null)
            darkenOverlay.SetActive(false);

        timerActive = false;

        foreach (GameObject round in roundObjects)
            round.SetActive(false);
    }

    void Update()
    {
        if (blurVolume != null)
        {
            blurVolume.weight = Mathf.MoveTowards(
                blurVolume.weight,
                targetWeight,
                blurTransitionSpeed * Time.unscaledDeltaTime);
        }

        if (!timerActive)
            return;

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

    void StartRound(int index)
    {
        for (int i = 0; i < roundObjects.Length; i++)
            roundObjects[i].SetActive(i == index);

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

        if (circularTimer != null)
            circularTimer.SetTime(currentTime, timeLimit);
    }

    public void OnRoundSolved()
    {
        currentRoundIndex++;

        if (currentRoundIndex >= roundObjects.Length)
        {
            timerActive = false;
            OnPuzzleSolved?.Invoke();
        }
        else
        {
            StartRound(currentRoundIndex);
        }
    }

    void HandlePuzzleSolved()
    {
        puzzlePlate.PuzzleSolved();

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerMovement.enabled = true;
        playerCamera.enabled = true;
        playerInteractor.enabled = true;

        if (blurVolume != null)
            blurVolume.weight = 0f;

        if (darkenOverlay != null)
            darkenOverlay.SetActive(false);

        puzzle.SetActive(false);
    }
}