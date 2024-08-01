using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler interactAction;

    private PlayerInputActions inputActions;

    public void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        interactAction?.Invoke(this, EventArgs.Empty);
    }

    // returns normalized input vector
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

}
