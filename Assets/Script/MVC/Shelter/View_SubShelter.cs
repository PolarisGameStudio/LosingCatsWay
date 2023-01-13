using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_SubShelter : ViewBehaviour
{
    public CatSkin catSkin;

    [Title("Left")] [SerializeField] private UIButton idButton;
    [SerializeField] private TextMeshProUGUI catIdText;
    [SerializeField] private GameObject blackMask;
    
    [Title("Top")]
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private Image moodImage;
    [SerializeField] private Card_ChipInfo info;
    [SerializeField] private GameObject infoButtonObject;

    [Title("Center")]
    [SerializeField] private TextMeshProUGUI catSexText;
    [SerializeField] private Image catSexImage;
    [SerializeField] private GameObject ligationImage;
    [SerializeField] private TextMeshProUGUI catVarietyText;
    [SerializeField] private GameObject[] healthTexts;
    [SerializeField] private TextMeshProUGUI catAgeText;
    [SerializeField] private TextMeshProUGUI catAgeLevelText;
    [SerializeField] private TextMeshProUGUI catSizeText;
    [SerializeField] private Card_Personality[] cardPersonalitys;

    public override void Init()
    {
        base.Init();
        App.model.shelter.OnSelectedAdoptCloudCatDataChange += OnSelectedAdoptCatDataChange;
    }

    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
    }

    private void OnSelectedAdoptCatDataChange(object value)
    {
        CloudCatData cloudCatData = (CloudCatData)value;

        catNameText.text = cloudCatData.CatData.CatName;
        
        bool isKitty = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0;
        catVarietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);

        catSizeText.text = $"{CatExtension.GetCatRealSize(cloudCatData.CatData.BodyScale):0.00}cm";

        catAgeText.text = cloudCatData.CatData.CatAge.ToString();
        catAgeLevelText.text =
            App.factory.stringFactory.GetAgeLevelString(cloudCatData.CatData.SurviveDays);
        
        catSexImage.sprite = App.factory.catFactory.GetCatSexSpriteEW(cloudCatData.CatData.Sex);
        ligationImage.SetActive(cloudCatData.CatHealthData.IsLigation);

        for (int i = 0; i < healthTexts.Length; i++)
            healthTexts[i].SetActive(false);

        int healthStatus;
        if (string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            healthStatus = 0;
        else if (cloudCatData.CatHealthData.SickId is "SK001" or "SK002")
            healthStatus = 2;
        else
            healthStatus = 1;
        
        healthTexts[healthStatus].SetActive(true);

        catSkin.ChangeSkin(cloudCatData);

        catSexText.text = (cloudCatData.CatData.Sex == 0) ? App.factory.stringFactory.GetBoyString() : App.factory.stringFactory.GetGirlString();

        int mood = CatExtension.GetCatMood(cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            if (i >= cloudCatData.CatData.PersonalityTypes.Count)
            {
                cardPersonalitys[i].gameObject.SetActive(false);
                continue;
            }

            int personality = cloudCatData.CatData.PersonalityTypes[i];
            int level = cloudCatData.CatData.PersonalityLevels[i];
            cardPersonalitys[i].SetData(personality, level);
            cardPersonalitys[i].gameObject.SetActive(true);
        }
        
        //Info
        if (cloudCatData.CatHealthData.IsChip)
            info.SetData(cloudCatData);
        
        infoButtonObject.SetActive(cloudCatData.CatHealthData.IsChip);
        
        //ID
        string tmpID = "ID:" + cloudCatData.CatData.CatId;
        catIdText.text = (cloudCatData.CatHealthData.IsChip) ? tmpID : "ID:-";
        idButton.interactable = cloudCatData.CatHealthData.IsChip;
        blackMask.SetActive(!cloudCatData.CatHealthData.IsChip);
    }
}
