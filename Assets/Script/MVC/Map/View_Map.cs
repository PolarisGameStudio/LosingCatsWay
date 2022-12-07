using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Map : ViewBehaviour
{
    [Title("Parallex")]
    public Scrollbar scrollbarX;
    [Space(20)]

    public RectTransform leftCloud;
    public RectTransform rightCloud;
    [Space(20)]

    [Range(0f, 1f)] public float parallexLeftValue;
    [Range(0f, 1f)] public float parallexRightValue;
    [Range(0f, 1f)] public float parallexFullSize;
    [Space(20)]

    public float leftOffset;
    public float rightOffset;
    [Space(20)]

    public float parallexSpeed;

    [SerializeField, Range(0f, 1f)] private float startScroll;

    [Title("Currency")] [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("DoTween")] [SerializeField] private RectTransform[] mapRects;
    [SerializeField] private CanvasGroup[] mapCanvasGroups;
    [SerializeField] private CanvasGroup[] buttonCanvasGroups;

    private Vector2 leftParallexPos;
    private Vector2 rightParallexPos;

    public override void Init()
    {
        base.Init();

        leftParallexPos = leftCloud.anchoredPosition;
        leftParallexPos.x += leftOffset;

        rightParallexPos = rightCloud.anchoredPosition;
        rightParallexPos.x -= rightOffset;

        leftCloud.anchoredPosition = leftParallexPos;
        rightCloud.anchoredPosition = rightParallexPos;
        
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
        leftCloud.anchoredPosition = leftParallexPos;
        rightCloud.anchoredPosition = rightParallexPos;
        scrollbarX.value = startScroll;
        
        UIView.InstantShow();

        for (int i = 0; i < mapRects.Length; i++)
        {
            mapRects[i].localScale = Vector2.zero;
            mapCanvasGroups[i].alpha = 0;
        }

        for (int i = 0; i < buttonCanvasGroups.Length; i++)
        {
            buttonCanvasGroups[i].alpha = 0;
        }

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
            {
                buttonCanvasGroups[i].DOFade(1, 0.225f).SetEase(Ease.InCubic);
            }
        });
    }

    private void FixedUpdate()
    {
        if (scrollbarX.size >= parallexFullSize)
        {
            leftCloud.anchoredPosition = Vector2.MoveTowards(leftCloud.anchoredPosition, Vector2.zero, parallexSpeed * Time.deltaTime);
            rightCloud.anchoredPosition = Vector2.MoveTowards(rightCloud.anchoredPosition, Vector2.zero, parallexSpeed * Time.deltaTime);
            return;
        }

        //Left
        if (scrollbarX.value <= parallexLeftValue)
        {
            leftCloud.anchoredPosition = Vector2.MoveTowards(leftCloud.anchoredPosition, leftParallexPos, parallexSpeed * Time.deltaTime);
            rightCloud.anchoredPosition = Vector2.MoveTowards(rightCloud.anchoredPosition, Vector2.zero, parallexSpeed * Time.deltaTime);
        }
        //Right
        else if (scrollbarX.value >= parallexRightValue)
        {
            leftCloud.anchoredPosition = Vector2.MoveTowards(leftCloud.anchoredPosition, Vector2.zero, parallexSpeed * Time.deltaTime);
            rightCloud.anchoredPosition = Vector2.MoveTowards(rightCloud.anchoredPosition, rightParallexPos, parallexSpeed * Time.deltaTime);
        }
    }
}
