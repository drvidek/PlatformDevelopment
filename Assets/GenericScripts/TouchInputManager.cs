using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TouchInputManager : MonoBehaviour
{
    PlayerInput input;
    InputAction actionPosition;
    InputAction actionTouch;

    public UnityEvent<Vector2> onTouch;
    public UnityEvent<Vector2> onTouchHeld;


    private bool _touchActive;

    public bool touchActive
    {
        get
        {
            return _touchActive;
        }
        private set
        {
            _touchActive = value;
        }
    }

    public Vector2 touchPosition
    {
        get
        {
            return actionPosition.ReadValue<Vector2>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        input = GetComponent<PlayerInput>();
        actionPosition = input.actions["TouchPosition"];
        actionTouch = input.actions["TouchPress"];
        actionTouch.performed += Touch;
        actionTouch.canceled += EndTouch;
    }

    void OnEnable()
    {
        actionTouch.performed += Touch;
        actionTouch.canceled += EndTouch;
    }

    void OnDisable()
    {
        actionTouch.performed -= Touch;
        actionTouch.canceled -= EndTouch;
    }

    void Update()
    {
        if (touchActive)
        {
            onTouchHeld.Invoke(touchPosition);
        }
    }

    public void Touch(InputAction.CallbackContext context)
    {
        touchActive = true;
        onTouch.Invoke(touchPosition);
    }

    public void EndTouch(InputAction.CallbackContext context)
    {
        touchActive = false;
    }
}
