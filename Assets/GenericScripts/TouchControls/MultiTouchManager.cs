using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchManager : MonoBehaviour
{
    [SerializeField] private int[] fingerId = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    [SerializeField] private ITouchable[] currentTouchable = new ITouchable[10];

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < fingerId.Length; i++)
        {
            if (TouchReciever.TryGetTouch(ref fingerId[i], out Touch touch, fingerId))
            {
                Debug.Log("Found touch");
                ManageTouch(touch.position, i);
                continue;
            }

            NoTouch(i);
        }

        if (Input.touchCount == 0)
            NoTouch(0);

    }

    /// <summary>
    /// Checks if something is currently touched and gives it the new touch position, else casts a new touch at the position.
    /// </summary>
    /// <param name="touchPosition"></param>
    private void ManageTouch(Vector3 touchPosition, int fingerIndex)
    {
        if (!CurrentlyTouching(touchPosition, fingerIndex))
            CastTouch(touchPosition, fingerIndex);
    }

    /// <summary>
    /// Returns true if there's a currently touched ITouchable, and sends the touch position to it. Else returns false.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool CurrentlyTouching(Vector3 position, int fingerIndex)
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
    private void NoTouch(int fingerIndex)
    {

#if UNITY_EDITOR
        if (fingerIndex == 0 && Input.GetMouseButton(0))
        {
            ManageTouch(Input.mousePosition, 0);
            return;
        }
#endif
        fingerId[fingerIndex] = -1;
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
    private void CastTouch(Vector3 screenPosition, int fingerIndex)
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
