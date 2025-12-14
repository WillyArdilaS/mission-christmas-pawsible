using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    // === Input ===
    private PlayerInput playerInput;

    // === Global Events ===
    public event Action SelectLevelPressed;
    public event Action PausePressed;

    // === Sleigh Events ===
    public event Action MoveLeftPressed;
    public event Action MoveRightPressed;
    public event Action JumpPressed;

    // === Player Events ===
    public event Action GoInsidePressed;

    // === Properties ===
    public PlayerInput PlayerInput => playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void OnDestroy()
    {
        if (playerInput != null) playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap.name == "Global" && ctx.started)
        {
            switch (ctx.action.name)
            {
                case "Select Level":
                    SelectLevelPressed?.Invoke();
                    break;
                case "Pause":
                    PausePressed?.Invoke();
                    break;
            }
        }

        if (ctx.action.actionMap.name == "Sleigh" && ctx.started)
        {
            switch (ctx.action.name)
            {
                case "Move Left":
                    MoveLeftPressed?.Invoke();
                    break;
                case "Move Right":
                    MoveRightPressed?.Invoke();
                    break;
                case "Jump":
                    JumpPressed?.Invoke();
                    break;
            }
        }

        if (ctx.action.actionMap.name == "Player" && ctx.started)
        {
            if (ctx.action.name == "Go Inside")
            {
                GoInsidePressed?.Invoke();
            } 
        }
    }
}