using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_SubInfo_Status : ViewBehaviour
{
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private TextMeshProUGUI catVarietyText;
    [SerializeField] private Card_ChipInfo chipInfo;
    
    [Title("Gender")]
    [SerializeField] private TextMeshProUGUI catSexText;
    [SerializeField] private Image catSexImage;
    [SerializeField] private GameObject ligationImage;
    
    [Title("Age")]
    [SerializeField]
    private TextMeshProUGUI catAgeText;
    [SerializeField] private TextMeshProUGUI catAgeLevelText;
    
    [Title("Size")]
    [SerializeField]
    private TextMeshProUGUI catSizeText;
    
    [Title("Health")]
    [SerializeField] private TextMeshProUGUI healthText;
    
    [Title("Status")]
    [SerializeField] private TextMeshProUGUI satietyValueText;
    [SerializeField] private TextMeshProUGUI satietyValueText_Back;
    [SerializeField] private TextMeshProUGUI favorabilityValueText;
    [SerializeField] private TextMeshProUGUI favorabilityValueText_Back;
    [SerializeField] private TextMeshProUGUI moistureValueText;
    [SerializeField] private TextMeshProUGUI moistureValueText_Back;
    
    [Title("Mood")]
    [SerializeField] private Image moodImage;
    
    [Title("Personality")]
    [SerializeField] private Card_Personality[] cardPersonalitys;
    
    [Title("Bar")]
    [SerializeField] private Image satietyBar;
    [SerializeField] private Image moistureBar;
    [SerializeField] private Image favorabilityBar;

    public override void Init()
    {
        base.Init();
        App.model.information.OnSelectedCatChange += OnSelectedCatChange;
    }

    public override void Open()
    {
        chipInfo.CloseInfo();
        base.Open();
    }

    public override void Close()
    {
        chipInfo.CloseInfo();
        base.Close();
    }

    private void OnSelectedCatChange(object value)
    {
        var cat = (Cat)value;

        catNameText.text = cat.cloudCatData.CatData.CatName;
        chipInfo.SetData(cat.cloudCatData);

        catSizeText.text = $"{CatExtension.GetCatRealSize(cat.cloudCatData.CatData.BodyScale):0.00}cm";

        bool isKitty = CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays) == 0;
        catVarietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cat.cloudCatData.CatData.Variety);
        
        catAgeText.text = cat.cloudCatData.CatData.CatAge.ToString();
        catAgeLevelText.text =
            App.factory.stringFactory.GetAgeLevelString(cat.cloudCatData.CatData.SurviveDays);
        
        catSexImage.sprite = App.factory.catFactory.GetCatSexSpriteEW(cat.cloudCatData.CatData.Sex);
        ligationImage.SetActive(cat.cloudCatData.CatHealthData.IsLigation);

        satietyBar.DOFillAmount(cat.cloudCatData.CatSurviveData.Satiety / 100f, .75f).From(0).SetEase(Ease.OutExpo).SetDelay(0.1875f);
        moistureBar.DOFillAmount(cat.cloudCatData.CatSurviveData.Moisture / 100f, .75f).From(0).SetEase(Ease.OutExpo).SetDelay(0.1875f + 0.1f);
        favorabilityBar.DOFillAmount(cat.cloudCatData.CatSurviveData.Favourbility / 100f, .75f).From(0).SetEase(Ease.OutExpo).SetDelay(0.1875f + 0.2f);

        satietyValueText.text = Convert.ToInt32(cat.cloudCatData.CatSurviveData.Satiety) + "/100%";
        favorabilityValueText.text = Convert.ToInt32(cat.cloudCatData.CatSurviveData.Favourbility) + "/100%";
        moistureValueText.text = Convert.ToInt32(cat.cloudCatData.CatSurviveData.Moisture) + "/100%";

        //色違字
        satietyValueText_Back.text = Convert.ToInt32(cat.cloudCatData.CatSurviveData.Satiety) + "/100%";
        favorabilityValueText_Back.text = Convert.ToInt32(cat.cloudCatData.CatSurviveData.Favourbility) + "/100%";
        moistureValueText_Back.text = Convert.ToInt32(cat.cloudCatData.CatSurviveData.Moisture) + "/100%";

        healthText.text =
            String.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId) ? App.factory.stringFactory.GetHealthString() : App.factory.stringFactory.GetSickString();

        catSexText.text = cat.cloudCatData.CatData.Sex == 0 ? App.factory.stringFactory.GetBoyString() : App.factory.stringFactory.GetGirlString();

        int mood = CatExtension.GetCatMood(cat.cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            if (i >= cat.cloudCatData.CatData.PersonalityTypes.Count)
            {
                cardPersonalitys[i].gameObject.SetActive(false);
                continue;
            }

            int personality = cat.cloudCatData.CatData.PersonalityTypes[i];
            int level = cat.cloudCatData.CatData.PersonalityLevels[i];
            cardPersonalitys[i].SetData(personality, level);
            cardPersonalitys[i].gameObject.SetActive(true);
        }
    }
}
