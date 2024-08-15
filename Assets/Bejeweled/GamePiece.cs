using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePiece : MonoBehaviour, ITouchable
{
    private Vector2 offset;

    protected SpriteRenderer spriteRenderer;

    protected Color colorBase;

    public abstract void Initialise();

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        colorBase = spriteRenderer.color;
    }

    public virtual void OnTouchBegin(Vector3 screenPosition)
    {
        offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(screenPosition);
        spriteRenderer.color += Color.white * 0.1f;
    }
    
    public virtual void OnTouchStay(Vector3 screenPosition)
    {
        Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        transform.position = touchWorldPosition + offset;
        transform.position += Vector3.back;
    }

    public virtual void OnTouchEnd(Vector3 screenPosition)
    {
        spriteRenderer.color = colorBase;
        transform.position += Vector3.forward;
    }
}
