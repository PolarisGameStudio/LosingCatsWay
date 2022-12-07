using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnDrag2D : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private UnityEvent DragBegin;
    [SerializeField] private UnityEvent Drag;
    [SerializeField] private UnityEvent DragEnd;

    public void OnDrag(PointerEventData pointerEventData)
    {
        Drag?.Invoke();
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        DragBegin?.Invoke();
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        DragEnd?.Invoke();
    }
}
