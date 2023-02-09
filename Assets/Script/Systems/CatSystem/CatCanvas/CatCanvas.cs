using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CatCanvas : MvcBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private DirectionChecker directionChecker;

    [Title("Money")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private RectTransform moneyPop;

    [Title("Exp")]
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private RectTransform expPop;

    private Vector2 startPos = new(0, .75f);
    private Vector2 endPos = new(0, 2f);

    private Vector2 startScale;
    private Vector2 flipScale;
    
    private void Start()
    {
        directionChecker.OnDirectionChange += CheckDirection;

        startScale = transform.localScale;
        flipScale = new Vector2(-startScale.x, startScale.y);
        
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogWarning("Canvas render mode wasn't WorldSpace.");
            return;
        }
        
        canvas.worldCamera = Camera.main;
    }

    public void ActiveMoney(int value, Callback endAction = null)
    {
        CheckDirection();
        
        moneyText.text = $"+{value}";
        moneyPop.DOScale(Vector2.one, 0.5f).From(Vector2.zero).SetEase(Ease.OutBack);
        moneyPop.DOAnchorPos(endPos, 0.5f).From(startPos);
        moneyPop.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InBack).SetDelay(1)
            .OnComplete(() => endAction?.Invoke());
    }

    public void ActiveExp(int value, Callback endAction = null)
    {
        CheckDirection();
        
        expText.text = $"+{value}";
        expPop.DOScale(Vector2.one, 0.5f).From(Vector2.zero).SetEase(Ease.OutBack);
        expPop.DOAnchorPos(endPos, 0.5f).From(startPos);
        expPop.DOScale(Vector2.zero, 0.5f).SetEase(Ease.InBack).SetDelay(1)
            .OnComplete(() => endAction?.Invoke());
    }

    private void CheckDirection()
    {
        float parentX = transform.parent.localScale.x;
        if (parentX >= 0)
        {
            transform.localScale = startScale;
            return;
        }
        transform.localScale = flipScale;
    }
}
