using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_ClinicInvoice : ViewBehaviour
{
    [Title("InvoiceContent")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private GameObject[] subjects;
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI ownCoinText;

    [Title("Tween")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform invoiceRect;
    [SerializeField] private GameObject[] invoiceButtons;
    [SerializeField] private float invoice_StartY;
    [SerializeField] private float invoice_EndY;

    public override void Init()
    {
        base.Init();
        App.model.clinic.OnPaymentChange += OnPaymentChange;
        App.model.clinic.OnSelectedCatChange += OnSelectedCatChange;
    }

    private void OnPaymentChange(object value)
    {
        var payment = (Dictionary<string, int>)value;

        for (int i = 0; i < subjects.Length; i++)
        {
            subjects[i].SetActive(false);
        }

        ownCoinText.text = App.system.player.Coin.ToString();

        int total = 0;
        for (int i = 0; i < payment.Count; i++)
        {
            total += payment.ElementAt(i).Value;
        }
        totalText.text = total.ToString();

        subjects[0].SetActive(payment.ContainsKey("CP001"));
        subjects[1].SetActive(payment.ContainsKey("CP002"));
        subjects[2].SetActive(payment.ContainsKey("CP003"));
        subjects[3].SetActive(payment.ContainsKey("CP004"));
        subjects[4].SetActive(payment.ContainsKey("CP005"));
        subjects[5].SetActive(payment.ContainsKey("CP006"));
        subjects[6].SetActive(payment.ContainsKey("CP007"));
    }

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        playerNameText.text = App.system.player.PlayerName;
        catNameText.text = cat.cloudCatData.CatData.CatName;
        dateText.text = App.system.myTime.MyTimeNow.ToString("yyyy-MM-dd");
    }

    public override void Open()
    {
        scrollRect.enabled = false;
        invoiceRect.DOAnchorPosY(invoice_StartY, 0);

        for (int i = 0; i < invoiceButtons.Length; i++)
            invoiceButtons[i].SetActive(false);

        Invoke(nameof(InvoiceAnimation), 2f);
        base.Open();
    }

    private void InvoiceAnimation()
    {
        invoiceRect.DOAnchorPosY(invoice_EndY, 4f).OnComplete(() =>
        {
            scrollRect.enabled = true;
            for (int i = 0; i < invoiceButtons.Length; i++)
                invoiceButtons[i].SetActive(true);
        });
    }

    public void Skip()
    {
        CancelInvoke(nameof(InvoiceAnimation));
        invoiceRect.DOKill();
        invoiceRect.DOAnchorPosY(invoice_EndY, 0);
        scrollRect.enabled = true;
        for (int i = 0; i < invoiceButtons.Length; i++)
            invoiceButtons[i].SetActive(true);
    }
}
