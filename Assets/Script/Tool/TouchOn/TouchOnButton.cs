using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchOnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //public bool interactable = true;
    [Space(10)]

    public Image targetGraphic;
    //public Color32 enabledColor = Color.white;
    //public Color32 disabledColor = Color.white;
    [Space(10)]

    public Sprite onDownSprite;
    public Sprite onUpSprite;
    public Color32 onDownColor = Color.white;
    public Color32 onUpColor = Color.white;
    [Space(10)]

    public UnityEvent OnDown;
    public UnityEvent OnUp;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //if (!interactable) return;

        if (onDownSprite != null)
        {
            targetGraphic.sprite = onDownSprite;
            targetGraphic.color = onDownColor;
        }

        OnDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //if (!interactable) return;

        if (onUpSprite != null)
        {
            targetGraphic.sprite = onUpSprite;
            targetGraphic.color = onUpColor;
        }
        OnUp?.Invoke();
    }
}
