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
    [SerializeField] private TextMeshProUGUI traitText;
    [SerializeField] private TextMeshProUGUI catVarietyText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI catAgeText;
    [SerializeField] private TextMeshProUGUI catAgeLevelText;
    [SerializeField] private TextMeshProUGUI catSizeText;
    [SerializeField] private Card_Personality[] cardPersonalitys;

    [Title("TraitColor")] [SerializeField] private Color32 commonTraitColor;
    [SerializeField] private Color32 rareTraitColor;
    [SerializeField] private Color32 ssrTraitColor;

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
        string traitString = App.factory.stringFactory.GetTraitString(cloudCatData.CatData.Trait);
        traitText.text = traitString;

        char traitHead = cloudCatData.CatData.Trait[0];
        if (traitHead == 'C')
            traitText.color = commonTraitColor;
        else if (traitHead == 'R')
            traitText.color = rareTraitColor;
        else if (traitHead == 'S')
            traitText.color = ssrTraitColor;
        
        bool isKitty = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0;
        catVarietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);

        catSizeText.text = $"{CatExtension.GetCatRealSize(cloudCatData.CatData.BodyScale):0.00}cm";

        catAgeText.text = cloudCatData.CatData.CatAge.ToString();
        catAgeLevelText.text =
            App.factory.stringFactory.GetAgeLevelString(cloudCatData.CatData.SurviveDays);
        
        catSexImage.sprite = App.factory.catFactory.GetCatSexSpriteEW(cloudCatData.CatData.Sex);
        ligationImage.SetActive(cloudCatData.CatHealthData.IsLigation);

        healthText.text =
            (string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId)) ? App.factory.stringFactory.GetHealthString() : App.factory.stringFactory.GetSickString();

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

    public void CopyID()
    {
        //TODO �ѽ��X
        string CatId = App.model.shelter.SelectedAdoptCloudCatData.CatData.CatId;
        CatId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
    }
}
