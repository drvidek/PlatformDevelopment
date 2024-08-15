using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableController : MonoBehaviour
{
    [SerializeField] private bool allowMultiTouch;
    [SerializeField] private int[] fingerIDs = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    [SerializeField] private ITouchable[] currentTouchable = new ITouchable[10];

    // Update is called once per frame
    void Update()
    {
        if (allowMultiTouch)
            CheckMultiTouch();
        else
            CheckSingleTouch();
    }

    /// <summary>
    /// Try to get or maintain a single finger on the screen.
    /// </summary>
    private void CheckSingleTouch()
    {
        if (TouchReciever.TryGetTouch(ref fingerIDs[0], out Touch touch))
        {
            Debug.Log("Found touch");
            ManageTouch(touch.position);
            return;
        }

        NoTouch();
    }

    /// <summary>
    /// Try to get or maintain up to 10 unique touches.
    /// </summary>
    private void CheckMultiTouch()
    {
        for (int i = 0; i < fingerIDs.Length; i++)
        {
            if (TouchReciever.TryGetTouch(ref fingerIDs[i], out Touch touch, fingerIDs))
            {
                Debug.Log("Found touch");
                ManageTouch(touch.position, i);
                continue;
            }

            NoTouch(i);
        }
    }

    /// <summary>
    /// Checks if something is currently touched and gives it the new touch position, else casts a new touch at the position.
    /// </summary>
    /// <param name="touchPosition"></param>
    private void ManageTouch(Vector3 touchPosition, int fingerIndex = 0)
    {
        if (!CurrentlyTouching(touchPosition, fingerIndex))
            CastTouch(touchPosition, fingerIndex);
    }

    /// <summary>
    /// Returns true if there's a currently touched ITouchable, and sends the touch position to it. Else returns false.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CurrentlyTouching(Vector3 position, int fingerIndex = 0)
    {
        if (currentTouchable[fingerIndex] != null)
        {
            currentTouchable[fingerIndex].OnTouchStay(position);
            return true;
        }
        return false;
    }

    /// <summary>
    /// If we're in the editor, check for simulated touches using the mouse. Else, end any active touch and reset fingerCurrentId.
    /// </summary>
    private void NoTouch(int fingerIndex = 0)
    {

#if UNITY_EDITOR
        if (fingerIndex == 0 && Input.GetMouseButton(0))
        {
            ManageTouch(Input.mousePosition, 0);
            return;
        }
#endif
        fingerIDs[fingerIndex] = -1;
        if (currentTouchable[fingerIndex] != null)
        {
            currentTouchable[fingerIndex].OnTouchEnd(Input.mousePosition);
            currentTouchable[fingerIndex] = null;
        }
    }

    /// <summary>
    /// Raycast from the given screen position and try to get a touchable component, beginning touch if one is found.
    /// </summary>
    /// <param name="screenPosition"></param>
    private void CastTouch(Vector3 screenPosition, int fingerIndex = 0)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent<ITouchable>(out ITouchable touched))
            {
                currentTouchable[fingerIndex] = touched;
                currentTouchable[fingerIndex].OnTouchBegin(screenPosition);
            }
        }
    }

}
