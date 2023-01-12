using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Map : ViewBehaviour
{
    public Scrollbar scrollbarX;
    [SerializeField, Range(0f, 1f)] private float startScroll;

    [Title("Currency")] [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("DoTween")] [SerializeField] private RectTransform[] mapRects;
    [SerializeField] private CanvasGroup[] mapCanvasGroups;
    [SerializeField] private CanvasGroup[] buttonCanvasGroups;

    public override void Init()
    {
        base.Init();

        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnDiamondChange(object value)
    {
        int diamond = (int)value;
        diamondText.text = diamond.ToString();
    }

    private void OnCoinChange(object value)
    {
        int coin = (int)value;
        coinText.text = coin.ToString();
    }

    public override void Open()
    {
        scrollbarX.value = startScroll;
        
        UIView.InstantShow();

        for (int i = 0; i < mapRects.Length; i++)
        {
            mapRects[i].localScale = Vector2.zero;
            mapCanvasGroups[i].alpha = 0;
        }

        for (int i = 0; i < buttonCanvasGroups.Length; i++)
            buttonCanvasGroups[i].alpha = 0;

        DOVirtual.DelayedCall(0.2f, () =>
        {
            for (int i = 0; i < mapRects.Length; i++)
            {
                mapRects[i].DOScale(Vector2.one, 0.225f).From(Vector2.zero).SetEase(Ease.OutCirc).SetDelay(0.08f * i);
                mapCanvasGroups[i].DOFade(1, 0.225f).From(0).SetEase(Ease.InCubic).SetDelay(0.08f * i);
            }
        });

        DOVirtual.DelayedCall(0.7f, () =>
        {
            for (int i = 0; i < buttonCanvasGroups.Length; i++)
                buttonCanvasGroups[i].DOFade(1, 0.225f).SetEase(Ease.InCubic);
        });
    }
}
