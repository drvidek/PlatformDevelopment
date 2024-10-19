using UnityEngine;

public static class TouchReciever
{

    /// <summary>
    /// Try to get or maintain a unique touch.
    /// </summary>
    /// <param name="touchID"></param>
    /// <param name="touchFound"></param>
    /// <returns></returns>
    public static bool TryGetTouch(ref int touchID, out Touch touchFound, params int[] excludeIDs)
    {
        foreach (Touch touch in Input.touches)
        {
            bool cont = false;


            //check if the current touch is registered to another finger index already
            foreach (int id in excludeIDs)
            {
                //ignore our own touchID
                if (touchID == id)
                    continue;

                //if the current touch is registered in the excludeIDs, move to the next touch
                if (touch.fingerId == id)
                {
                    cont = true;
                    break;
                }
            }
            if (cont)
            {
                Debug.Log("Touch already logged");
                continue;
            }


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
    public abstract void OnTouchBegin(Vector3 touchPosition);
    public abstract void OnTouchStay(Vector3 touchPosition);
    public abstract void OnTouchEnd(Vector3 touchPosition);
}

public interface ITappable : ITouchable
{
    public static float TapAllowance = 0.3f;

    public static void TrackTouchLength(ref float time)
    {
        time += Time.deltaTime;
    }

    public static bool ValidTap(float time)
    {
        return time < TapAllowance;
    }
}

public interface IDraggable : ITouchable
{
    
}