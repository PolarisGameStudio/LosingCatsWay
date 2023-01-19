using DG.Tweening;
using Sirenix.OdinInspector;
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

    [Title("Tween/InvoiceScroll")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform invoiceRect;
    [SerializeField] private GameObject[] invoiceButtons;
    [SerializeField] private float invoice_StartY;
    [SerializeField] private float invoice_EndY;

    [Title("Tween/InvoiceContent")]
    [SerializeField] private Transform vetIconTransform;
    [SerializeField] private TextMeshProUGUI[] subjectsTexts;

    public override void Open()
    {
        scrollRect.enabled = false;
        invoiceRect.DOAnchorPosY(invoice_StartY, 0);

        for (int i = 0; i < invoiceButtons.Length; i++)
            invoiceButtons[i].SetActive(false);
        
        ownCoinText.text = App.system.player.Coin.ToString();
        
        InvoiceAnimation();
        
        base.Open();
    }

    public override void Init()
    {
        base.Init();
        App.system.player.OnPlayerNameChange += OnPlayerNameChange;
        
        App.model.clinic.OnPaymentChange += OnPaymentChange;
        App.model.clinic.OnSelectedCatChange += OnSelectedCatChange;
    }

    private void OnPlayerNameChange(object value)
    {
        playerNameText.text = (string)value;
    }

    private void OnPaymentChange(object value)
    {
        var payment = (Dictionary<string, int>)value;

        for (int i = 0; i < subjects.Length; i++)
        {
            subjects[i].SetActive(false);
        }

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
        catNameText.text = cat.cloudCatData.CatData.CatName;
        dateText.text = App.system.myTime.MyTimeNow.ToString("yyyy-MM-dd");
    }

    private void InvoiceAnimation()
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
        ownCoinText.DOFade(1, 0.15f).From(0).SetDelay(4.5f).SetEase(Ease.OutSine);

        for (int i = 0; i < subjectsTexts.Length; i++)
            subjectsTexts[i].DOFade(1, 0.15f).From(0).SetDelay(3.75f).SetEase(Ease.OutSine);
    }

    public void Skip()
    {
        DOTween.Kill(invoiceRect, true);
        DOTween.Kill(vetIconTransform, true);
        DOTween.Kill(playerNameText, true);
        DOTween.Kill(catNameText, true);
        DOTween.Kill(dateText, true);
        DOTween.Kill(totalText, true);
        DOTween.Kill(ownCoinText, true);

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
