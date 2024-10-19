using System;
using System.Collections.Generic;
using UnityEngine;

public struct Swipe
{
    private static float threshold = 150f;
    public static float Threshold => threshold;

    public float distance;
    public Vector2 direction;
    public Vector2 start;
    public Vector2 end;

    public Swipe(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
        distance = Vector2.Distance(start, end);
        direction = (end - start).normalized;
    }

    public Vector3 WorldStart => ScreenInteractionController.camera.ScreenToWorldPoint(start);
    public Vector3 WorldEnd => ScreenInteractionController.camera.ScreenToWorldPoint(end);
    public float WorldDistance => Vector3.Distance(WorldStart, WorldEnd);
}

public class ScreenInteractionController : MonoBehaviour
{
    public int fingerID;
    public Vector3 startPosition;
    public Vector3 lastPosition;
    public ITouchable touchable;

    public float tapTimeThreshold = 0.3f;
    public float tapDistanceThreshold = 5f;

    private float timeStartTouch;

    public static Camera camera;
    private static Swipe _swipeNewest;
    public static Swipe Newest => _swipeNewest;
    public static Action<Swipe> onSwipeScreen;
    public static Action onNewTouch;
    public static Action onTapScreen;


    private void Start()
    {
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouch();
    }

    /// <summary>
    /// Try to get or maintain a single finger on the screen.
    /// </summary>
    private void CheckTouch()
    {
        bool newTouch = fingerID == -1;
        if (TouchReciever.TryGetTouch(ref fingerID, out Touch touch))
        {
            if (newTouch)
                CastNewTouch(startPosition);
            else
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
        lastPosition = touchPosition;

        if (touchable == null)
            return;

        touchable.OnTouchStay(touchPosition);
    }

    /// <summary>
    /// If we're in the editor, check for simulated touches using the mouse. Else, end any active touch and reset fingerCurrentId.
    /// </summary>
    private void NoTouch()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            bool newTouch = fingerID == -1;
            if (newTouch)
            {
                fingerID = -2;
                CastNewTouch(Input.mousePosition);
            }
            else
                ManageTouch(Input.mousePosition);
            return;
        }
#endif
        if (fingerID != -1)
        {
            if (touchable != null)
            {
                touchable.OnTouchEnd(lastPosition);
            }
            else
            {
                if (!TryToSwipe(startPosition, lastPosition))
                    TryToTap(startPosition, lastPosition);
            }
        }

        fingerID = -1;
        touchable = null;

        startPosition = Vector3.zero;
        lastPosition = Vector3.zero;
    }

    /// <summary>
    /// Raycast from the given screen position and try to get a touchable component, beginning touch if one is found.
    /// </summary>
    /// <param name="screenPosition"></param>
    private bool CastNewTouch(Vector3 screenPosition)
    {
        startPosition = screenPosition;

        timeStartTouch = Time.time;

        onNewTouch?.Invoke();

        Ray ray = camera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent<ITouchable>(out touchable))
            {
                touchable.OnTouchBegin(screenPosition);
                return true;
            }
        }
        return false;
    }

    public bool TryToSwipe(Vector2 start, Vector2 end)
    {
        if (Vector2.Distance(start, end) > Swipe.Threshold)
        {
            _swipeNewest = new(start, end);
            onSwipeScreen?.Invoke(_swipeNewest);
            return true;
        }
        return false;
    }

    public bool TryToTap(Vector2 start, Vector2 end)
    {
        if (Vector2.Distance(start, end) < tapDistanceThreshold && Time.time - timeStartTouch < tapTimeThreshold)
        {
            onTapScreen?.Invoke();
            return true;
        }
        return false;
    }
}