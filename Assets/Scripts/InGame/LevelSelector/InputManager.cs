using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    // === Input ===
    private PlayerInput playerInput;

    // === Events ===
    public event Action LevelSelected;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap.name == "Level Selector" && ctx.started)
        {
            if(ctx.action.name == "Continue") LevelSelected?.Invoke();
        }
    }
}