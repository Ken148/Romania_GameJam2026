using UnityEngine;
using UnityEngine.InputSystem;

public class DebugControls : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction restartAction;
    private InputAction solvePuzzlesAction;
    private InputAction insertFusesAction;

    private GameManager gameManager;

    private void Awake()
    {
        playerInput = playerInput = GetComponent<PlayerInput>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (!ValidateDependencies())
        {
            enabled = false;
            return;
        }

        restartAction = playerInput.actions["Debug/Restart"];
        solvePuzzlesAction = playerInput.actions["Debug/SolvePuzzlesFloor2"];
        insertFusesAction = playerInput.actions["Debug/InsertFusesFloor2"];
    }

    private bool ValidateDependencies()
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found.", this);
            return false;
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found.", this);
            return false;
        }

        return true;
    }

    private void Update()
    {
        HandleRestart();
        HandleMock3PuzzlesSolved();
    }

    private void HandleRestart()
    {
        if (restartAction.WasPressedThisFrame())
            gameManager.RestartLevel();
    }

    private void HandleMock3PuzzlesSolved()
    {
        if (!solvePuzzlesAction.WasPressedThisFrame())
            return;

        WirePuzzleGroup puzzleGroup = FindAnyObjectByType<WirePuzzleGroup>();

        if (puzzleGroup == null)
            return;

        for (int i = 0; i < 3; i++)
            puzzleGroup.PuzzleSolved();
    }

    private void Handle2FusesInserted()
    {
        if (!insertFusesAction.WasPressedThisFrame())
            return;

        FuseGroup fuseGroup = FindAnyObjectByType<FuseGroup>();

        if (fuseGroup == null)
            return;

        fuseGroup.FuseInserted();
        fuseGroup.FuseInserted();
    }
}