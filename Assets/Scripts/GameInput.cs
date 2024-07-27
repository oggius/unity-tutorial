using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }

    // returns normalized input vector
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        Debug.Log(inputVector);
        return inputVector.normalized;
    }

}
