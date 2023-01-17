using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CatchCatHealthBar : MvcBehaviour
{
    [Title("Bar")]
    [SerializeField] private RectTransform barRect;
    [SerializeField] private Image fillImage;
    
    [Title("UI")]
    [SerializeField] private RectTransform heartRect;
    [SerializeField] private RectTransform titleRect;

    public void Init()
    {
        barRect.localScale = Vector2.zero;
        fillImage.fillAmount = 0;
        heartRect.anchoredPosition = new Vector2(heartRect.anchoredPosition.x, 0);
        titleRect.anchoredPosition = new Vector2(titleRect.anchoredPosition.x, 0);
    }
    
    public void Open()
    {
        barRect.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack);
        fillImage.DOFillAmount(1, 0.35f).SetEase(Ease.OutExpo).SetDelay(0.25f);
        heartRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack).SetDelay(0.3125f);
        titleRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack).SetDelay(0.375f);
    }

    public void Close()
    {
        titleRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack);
        heartRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack).SetDelay(0.1f);
        barRect.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack).SetDelay(0.25f);
    }

    public void ChangeBarValue(float value)
    {
        fillImage.DOFillAmount(value, 0.35f).SetEase(Ease.OutExpo);
    }
}
