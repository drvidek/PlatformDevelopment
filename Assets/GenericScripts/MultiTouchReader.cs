using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
// 'Touch' is ambiguous between the new and old Input systems
// This declares that, in this script, 'Touch' is from new Input system by default
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MultiTouchReader : MonoBehaviour
{
    public UnityEvent onTouchBegin;
    public UnityEvent<Vector2> onTouchStay;
    public UnityEvent onTouchEnd;
    public UnityEvent<int> onFingerCountChange;

    private List<Touch> currentTouches = new();

    private int _touchCountCurrent;

    public int touchCountCurrent
    {
        get => _touchCountCurrent;
        private set
        {
            if (value != _touchCountCurrent)
            {
                onFingerCountChange.Invoke(value);
            }
            if (_touchCountCurrent == 0 && value > 0)
            {
                onTouchBegin.Invoke();
            }
            if (_touchCountCurrent > 0 && value == 0)
            {
                onTouchEnd.Invoke();
            }

            _touchCountCurrent = value;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Turns on enhanced touches, simplifying multi-touch reading 
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        print(string.Join(",\n", Touch.activeTouches));

        touchCountCurrent = Touch.activeTouches.Count;

        if (touchCountCurrent == 0)
            return;

        onTouchStay.Invoke(Touch.activeTouches[0].screenPosition);
    }
}
