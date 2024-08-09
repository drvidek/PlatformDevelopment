using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchReciever : MonoBehaviour
{
    private int fingerId = -1;
    private ITouchable currentTouchable;

    // Update is called once per frame
    void Update()
    {
        if (TryGetTouch(ref fingerId, out Touch touch))
        {
            ManageTouch(touch.position);
            return;
        }

        NoTouch();
    }

    /// <summary>
    /// Checks if something is currently touched and gives it the new touch position, else casts a new touch at the position.
    /// </summary>
    /// <param name="touchPosition"></param>
    private void ManageTouch(Vector3 touchPosition)
    {
        if (!CurrentlyTouching(touchPosition))
            CastTouch(touchPosition);
    }

    /// <summary>
    /// Returns true if there's a currently touched ITouchable, and sends the touch position to it. Else returns false.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CurrentlyTouching(Vector3 position)
    {
        if (currentTouchable != null)
        {
            currentTouchable.OnTouchStay(position);
            return true;
        }
        return false;
    }

    /// <summary>
    /// If we're in the editor, check for simulated touches using the mouse. Else, end any active touch and reset fingerCurrentId.
    /// </summary>
    private void NoTouch()
    {
        fingerId = -1;

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            ManageTouch(Input.mousePosition);
            return;
        }
#endif
        if (currentTouchable != null)
        {
            currentTouchable.OnTouchEnd(Input.mousePosition);
            currentTouchable = null;
        }
    }

    /// <summary>
    /// Raycast from the given screen position and try to get a touchable component, beginning touch if one is found.
    /// </summary>
    /// <param name="screenPosition"></param>
    private void CastTouch(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent<ITouchable>(out ITouchable touched))
            {
                currentTouchable = touched;
                currentTouchable.OnTouchBegin(screenPosition);
            }
        }
    }

    /// <summary>
    /// Try to get or maintain a unique touch.
    /// </summary>
    /// <param name="touchID"></param>
    /// <param name="touchFound"></param>
    /// <returns></returns>
    private bool TryGetTouch(ref int touchID, out Touch touchFound)
    {
        foreach (Touch touch in Input.touches)
        {
            if (touchID == -1 || touch.fingerId == touchID)
            {
                touchFound = touch;
                touchID = touchFound.fingerId;
                return true;
            }
        }
        touchFound = new();
        return false;
    }
}


public interface ITouchable
{
    public void OnTouchBegin(Vector3 touchPosition);
    public void OnTouchStay(Vector3 touchPosition);
    public void OnTouchEnd(Vector3 touchPosition);
}
