using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour, ITouchable
{
    private Vector2 offset;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnTouchBegin(Vector3 screenPosition)
    {
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(screenPosition);
        spriteRenderer.color = Color.green;
    }
    
    public void OnTouchStay(Vector3 screenPosition)
    {
        Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = touchWorldPosition + offset;
        transform.position += Vector3.back;
    }

    public void OnTouchEnd(Vector3 screenPosition)
    {
        spriteRenderer.color = Color.white;
        transform.position += Vector3.forward;
    }
}
