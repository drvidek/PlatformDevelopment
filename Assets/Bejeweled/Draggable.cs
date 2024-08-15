using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 touchOffset = Vector2.zero;
    private bool isDragging;
    private RectTransform rectTransform => (RectTransform)transform;

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
        if (isDragging)
        rectTransform.anchoredPosition = eventData.position;// + touchOffset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging)
        {
            touchOffset = eventData.position - rectTransform.anchoredPosition;
            isDragging = true;
        }
        Debug.Log(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
