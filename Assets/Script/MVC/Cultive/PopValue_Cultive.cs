using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopValue_Cultive : MvcBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private CanvasGroup canvasGroup;

    Vector3 startPos;
    DG.Tweening.Sequence seq;

    private void Start()
    {
        startPos = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0;
    }

    public void Pop(int value)
    {
        if (value <= 0) return;

        valueText.text = $"+{value}";

        if (seq != null) seq.Kill();
        seq = DOTween.Sequence();

        seq
            .Append(canvasGroup.DOFade(1, 0.5f).From(0))
            .Join(rectTransform.DOAnchorPosY(startPos.y + 50, 1f).From(startPos).SetEase(Ease.OutExpo))
            .Append(canvasGroup.DOFade(0, 0.5f).SetEase(Ease.OutExpo))
            .SetAutoKill(true);
    }
}
