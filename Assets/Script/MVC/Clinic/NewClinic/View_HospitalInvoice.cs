using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_HospitalInvoice : ViewBehaviour
{
    [Title("Invoice")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private GameObject[] subjects;
    [SerializeField] private GameObject wormSubject;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI myMoneyText;
    
    [Title("Tween")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform invoiceRect;
    [SerializeField] private GameObject[] invoiceButtons;
    [SerializeField] private float invoice_StartY = -913f;
    [SerializeField] private float invoice_EndY = -148f;
    [SerializeField] private Transform vetIconTransform;
    [SerializeField] private TextMeshProUGUI[] subjectsTexts;
    
    public override void Open()
    {
        ResetInvoiceTween();
        PlayInvoiceTween();
        base.Open();
    }

    public override void Init()
    {
        base.Init();
        //Player
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnPlayerNameChange += OnPlayerNameChange;
        
        //Hospital
        App.model.hospital.OnIsCatHasWormChange += OnIsCatHasWormChange;
        App.model.hospital.OnFunctionIndexChange += OnFunctionIndexChange;
        App.model.hospital.OnSelectedCatChange += OnSelectedCatChange;
    }

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        catNameText.text = cat.cloudCatData.CatData.CatName;
    }

    private void OnPlayerNameChange(object value)
    {
        string playerName = value.ToString();
        playerNameText.text = playerName;
    }

    private void OnFunctionIndexChange(object value)
    {
        int index = (int)value;
        for (int i = 0; i < subjects.Length; i++)
            subjects[i].SetActive(i == index);
    }

    private void OnIsCatHasWormChange(object value)
    {
        bool hasWorm = (bool)value;
        wormSubject.SetActive(hasWorm);
    }

    private void OnCoinChange(object value)
    {
        int money = (int)value;
        myMoneyText.text = money.ToString();
    }

    private void ResetInvoiceTween()
    {
        scrollRect.enabled = false;
        invoiceRect.DOAnchorPosY(invoice_StartY, 0);
        for (int i = 0; i < invoiceButtons.Length; i++)
            invoiceButtons[i].SetActive(false);
    }

    private void PlayInvoiceTween()
    {
        invoiceRect.DOAnchorPosY(invoice_EndY, 4f).SetDelay(1.5f).OnComplete(() =>
        {
            scrollRect.enabled = true;
            ActiveInvoiceButtons();
        });

        vetIconTransform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetDelay(2f).SetEase(Ease.OutBack);
        playerNameText.DOFade(1, 0.15f).From(0).SetDelay(2.5f).SetEase(Ease.OutSine);
        catNameText.DOFade(1, 0.15f).From(0).SetDelay(3f).SetEase(Ease.OutSine);
        dateText.DOFade(1, 0.15f).From(0).SetDelay(3.5f).SetEase(Ease.OutSine);
        totalText.DOFade(1, 0.15f).From(0).SetDelay(4.15f).SetEase(Ease.OutSine);
        myMoneyText.DOFade(1, 0.15f).From(0).SetDelay(4.5f).SetEase(Ease.OutSine);

        for (int i = 0; i < subjectsTexts.Length; i++)
            subjectsTexts[i].DOFade(1, 0.15f).From(0).SetDelay(3.75f).SetEase(Ease.OutSine);
    }

    public void SkipTween()
    {
        DOTween.Kill(invoiceRect, true);
        DOTween.Kill(vetIconTransform, true);
        DOTween.Kill(playerNameText, true);
        DOTween.Kill(catNameText, true);
        DOTween.Kill(dateText, true);
        DOTween.Kill(totalText, true);
        DOTween.Kill(myMoneyText, true);

        for (int i = 0; i < subjectsTexts.Length; i++)
            DOTween.Kill(subjectsTexts[i], true);
    }

    private void ActiveInvoiceButtons()
    {
        for (int i = 0; i < invoiceButtons.Length; i++)
        {
            invoiceButtons[i].SetActive(true);
            invoiceButtons[i].transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetDelay(i * 0.1f);
        }
    }
}
