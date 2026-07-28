using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    /// <summary>
    /// The input direction from the player, e.g. arrow keys
    /// </summary>
    public Vector2 direction { get; private set; }
    
    /// <summary>
    /// Whether jump was pressed this frame
    /// </summary>
    public bool jumpPressed{ get; private set; }

    /// <summary>
    /// Whether jump is currently held
    /// </summary>
    public bool jumpHeld { get; private set; }

    // LateUpdate() runs after everything has done Updare
    void LateUpdate()
    {
        jumpPressed = false;
    }

    #region BroadcastMessages
    // Methods must be named 'OnActionName', e.g. 'OnJump', 'OnSprint'
    // Methods may have an InputValue parameter for more than just "this was pressed"
    // BroadcastMessages messages will fire when buttons are 
    public void OnMove(InputValue inputValue)
    {
        // .Get<T>() gets the contained value from the input
        direction = inputValue.Get<Vector2>();
    }

    public void OnJump(InputValue inputValue)
    {
        // InputValue.isPressed will be true if the button has just been pressed
        jumpPressed = inputValue.isPressed;
        jumpHeld = inputValue.isPressed;
    }

    #endregion

    #region UnityEvent
    // Methods can be named anything
    // Methods may have an InputAction.CallbackContext parameter for specifics about the action
    // UnityEvent messages will fire anytime the input value changes, from press to release
    public void UpdateJumpValue(InputAction.CallbackContext callbackContext)
    {
        // CallbackContext contains different 'phases' we can check
        // .performed means the button was just pressed
        jumpPressed = callbackContext.performed;

        // .cancelled means the button is no longer pressed
        jumpHeld = callbackContext.performed || !callbackContext.canceled;
    }

    public void SetDirection(InputAction.CallbackContext callbackContext)
    {
        // .ReadValue<T>() gets the contained value from an input via a CallbackContext
        direction = callbackContext.ReadValue<Vector2>();
    }
    #endregion
}
