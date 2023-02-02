using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_ClinicResult : ViewBehaviour
{
    [Title("Top")]
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image moodImage;
    [SerializeField] private GameObject[] genderObjects;
    [SerializeField] private TextMeshProUGUI ageText;
    [SerializeField] private TextMeshProUGUI ageLevelText;
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private TextMeshProUGUI idText;
    
    [Title("Down")]
    //[SerializeField] private GameObject[] healthObjects;
    [SerializeField] private TextMeshProUGUI sickNameText;
    [SerializeField] private GameObject[] sickLevels;
    [SerializeField] private TextMeshProUGUI sickInfoText;
    [SerializeField] private TextMeshProUGUI metCountText;
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private Image sickInfoImage;

    [Title("MetCount")] [SerializeField] private GameObject metCountObject;
    [SerializeField] private GameObject cantMetObject;

    [Title("Health")] [SerializeField] private GameObject[] healthTexts;

    [Title("CantHeal")] [SerializeField] private GameObject signObject;
    [SerializeField] private GameObject deadSignObject;

    [Title("Tween")] [SerializeField] private RectTransform topLeftRect;
    [SerializeField] private RectTransform nameRect;
    [SerializeField] private RectTransform infoRect;
    [SerializeField] private RectTransform botRect;
    [SerializeField] private RectTransform okRect;
    [SerializeField] private RectTransform panelRect;
    
    private Queue<string> resultIds = new Queue<string>();
    private Cat cat;
    private string _sickId;
    
    private Vector2 topLeftOrigin = Vector2.zero;
    private Vector2 topLeftOffset = Vector2.zero;
    private Vector2 botOrigin = Vector2.zero;
    private Vector2 botOffset = Vector2.zero;


    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);
        panelRect.localScale = Vector2.zero;

        if (cat.cloudCatData.CatHealthData.SickId is "SK001" or "SK002")
        {
            resultIds.Enqueue("CP001");
            App.controller.clinic.DoFunction(); // 設定絕症已看診，下次才會顯示不可治療
        }
        
        ReadResult();
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
    }

    public override void Init()
    {
        base.Init();
        App.model.clinic.OnSelectedCatChange += OnSelectedCatChange;
        App.model.clinic.OnPaymentChange += OnPaymentChange;
        App.model.clinic.OnSickIdChange += OnSickIdChange;
        App.model.clinic.OnMetCountChange += OnMetCountChange;
    }

    private void OnMetCountChange(object value)
    {
        int count = (int)value;
        ChangeMetCount(count);
    }

    private void OnSickIdChange(object value)
    {
        string sickId = value.ToString();
        _sickId = sickId;

        if (string.IsNullOrEmpty(sickId))
            return;
        
        ChangeSickContent(sickId);
    }

    private void OnSelectedCatChange(object value)
    {
        cat = (Cat)value;

        catSkin.ChangeSkin(cat.cloudCatData);

        nameText.text = cat.cloudCatData.CatData.CatName;

        int mood = CatExtension.GetCatMood(cat.cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        for (int i = 0; i < genderObjects.Length; i++)
        {
            if (i == cat.cloudCatData.CatData.Sex)
                genderObjects[i].SetActive(true);
            else
                genderObjects[i].SetActive(false);
        }

        ageText.text = cat.cloudCatData.CatData.CatAge.ToString();
        ageLevelText.text = App.factory.stringFactory.GetAgeLevelString(cat.cloudCatData.CatData.SurviveDays);
        
        sizeText.text = $"{CatExtension.GetCatRealSize(cat.cloudCatData.CatData.BodyScale):0.00}cm";
        idText.text = $"ID:{cat.cloudCatData.CatData.CatId}";

        for (int i = 0; i < healthTexts.Length; i++)
            healthTexts[i].SetActive(false);

        int healthStatus;
        if (string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId))
            healthStatus = 0;
        else if (cat.cloudCatData.CatHealthData.SickId is "SK001" or "SK002")
            healthStatus = 2;
        else
            healthStatus = 1;
        
        healthTexts[healthStatus].SetActive(true);
    }

    private void OnPaymentChange(object value)
    {
        var payment = (Dictionary<string, int>)value;
        resultIds.Clear();
        for (int i = 0; i < payment.Count; i++)
            resultIds.Enqueue(payment.ElementAt(i).Key);
    }

    public void ReadResult()
    {
        if (resultIds.Count > 0)
        {
            if (string.IsNullOrEmpty(_sickId))
            {
                resultIds = new Queue<string>(resultIds.Where(x => x != "CP001")); //客製化病狀單
                FadeContent();
            }
            else
            {
                FadeContent();
            }
        }
        else
        {
            App.controller.clinic.CloseCheckResult();
            App.controller.clinic.OpenChooseFunction();
        }
    }

    private void FadeContent()
    {
        contentGroup.DOFade(0, 0.25f).From(1);
        DOVirtual.DelayedCall(0.25f, ChangeContent);
        contentGroup.DOFade(1, 0.25f).From(0).SetDelay(.5f);
    }

    // 病狀單內容
    private void ChangeContent()
    {
        if (resultIds.Count <= 0)
            return;
        
        TweenIn();
        string id = resultIds.Dequeue();

        if (id == "CP001")
        {
            ChangeSickContent(_sickId);
            ChangeMetCount(cat.cloudCatData.CatHealthData.MetDoctorCount);
            return;
        }

        sickNameText.text = App.factory.stringFactory.GetPaymentName(id);
        sickInfoText.text = App.factory.stringFactory.GetPaymentInfo(id);
        metCountText.text = "-";
        sickInfoImage.sprite = App.factory.sickFactory.GetClinicSprite(id);
        
        for (int i = 0; i < sickLevels.Length; i++)
            sickLevels[i].SetActive(i == 3);
    }

    private void ChangeSickContent(string sickId)
    {
        sickNameText.text = App.factory.stringFactory.GetSickName(sickId);
        sickInfoText.text = App.factory.stringFactory.GetSickInfo(sickId);
        sickInfoImage.sprite = App.factory.sickFactory.GetSickSprite(sickId);

        for (int i = 0; i < sickLevels.Length; i++)
        {
            int sickLevel = App.factory.sickFactory.GetSickLevel(sickId);
            if (i == sickLevel)
                sickLevels[i].SetActive(true);
            else
                sickLevels[i].SetActive(false);
        }

        if (sickId is "SK001" or "SK002")
        {
            metCountObject.SetActive(false);
            cantMetObject.SetActive(true);
            signObject.SetActive(false);
            deadSignObject.SetActive(true);
        }
        else
        {  
            metCountObject.SetActive(true);
            signObject.SetActive(true);
            deadSignObject.SetActive(false);
        }
        
        catSkin.ChangeSkin(cat.cloudCatData);
    }

    private void ChangeMetCount(int count)
    {
        metCountText.text = count.ToString();
    }

    private void TweenIn()
    {
        panelRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero);
        
        topLeftOrigin = topLeftRect.anchoredPosition;
        topLeftOffset.x = topLeftOrigin.x - topLeftRect.sizeDelta.x;
        topLeftOffset.y = topLeftOrigin.y;
        topLeftRect.DOAnchorPos(topLeftOrigin, 0.25f).From(topLeftOffset).SetDelay(0.25f);

        nameRect.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        moodImage.transform.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        infoRect.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.3f);

        botOrigin = botRect.anchoredPosition;
        botOffset.x = botOrigin.x;
        botOffset.y = botOrigin.y - botRect.sizeDelta.y;
        botRect.DOAnchorPos(botOrigin, 0.25f).From(botOffset).SetEase(Ease.OutExpo).SetDelay(0.35f);

        signObject.transform.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        deadSignObject.transform.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        
        okRect.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.4f);
    }
}
