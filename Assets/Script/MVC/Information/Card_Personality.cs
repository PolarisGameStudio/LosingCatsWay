using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_Personality : MvcBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image levelImage;
    [SerializeField] private TextMeshProUGUI nameText;

    public void SetData(int personality, int level)
    {
        iconImage.sprite = App.factory.catFactory.GetPersonalitySprite(personality);
        levelImage.sprite = App.factory.catFactory.GetPersonalityLevelSprite(level);

        string id = $"{personality}{level}";
        nameText.text = App.factory.stringFactory.GetPersonality(id);
    }
}
