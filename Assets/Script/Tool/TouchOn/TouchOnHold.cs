using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchOnHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool interactable = true;
    public bool useFixedUpdate = false;
    [Space(10)]

    public Image targetGraphic;
    public Color32 enabledColor = Color.white;
    public Color32 disabledColor = Color.white;
    [Space(10)]

    public UnityEvent OnHoldDown;

    bool OnHold = false;

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

    void Start()
    {
        OnHold = false; //�קK�۰ʶ}�l

        InteractableChecker();
    }

    void Update()
    {
        InteractableChecker();

        if (useFixedUpdate) return;

        if (OnHold) OnHoldDown?.Invoke();
    }

    void FixedUpdate()
    {
        InteractableChecker();

        if (!useFixedUpdate) return;

        if (OnHold) OnHoldDown?.Invoke();
    }

    private void OnDisable()
    {
        OnHold = false;
    }

    #region Logic

    void InteractableChecker()
    {
        if (interactable) targetGraphic.color = enabledColor;
        else targetGraphic.color = disabledColor;
    }

    #endregion
}
