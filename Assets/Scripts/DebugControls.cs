using UnityEngine;
using UnityEngine.InputSystem;

public class DebugControls : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction restartAction;

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
    }

    private void HandleRestart()
    {
        if (restartAction.WasPressedThisFrame())
            gameManager.RestartLevel();
    }
}