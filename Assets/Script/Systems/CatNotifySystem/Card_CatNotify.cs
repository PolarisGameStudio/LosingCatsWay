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

    public Callback OnClick;

    private Sequence popSeq;

    [ReadOnly] public bool isOpen;
    
    [Button(30)]
    public void Open(Cat cat)
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        isOpen = true;

        CloudCatData cloudCatData = cat.cloudCatData;
        catSkin.ChangeSkin(cloudCatData);
        catSkin.SetActive(true);

        text.text = App.factory.stringFactory.GetCatNotify(cat.catNotifyId);

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

    [Button(30)]
    public void Close()
    {
        gameObject.SetActive(false);
        isOpen = false;
        // catSkin.SetActive(false);
        //
        // popSeq.Kill();
        // popSeq = DOTween.Sequence();
        //
        // popSeq
        //     .OnStart(() =>
        //     {
        //         redDot.SetActive(false);
        //         bubbleRect.localScale = Vector3.one;
        //         chatRect.localScale = Vector3.one;
        //     })
        //     .Append(bubbleRect.DOScale(Vector3.zero, popDuration).From(Vector3.one).SetEase(closeEase))
        //     .OnComplete(() =>
        //     {
        //         gameObject.SetActive(false);
        //         App.system.catNotify.PopUp();
        //     });
    }

    public void Click()
    {
        OnClick?.Invoke();
        OnClick = null;
        Close();
        App.system.soundEffect.Play("Button");
    }
}
