using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyDragListener : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Serializable] public class ButtonClickedEvent : UnityEvent{}

    [Title("Usage")] [SerializeField] private bool useBeginDrag;
    [SerializeField] private bool useEndDrag;
    
    // Event delegates triggered on click.
    [SerializeField, PropertySpace(5)]
    private ButtonClickedEvent m_OnDrag = new ButtonClickedEvent();
    [SerializeField, PropertySpace(5), ShowIf("useBeginDrag")] private ButtonClickedEvent m_OnBeginDrag = new ButtonClickedEvent();
    [SerializeField, PropertySpace(5), ShowIf("useEndDrag")] private ButtonClickedEvent m_OnEndDrag = new ButtonClickedEvent();

    public ButtonClickedEvent onDrag => m_OnDrag;
    public ButtonClickedEvent onBeginDrag => m_OnBeginDrag;
    public ButtonClickedEvent onEndDrag => m_OnEndDrag;

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!useBeginDrag) return;
        onBeginDrag?.Invoke();
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!useEndDrag) return;
        onEndDrag?.Invoke();
    }
}