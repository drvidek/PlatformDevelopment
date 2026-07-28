using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 direction;
    
    public bool jumpPressed;
    public bool jumpHeld;

    void LateUpdate()
    {
        jumpPressed = false;
    }

    public void OnMove(InputValue inputValue)
    {
        direction = inputValue.Get<Vector2>();
    }

    public void OnJump(InputValue inputValue)
    {
        jumpPressed = inputValue.isPressed;
        jumpHeld = inputValue.isPressed;
        print("jump is pressed");
    }

    public void UpdateJumpValue(InputAction.CallbackContext callbackContext)
    {
        jumpPressed = callbackContext.performed;
        jumpHeld = callbackContext.performed || !callbackContext.canceled;
    }
}
