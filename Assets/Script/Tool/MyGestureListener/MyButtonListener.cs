using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MyButtonListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool interactable;
    public bool useDown;
    public bool useUp;
    
    [ShowIf("useDown")] public UnityEvent OnPressed;
    [ShowIf("useUp")] public UnityEvent OnRelease;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!useDown) return;
        if (!interactable) return;
        OnPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!useUp) return;
        if (!interactable) return;
        OnRelease?.Invoke();
    }
}
