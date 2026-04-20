using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
// 'Touch' and 'TouchPhase' are ambiguous between the new and old Input systems
// This declares that, in this script, 'Touch' is from new Input system by default
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
// Similarly, this makes sure we use the new enum, not the old one
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class MultiTouchReader : MonoBehaviour
{
    public UnityEvent onTouchBegin;
    public UnityEvent<Vector2> onTouchStay;
    public UnityEvent onTouchEnd;
    public UnityEvent onMultiTouchBegin;
    public UnityEvent onMultiTouchEnd;

    public UnityEvent<int> onCurrentFingerCount;

    int _touchCountPrevious;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Turns on enhanced touches, simplifying multi-touch reading 
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        // Get the current number of active touches
        int touchCountCurrent = Touch.activeTouches.Count;

        onCurrentFingerCount.Invoke(touchCountCurrent);

        // If there are none...
        if (touchCountCurrent == 0)
        {
            /// If we had touches previous frame...
            if (_touchCountPrevious > 0)
            {
                // Signal that all touches are over
                onTouchEnd.Invoke();

                if (_touchCountPrevious > 1)
                {
                    onMultiTouchEnd.Invoke();
                }
                _touchCountPrevious = 0;
            }
            return;
        }

        Touch touchFirst = new();
        // Get the first active touch
        foreach (Touch touch in Touch.activeTouches)
        {
            if (touch.ended)
                continue;
            touchFirst = touch;
            break;
        }

        // If the touch is just beginning...
        if (touchFirst.began)
        {
            // Signal we have a new touch
            onTouchBegin.Invoke();
        }

        // Signal the position of the touch
        onTouchStay.Invoke(touchFirst.screenPosition);

        // If we had 0 or 1 touch, and now have 2 or more...
        if (_touchCountPrevious < 2 && touchCountCurrent >= 2)
        {
            // Signal our multi-touch has started
            onMultiTouchBegin.Invoke();
        }

        // If we had 2 or more touches, and now have less...
        if (_touchCountPrevious >= 2 && touchCountCurrent < 2)
        {
            // Signal our multi-touch is over
            onMultiTouchEnd.Invoke();
        }

        // Track how many fingers we had previously
        _touchCountPrevious = touchCountCurrent;
    }
}
