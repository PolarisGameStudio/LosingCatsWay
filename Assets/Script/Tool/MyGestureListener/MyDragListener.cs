using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MyDragListener : MonoBehaviour, IDragHandler
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent
    {
    }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")] [SerializeField]
    private ButtonClickedEvent m_OnDrag = new ButtonClickedEvent();

    public ButtonClickedEvent onDrag
    {
        get { return m_OnDrag; }
        set { m_OnDrag = value; }
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke();
    }
}