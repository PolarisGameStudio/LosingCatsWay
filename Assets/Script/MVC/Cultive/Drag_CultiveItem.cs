using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag_CultiveItem : MvcBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [HideInInspector] public Image icon;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public Item item;

    Vector3 startPos;

    private void Start()
    {
        canvasGroup.alpha = 0;
        startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!App.controller.cultive.isCanDrag)
            return;

        App.controller.cultive.CloseClickCat();
        App.controller.cultive.OpenDropSensor();

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        App.controller.cultive.DragItem(item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!App.controller.cultive.isCanDrag)
            return;
        
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        App.controller.cultive.OpenDropSensor();

        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        App.controller.cultive.CloseDropSensor();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = startPos;
    }
}
