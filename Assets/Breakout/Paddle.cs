using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour, IReset
{
    [Tooltip("How fast the paddle can move towards the touch input")]
    [SerializeField] private float speed;

    private int fingerCurrentId = -1;

    // Update is called once per frame
    void Update()
    {
        if (!RoundManager.Singleton.RoundActive)
            return;

        Vector2 point = new Vector2();

#if UNITY_EDITOR
        if (!Input.GetMouseButton(0))
            return;

        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

#else

        if (!TouchReciever.TryGetTouch(ref fingerCurrentId, out Touch touch))
        {
            fingerCurrentId = -1;
            return;
        }

        point = Camera.main.ScreenToWorldPoint(touch.position);

#endif

        MoveTowards(point);

    }

    private void MoveTowards(Vector2 position)
    {
        Vector3 target = new Vector3(position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void Reset()
    {
        transform.position = new(0, transform.position.y, transform.position.z);
    }
}
