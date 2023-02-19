using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_HospitalDoctorResult : ViewBehaviour
{
    [Title("CatInfo")]
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private Image moodImage;
    [SerializeField] private GameObject[] genderObjects;
    [SerializeField] private TextMeshProUGUI ageText;
    [SerializeField] private TextMeshProUGUI ageLevelText;
    [SerializeField] private TextMeshProUGUI sizeText;
    [SerializeField] private TextMeshProUGUI idText;

    [Title("SickInfo")]
    [SerializeField] private GameObject[] healthTextObjects;
    [SerializeField] private TextMeshProUGUI sickNameText;
    [SerializeField] private GameObject[] sickLevels;
    [SerializeField] private TextMeshProUGUI sickInfoText;
    [SerializeField] private TextMeshProUGUI metCountText;
    [SerializeField] private Image sickInfoImage;

    [Title("BotLeft")]
    [SerializeField] private GameObject solutionObject; // 治療方法
    [SerializeField] private GameObject noSolutionObject; // 無法治療

    [Title("Stamp")]
    [SerializeField] private GameObject vetStamp;
    [SerializeField] private GameObject deadStamp;
    
    [Title("Tween")]
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private RectTransform topLeftRect;
    [SerializeField] private RectTransform nameRect;
    [SerializeField] private RectTransform infoRect;
    [SerializeField] private RectTransform botRect;
    [SerializeField] private RectTransform okRect;
    [SerializeField] private RectTransform panelRect;

    private Queue<string> infoStrings = new Queue<string>();
    
    // Tween
    private Vector2 topLeftOrigin = Vector2.zero;
    private Vector2 topLeftOffset = Vector2.zero;
    private Vector2 botOrigin = Vector2.zero;
    private Vector2 botOffset = Vector2.zero;

    private int tmpFunctionIndex;

    public override void Init()
    {
        base.Init();
        App.model.hospital.OnFunctionIndexChange += OnFunctionIndexChange;
        App.model.hospital.OnSelectedCatChange += OnSelectedCatChange;
        App.model.hospital.OnTmpCatChange += OnTmpCatChange;
        App.model.hospital.OnIsCatHasWormChange += OnIsCatHasWormChange;
    }

    private void OnTmpCatChange(object value)
    {
        Cat cat = (Cat)value;

        if (tmpFunctionIndex != 0)
            return;
        
        int healthStatus;
        string sickId = cat.cloudCatData.CatHealthData.SickId;
        
        if (string.IsNullOrEmpty(sickId)) // 只有除蟲的話就會有健康的情況
            healthStatus = 0;
        else if (sickId is "SK001" or "SK002") // 不治
            healthStatus = 2;
        else //一般生病
            healthStatus = 1;

        // 圖文介紹
        if (healthStatus != 0)
            infoStrings.Enqueue(sickId);
    }

    private void OnIsCatHasWormChange(object value)
    {
        bool hasWorm = (bool)value;
        if (tmpFunctionIndex != 0)
            return;
        if (!hasWorm)
            return;
        infoStrings.Enqueue("Deworm"); // 除蟲
    }

    private void OnSelectedCatChange(object value) // 只會ValueChange在ChooseCat的時候的狀態 治療後的不會ValueChange
    {
        Cat cat = (Cat)value;
        catSkin.ChangeSkin(cat.cloudCatData);
        catNameText.text = cat.cloudCatData.CatData.CatName;
        
        int mood = CatExtension.GetCatMood(cat.cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        for (int i = 0; i < genderObjects.Length; i++)
            genderObjects[i].SetActive(i == cat.cloudCatData.CatData.Sex);
        
        ageText.text = cat.cloudCatData.CatData.CatAge.ToString();
        ageLevelText.text = App.factory.stringFactory.GetAgeLevelString(cat.cloudCatData.CatData.SurviveDays);
        
        sizeText.text = $"{CatExtension.GetCatRealSize(cat.cloudCatData.CatData.BodyScale):0.00}cm";
        idText.text = $"ID:{cat.cloudCatData.CatData.CatId}";

        int healthStatus;
        string sickId = cat.cloudCatData.CatHealthData.SickId;
        
        if (string.IsNullOrEmpty(sickId)) // 只有除蟲的話就會有健康的情況
            healthStatus = 0;
        else if (sickId is "SK001" or "SK002") // 不治
            healthStatus = 2;
        else //一般生病
            healthStatus = 1;

        for (int i = 0; i < healthTextObjects.Length; i++)
            healthTextObjects[i].SetActive(i == healthStatus);
        
        metCountText.text = cat.cloudCatData.CatHealthData.MetDoctorCount.ToString();
    }

    private void OnFunctionIndexChange(object value)
    {
        int index = (int)value;

        tmpFunctionIndex = index;

        if (index == 0) // 看診
            return;
        
        string id = string.Empty;
        switch (index)
        {
            case 1:
                id = "Vaccine"; //疫苗
                break;
            case 2:
                id = "AntiWorm"; //預防
                break;
            case 3:
                id = "Chip"; //晶片
                break;
            case 4:
                id = "Ligation"; //絕育
                break;
        }

        infoStrings.Clear();
        infoStrings.Enqueue(id);
    }

    public override void Open()
    {
        base.Open();
        panelRect.localScale = Vector2.zero;
        catSkin.SetActive(true);
        NextDoctorResult();
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
    }

    public void NextDoctorResult()
    {
        if (infoStrings.Count <= 0)
        {
            App.controller.hospital.CloseDoctorResult();
            App.controller.hospital.OpenChooseFunction();
            
            if(App.system.tutorial.isTutorial)
                App.system.tutorial.Next();
            return;
        }
        
        CloseContent();
        DOVirtual.DelayedCall(0.25f, () =>
        {
            SetDoctorResult();
            JumpContent();
        });
        DOVirtual.DelayedCall(0.5f, OpenContent);
    }

    private void SetDoctorResult()
    {
        string infoString = infoStrings.Dequeue();
        bool isSick = infoString.Contains("SK");
        bool isDead = infoString is "SK001" or "SK002";

        if (isSick)
        {
            sickNameText.text = App.factory.stringFactory.GetSickName(infoString);
            sickInfoText.text = App.factory.stringFactory.GetSickInfo(infoString);
            sickInfoImage.sprite = App.factory.sickFactory.GetSickSprite(infoString);

            // 病情等級
            int sickLevel = App.factory.sickFactory.GetSickLevel(infoString);
            for (int i = 0; i < sickLevels.Length; i++)
                sickLevels[i].SetActive(i == sickLevel);

            // 印章
            deadStamp.SetActive(isDead);
            vetStamp.SetActive(!isDead);

            // 方法
            noSolutionObject.SetActive(isDead);
            solutionObject.SetActive(!isDead);
        }
        else
        {
            sickNameText.text = App.factory.stringFactory.GetHospitalFunctionName(infoString);
            sickInfoText.text = App.factory.stringFactory.GetHospitalFunctionInfo(infoString);
            sickInfoImage.sprite = App.factory.sickFactory.GetHospitalFunctionSprite(infoString);

            for (int i = 0; i < sickLevels.Length; i++)
                sickLevels[i].SetActive(i == 3);

            deadStamp.SetActive(false);
            vetStamp.SetActive(true);

            noSolutionObject.SetActive(true);
            solutionObject.SetActive(false);
        }
    }

    private void CloseContent()
    {
        contentGroup.DOFade(0, 0.25f).From(1);
    }

    private void JumpContent()
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

        vetStamp.transform.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        deadStamp.transform.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        
        okRect.DOScale(Vector2.one, 0.2f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.4f);
    }

    private void OpenContent()
    {
        contentGroup.DOFade(1, 0.25f).From(0);
    }
}
