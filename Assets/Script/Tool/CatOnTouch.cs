using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CatOnTouch : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IDragHandler
{
    public UnityEvent OnCatTouch;
    [Space(20)]

    //public float touchOverTime;

    //float pressedTime;

    bool onTouch;

    private void Start()
    {
        ResetTouch();
    }

    #region EventSystems

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        onTouch = true;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        onTouch = false;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Touch();
    }

    #endregion

    #region TouchEvents

    public void Touch()
    {
        if (!onTouch) return;

        OnCatTouch?.Invoke();
        ResetTouch();
    }

    #endregion

    #region Reset

    void ResetTouch()
    {
        //pressedTime = 0f;
        onTouch = false;
    }

    #endregion
}
