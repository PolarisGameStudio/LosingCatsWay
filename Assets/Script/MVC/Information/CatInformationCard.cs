using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatInformationCard : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;

    [Title("Data")]
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private TextMeshProUGUI catVarietyText;
    [SerializeField] private TextMeshProUGUI catAgeText;
    [SerializeField] private Image catSexImage;
    [SerializeField] private Image moodImage;
    
    [Title("Bar")]
    [SerializeField] private Image satietyValueImage;
    [SerializeField] private Image favorabilityValueImage;
    [SerializeField] private Image moistureValueImage;

    [Title("Mask")] [SerializeField] private GameObject emptySlotMask;

    public void Select()
    {
        int index = transform.GetSiblingIndex(); //取得自身順序
        App.controller.information.SelectCat(index);
    }
    
    public void SetCat(CloudCatData cloudCatData)
    {
        if (cloudCatData == null)
        {
            emptySlotMask.SetActive(true);
            return;
        }

        emptySlotMask.SetActive(false);
        
        catNameText.text = cloudCatData.CatData.CatName;

        bool isKitty = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0;
        catVarietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);
        
        catAgeText.text = cloudCatData.CatData.CatAge.ToString();
        catSexImage.sprite = App.factory.catFactory.GetCatSexSpriteWhite(cloudCatData.CatData.Sex);

        satietyValueImage.DOFillAmount((cloudCatData.CatSurviveData.Satiety / 100), 0.8f).From(0).SetEase(Ease.OutExpo).SetDelay(0.1875f);
        moistureValueImage.DOFillAmount((cloudCatData.CatSurviveData.Moisture / 100), 0.8f).From(0).SetEase(Ease.OutExpo).SetDelay(0.1875f);
        favorabilityValueImage.DOFillAmount((cloudCatData.CatSurviveData.Favourbility / 100), 0.8f).From(0).SetEase(Ease.OutExpo).SetDelay(0.1875f);

        int mood = CatExtension.GetCatMood(cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        catSkin.ChangeSkin(cloudCatData);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }

    public void OpenMap()
    {
        App.controller.information.OpenMap();
    }
}
