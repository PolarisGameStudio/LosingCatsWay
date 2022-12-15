using System.Collections;
using System.Collections.Generic;
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
    public Button chipButton;
    [Space(10)]

    public TextMeshProUGUI varietyText;
    public TextMeshProUGUI ageText;
    [Space(10)]

    public Image satietyBar;
    public Image moistureBar;
    public Image funBar;

    [SerializeField] private Card_ChipInfo chipInfo;

    public void SetData(Cat cat)
    {
        catSkin.ChangeSkin(cat.cloudCatData);
        chipInfo.SetData(cat.cloudCatData);

        int mood = CatExtension.GetCatMood(cat.cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        nameText.text = cat.cloudCatData.CatData.CatName;
        genderImage.sprite = App.factory.catFactory.GetCatSexSpriteWhite(cat.cloudCatData.CatData.Sex);

        bool isKitty = CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays) == 0;
        varietyText.text = isKitty ? App.factory.stringFactory.GetKittyName() : App.factory.stringFactory.GetCatVariety(cat.cloudCatData.CatData.Variety);

        ageText.text = cat.cloudCatData.CatData.CatAge.ToString();

        satietyBar.fillAmount = cat.cloudCatData.CatSurviveData.Satiety / 100;
        moistureBar.fillAmount = cat.cloudCatData.CatSurviveData.Moisture / 100;
        funBar.fillAmount = cat.cloudCatData.CatSurviveData.Favourbility / 100;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.feed.Select(index);
        App.system.soundEffect.Play("Button");
    }
}
