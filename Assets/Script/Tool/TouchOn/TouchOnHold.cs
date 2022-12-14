using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchOnHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool interactable = true;
    public bool useFixedUpdate;
    
    public UnityEvent OnHoldDown;

    private bool OnHold;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!interactable) return;
        OnHold = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!interactable) return;
        OnHold = false;
    }

    private void Start()
    {
        OnHold = false;
    }

    private void Update()
    {
        if (useFixedUpdate) return;
        if (OnHold) OnHoldDown?.Invoke();
    }

    private void FixedUpdate()
    {
        if (!useFixedUpdate) return;
        if (OnHold) OnHoldDown?.Invoke();
    }

    private void OnDisable()
    {
        OnHold = false;
    }
}
