using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatRewardCanvas : MvcBehaviour
{
    [Title("UI")] [SerializeField] private Sprite coinSprite;
    [SerializeField] private Sprite expSprite;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private Vector2 originScale;
    private Quaternion faceLeft, faceRight;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        canvasGroup.alpha = 0;
        originScale = transform.localScale;

        faceLeft = new Quaternion(0, 180, 0, 0);
        faceRight = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        FixRotation();
    }

    private void FixRotation()
    {
        if (transform.parent.localScale.x < 0)
            rectTransform.rotation = faceLeft;
        else
            rectTransform.rotation = faceRight;
    }

    public void PopReward(int exp, int coin)
    {
        App.system.player.AddExp(exp);
        App.system.player.AddMoney(coin);
        
        rewardIcon.sprite = expSprite;
        rewardText.text = $"+{exp}";
        Open();

        DOVirtual.DelayedCall(1.5f, () =>
        {
            rewardIcon.sprite = coinSprite;
            rewardText.text = $"+{coin}";
            Open();
        });
    }

    private void Open()
    {
        rectTransform.DOScale(originScale, 0.5f).From(Vector2.zero);
        rectTransform.DOLocalMoveY(3.5f, 0.5f).From(1);
        canvasGroup.DOFade(1, 0.5f).From(0);

        DOVirtual.DelayedCall(0.75f, Close);
    }

    private void Close()
    {
        rectTransform.DOScale(Vector2.zero, 0.5f);
        rectTransform.DOLocalMoveY(1, 0.5f);
        canvasGroup.DOFade(0, 0.5f);
    }
}
