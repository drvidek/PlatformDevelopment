using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    ///// For Send/Broadcast Messages. Function name must be 'OnMyInput'.
    public void OnJump(InputValue value)
    {
        Debug.Log("Jump was pressed via Send Messages: " + value.isPressed);
    }

    public void OnPointerDelta(InputValue value)
    {
        Debug.Log("Delta of pointer: " + value.Get<Vector2>());
    }

    ///// For UnityEvent. Function name can be anything.
    public void OnJump()
    {
        Debug.Log("Jump button pressed via Unity Event! No parameter.");
    }

    public void OnDirectionalInput(InputAction.CallbackContext context)
    {
        Debug.Log("Directional input received via Unity Event! " + context.ReadValue<Vector2>().ToString());
    }
}
