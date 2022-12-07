using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class View_ChooseFloor : ViewBehaviour
{
    [Title("UI")]
    public RectTransform underBar;

    [Title("Selector")]
    public Transform selector;
    public Transform pointer;
    
    [Title("Cards")]
    public CanvasGroup[] selecteds;
    public CanvasGroup[] deselecteds;
    
    public override void Init()
    {
        base.Init();

        App.model.chooseFloor.OnUsingFloorIndexChange += OnUsingFloorIndexChange;
    }

    public void OnUsingFloorIndexChange(object from,object to)
    {
        int fromValue = (int) from;
        int toValue = (int) to;

        var targetPosition = selecteds[toValue].transform.position;

        selector.DOMoveX(targetPosition.x, 0.25f);
        pointer.DOMoveX(targetPosition.x, 0.25f);
        
        selecteds[toValue].DOFade(1, 0.25f).From(0);
        deselecteds[toValue].DOFade(0, 0.25f).From(1);
        
        if (fromValue == -1)
            return;
        
        selecteds[fromValue].DOFade(0, 0.25f).From(1);
        deselecteds[fromValue].DOFade(1, 0.25f).From(0);
    }

    public override void Open()
    {
        UIView.InstantShow();
        underBar.DOAnchorPosY(0, .5f).SetEase(Ease.OutExpo).From(new Vector2(0, -450));
    }

    public override void Close()
    {
        underBar.DOAnchorPosY(-450, .5f).SetEase(Ease.OutExpo).From(new Vector2(0, 0)).OnComplete(() =>
        {
            UIView.InstantHide();
        });
    }
}