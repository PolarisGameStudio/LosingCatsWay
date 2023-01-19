using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class View_ClinicChooseFunction : ViewBehaviour
{
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Button[] functionButtons;

    private Vector2 panelOrigin = Vector2.zero;
    private Vector2 panelOffset = Vector2.zero;

    public override void Open()
    {
        base.Open();

        for (int i = 0; i < functionButtons.Length; i++)
            functionButtons[i].transform.localScale = Vector2.zero;

        panelOrigin = panelTransform.anchoredPosition;
        panelOffset.x = panelOrigin.x + panelTransform.sizeDelta.x * 2;
        panelOffset.y = panelOrigin.y;
        panelTransform.DOAnchorPos(panelOrigin, 0.4f).From(panelOffset).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                for (int i = 0; i < functionButtons.Length; i++)
                {
                    Button tmp = functionButtons[i];
                    tmp.transform.DOScale(Vector2.one, 0.2f)
                        .OnStart(() => tmp.interactable = false)
                        .SetDelay(i * 0.1f)
                        .OnComplete(() => tmp.interactable = true);
                }
            });
    }

    public override void Close()
    {
        base.Close();
        DOTween.Kill(panelTransform, true);
        for (int i = 0; i < functionButtons.Length; i++)
            DOTween.Kill(functionButtons[i].transform, true);
    }
}
