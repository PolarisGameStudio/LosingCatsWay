using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

public class CatLosingSystem : MvcBehaviour
{
    [SerializeField] private UIView _uiView;

    [Title("DoTween")] [SerializeField] private CanvasGroup letterCanvasGroup;
    [SerializeField] private RectTransform letterRect;
    [SerializeField] private CanvasGroup contentCanvasGroup;
    [SerializeField] private RectTransform paperRect;
    [SerializeField] private CanvasGroup paperCanvasGroup;

    [Button]
    public void Open()
    {
        //Letter
        letterCanvasGroup.alpha = 0;
        Vector2 letterOrigin = letterRect.anchoredPosition;
        Vector2 letterOffset = letterRect.anchoredPosition;
        letterOffset.x += 200f;
        
        //Paper
        paperCanvasGroup.alpha = 0;
        Vector2 paperOrigin = paperRect.anchoredPosition;
        Vector2 paperOffset = paperRect.anchoredPosition;
        paperOffset.y -= 200f;
        
        //Content
        contentCanvasGroup.alpha = 0;
        
        _uiView.Show();

        letterCanvasGroup.DOFade(1, 0.45f).From(0).SetEase(Ease.InQuart);
        letterRect.DOAnchorPos(letterOrigin, 0.45f).From(letterOffset).SetEase(Ease.OutExpo);
        letterRect.DORotate(Vector3.zero, 0.45f).From(new Vector3(0, 0, -45f)).SetEase(Ease.OutExpo);
        
        paperCanvasGroup.DOFade(1, 0.5f).From(0).SetDelay(0.5f);
        paperRect.DOAnchorPos(paperOrigin, 0.5f).From(paperOffset).SetEase(Ease.OutExpo).SetDelay(0.5f);

        contentCanvasGroup.DOFade(1, 0.35f).From(0).SetDelay(1f);
    }

    [Button]
    public void Close()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            _uiView.InstantHide();
        });
    }
}
