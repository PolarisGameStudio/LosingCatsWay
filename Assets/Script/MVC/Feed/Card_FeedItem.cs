using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card_FeedItem : MvcBehaviour
{
    public CatSkin catSkin;
    [Space(10)]

    public Image moodImage;
    [Space(10)]

    public TextMeshProUGUI nameText;
    public Image genderImage;
    [Space(10)]

    public TextMeshProUGUI varietyText;
    public TextMeshProUGUI ageText;
    [Space(10)]

    public Image satietyBar;
    public Image moistureBar;
    public Image funBar;

    [SerializeField] private Card_ChipInfo chipInfo;
    [SerializeField] private GameObject container;

    [Title("ChangeColor")]
    [SerializeField] private Image satietyFill;
    [SerializeField] private Image moistureFill;
    [SerializeField] private Image favourbilityFill;
    [SerializeField] private Sprite yellowSprite;
    [SerializeField] private Sprite redSprite;

    public void SetData(Cat cat)
    {
        catSkin.ChangeSkin(cat.cloudCatData);
        if (cat.cloudCatData.CatData.SurviveDays > 3)
            catSkin.SetStatusFace(cat.cloudCatData);
        chipInfo.SetData(cat.cloudCatData);

        int mood = CatExtension.GetCatMood(cat.cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        nameText.text = cat.cloudCatData.CatData.CatName;
        genderImage.sprite = App.factory.catFactory.GetCatSexSpriteWhite(cat.cloudCatData.CatData.Sex);

        bool isKitty = cat.cloudCatData.CatData.SurviveDays <= 3;
        varietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cat.cloudCatData.CatData.Variety);

        ageText.text = cat.cloudCatData.CatData.CatAge.ToString();

        satietyBar.fillAmount = cat.cloudCatData.CatSurviveData.Satiety / 100;
        moistureBar.fillAmount = cat.cloudCatData.CatSurviveData.Moisture / 100;
        funBar.fillAmount = cat.cloudCatData.CatSurviveData.Favourbility / 100;

        satietyFill.sprite = satietyBar.fillAmount > 0.2f ? yellowSprite : redSprite;
        moistureFill.sprite = moistureBar.fillAmount > 0.2f ? yellowSprite : redSprite;
        favourbilityFill.sprite = funBar.fillAmount > 0.2f ? yellowSprite : redSprite;
    }

    public void SetActiveContainer(bool active)
    {
        container.SetActive(active);
        catSkin.SetActive(active);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.feed.Select(index);
        App.system.soundEffect.Play("Button");
    }
}
