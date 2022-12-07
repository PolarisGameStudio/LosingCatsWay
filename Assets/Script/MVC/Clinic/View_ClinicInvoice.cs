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

        //總金額
        int total = 0;
        for (int i = 0; i < payment.Count; i++)
        {
            total += payment.ElementAt(i).Value;
        }
        totalText.text = total.ToString();

        if (payment.ContainsKey("CP001"))
        {
            subjects[0].SetActive(true);
        }

        if (payment.ContainsKey("CP002"))
        {
            subjects[1].SetActive(true);
        }

        if (payment.ContainsKey("CP003"))
        {
            subjects[2].SetActive(true);
        }

        if (payment.ContainsKey("CP004"))
        {
            subjects[3].SetActive(true);
        }

        if (payment.ContainsKey("CP005"))
        {
            subjects[4].SetActive(true);
        }

        if (payment.ContainsKey("CP006"))
        {
            subjects[5].SetActive(true);
        }

        if (payment.ContainsKey("CP007"))
        {
            subjects[6].SetActive(true);
        }
    }

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        playerNameText.text = App.system.player.PlayerName;
        catNameText.text = cat.cloudCatData.CatData.CatName;
        dateText.text = System.DateTime.Now.ToString("yyyy-MM-dd");
    }

    public override void Open()
    {
        //重設帳單位置
        scrollRect.enabled = false;
        invoiceRect.DOAnchorPosY(invoice_StartY, 0);

        //關付款按鈕
        for (int i = 0; i < invoiceButtons.Length; i++)
        {
            invoiceButtons[i].SetActive(false);
        }

        DOVirtual.DelayedCall(2f, () =>
        {
            invoiceRect.DOAnchorPosY(invoice_EndY, 4f).OnComplete(() =>
            {
                scrollRect.enabled = true;
                for (int i = 0; i < invoiceButtons.Length; i++)
                {
                    invoiceButtons[i].SetActive(true);
                }
            });
        });

        base.Open();
    }
}
