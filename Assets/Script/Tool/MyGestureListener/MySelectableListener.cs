using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MySelectableListener : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler
{
    public UnityEvent OnSelected;

    private bool dragging = false;

    public void OnDrag(PointerEventData pointerEventData)
    {
        dragging = true;
    }
    
    public void OnEndDrag(PointerEventData pointerEventData)
    {
        dragging = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        dragging = false;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (dragging) return;
        OnSelected?.Invoke();
    }
}