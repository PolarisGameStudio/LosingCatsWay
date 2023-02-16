using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_CatNotify : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private RectTransform bubbleRect;
    [SerializeField] private RectTransform chatRect;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject redDot;

    [Title("Dotween")]
    [SerializeField] private float popDuration;
    [SerializeField] private Ease openEase;
    [SerializeField] private Ease closeEase;

    private Sequence popSeq;

    [ReadOnly] public Cat notifyCat;
    
    public void SetData(Cat cat)
    {
        CloudCatData cloudCatData = cat.cloudCatData;
        catSkin.ChangeSkin(cloudCatData);
        catSkin.SetActive(true);
        notifyCat = cat;

        text.text = App.factory.stringFactory.GetCatNotify(cat.catNotifyId);
    }
    
    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        popSeq.Kill();
        popSeq = DOTween.Sequence();

        popSeq
            .OnStart(() =>
            {
                redDot.SetActive(false);
                bubbleRect.localScale = Vector3.zero;
                chatRect.localScale = Vector3.zero;
            })
            .Append(bubbleRect.DOScale(Vector3.one, popDuration).From(Vector3.zero).SetEase(openEase))
            .Append(chatRect.DOScale(Vector3.one, popDuration).From(Vector3.zero).SetEase(openEase))
            .AppendInterval(1f)
            .Append(chatRect.DOScale(Vector3.zero, popDuration).From(Vector3.one).SetEase(closeEase))
            .OnComplete(() =>
            {
                redDot.SetActive(true);
            });
    }

    public void Click()
    {
        App.system.soundEffect.Play("Button");
        notifyCat.FollowCat();
    }

    private void OnDestroy()
    {
        popSeq.Kill();
    }
    
    public void CheckRedActivate()
    {
        if (!redDot.activeSelf)
            return;
        redDot.SetActive(false);
        DOVirtual.DelayedCall(0.45f, () => redDot.SetActive(true));
    }
}
