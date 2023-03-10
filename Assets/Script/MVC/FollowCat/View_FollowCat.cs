using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View_FollowCat : ViewBehaviour
{
    public CatSkin catSkin;

    //StatusUI
    public TextMeshProUGUI catNameText;
    public TextMeshProUGUI catAgeText;
    public TextMeshProUGUI varietyText;
    public Image catSexImage;
    public Image moodImage;

    [Title("Dialog")]
    [SerializeField] private GameObject dialogObject;
    [SerializeField] private TextMeshProUGUI growText;

    [Title("Fill")]
    [SerializeField] private Image satietyFill;
    [SerializeField] private Image moistureFill;
    [SerializeField] private Image favourabilityFill;
    [SerializeField] private Image satietyFillInner;
    [SerializeField] private Image moistureFillInner;
    [SerializeField] private Image funFillInner;
    [SerializeField] private Sprite lowValueSprite;
    [SerializeField] private Sprite highValueSprite;

    [Title("Trait")]
    [SerializeField] private UIButton traitButton;
    [SerializeField] private GameObject traitMask;
    [SerializeField] private UIView traitView;

    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);
    }

    public override void Close()
    {
        catSkin.SetActive(false);
        base.Close();
    }

    public void OpenTrait()
    {
        traitView.Show();
    }

    public void CloseTrait()
    {
        traitView.InstantHide();
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

        satietyFill.fillAmount = cloudCatData.CatSurviveData.Satiety / 100;
        favourabilityFill.fillAmount = cloudCatData.CatSurviveData.Favourbility / 100;
        moistureFill.fillAmount = cloudCatData.CatSurviveData.Moisture / 100;
        
        satietyFillInner.sprite = cloudCatData.CatSurviveData.Satiety > 20 ? highValueSprite : lowValueSprite;
        moistureFillInner.sprite = cloudCatData.CatSurviveData.Moisture > 20 ? highValueSprite : lowValueSprite;
        funFillInner.sprite = cloudCatData.CatSurviveData.Favourbility > 20 ? highValueSprite : lowValueSprite;

        int mood = CatExtension.GetCatMood(cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        bool isKitty = cloudCatData.CatData.SurviveDays <= 3;
        varietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);

        catSkin.ChangeSkin(cloudCatData);
        
        traitButton.interactable = cloudCatData.CatSkinData.UseSkinId is "Robot_Cat" or "Flyfish_Cat" or "Magic_Hat";
        traitMask.SetActive(!traitButton.interactable);
        
        dialogObject.SetActive(isKitty);
        if (isKitty)
            growText.text = (3 - cloudCatData.CatData.SurviveDays).ToString();
    }
}
