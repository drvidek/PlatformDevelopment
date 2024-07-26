using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePair : MonoBehaviour
{
    [SerializeField] private float speed = 4f;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Go()
    {
        rb.velocity = Vector2.left * speed;
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}
