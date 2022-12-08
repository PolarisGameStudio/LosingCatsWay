using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Card_PediaType : MvcBehaviour
{
    [Title("DoTween")] [SerializeField] private RectTransform maskRect;
    [SerializeField] private CanvasGroup maskCanvasGroup;

    public void SetSelect(bool value)
    {
        if (value)
        {
            Vector2 origin = maskRect.anchoredPosition;
            Vector2 offset = new Vector2(origin.x + 60f, origin.y);
            maskRect.DOAnchorPos(origin, 0.25f).From(offset).SetEase(Ease.OutBack);

            maskCanvasGroup.DOFade(1, 0.25f).From(0).SetEase(Ease.OutExpo);
        }
        else
            maskCanvasGroup.DOFade(0, 0.25f);
    }
}
