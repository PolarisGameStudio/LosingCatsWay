using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View_FollowCat : ViewBehaviour
{
    public CatSkin catSkin;

    //StatusUI
    public TextMeshProUGUI catNameText;
    public TextMeshProUGUI catAgeText;
    public Image catSexImage;
    public TextMeshProUGUI varietyText;
    [SerializeField] private TmpRevolving tmpRevolving;

    public Image satietyValueImage;
    public Image favorabilityValueImage;
    public Image moistureValueImage;

    public Image moodImage;

    [SerializeField] private UIButton traitButton;

    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);
        tmpRevolving.StartRevolving();
    }

    public override void Close()
    {
        catSkin.SetActive(false);
        tmpRevolving.StopRevolving();
        base.Close();
    }

    public override void Init()
    {
        base.Init();
        App.model.followCat.OnSelectedCatChange += OnSelectedCatChange;
    }

    private void OnSelectedCatChange(object value)
    {
        var cat = (Cat)value;
        CloudCatData cloudCatData = cat.cloudCatData;
        
        catNameText.text = cloudCatData.CatData.CatName;
        
        catAgeText.text = cloudCatData.CatData.CatAge.ToString();
        catSexImage.sprite = App.factory.catFactory.GetCatSexSpriteWhite(cloudCatData.CatData.Sex);

        satietyValueImage.fillAmount = cloudCatData.CatSurviveData.Satiety / 100;
        favorabilityValueImage.fillAmount = cloudCatData.CatSurviveData.Favourbility / 100;
        moistureValueImage.fillAmount = cloudCatData.CatSurviveData.Moisture / 100;

        int mood = CatExtension.GetCatMood(cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        bool isKitty = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0;
        varietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);

        catSkin.ChangeSkin(cloudCatData);
        
        traitButton.interactable = cloudCatData.CatSkinData.UseSkinId is "Robot" or "Flyfish" or "Magic_Hat";
    }
}
