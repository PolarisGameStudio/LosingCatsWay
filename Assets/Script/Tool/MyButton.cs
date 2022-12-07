using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isActive = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isActive = true;
        m_onButtonDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isActive = false;
        m_onButtonUp.Invoke();
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        m_onLongPress.Invoke();
    }

    [Serializable]
    public class ButtonLongPressEvent : UnityEvent
    {
    }

    [FormerlySerializedAs("onLongPress")] [SerializeField]
    private ButtonLongPressEvent m_onLongPress = new ButtonLongPressEvent();

    public ButtonLongPressEvent onLongPress
    {
        get { return m_onLongPress; }
        set { m_onLongPress = value; }
    }

    [Serializable]
    public class ButtonUpPressEvent : UnityEvent
    {
    }

    [FormerlySerializedAs("onButtonUp")] [SerializeField]
    private ButtonUpPressEvent m_onButtonUp = new ButtonUpPressEvent();

    public ButtonUpPressEvent onButtonUp
    {
        get { return m_onButtonUp; }
        set { m_onButtonUp = value; }
    }

    [Serializable]
    public class ButtonDownPressEvent : UnityEvent
    {
    }

    [FormerlySerializedAs("onButtonDown")] [SerializeField]
    private ButtonDownPressEvent m_onButtonDown = new ButtonDownPressEvent();

    public ButtonDownPressEvent onButtonDown
    {
        get { return m_onButtonDown; }
        set { m_onButtonDown = value; }
    }
}