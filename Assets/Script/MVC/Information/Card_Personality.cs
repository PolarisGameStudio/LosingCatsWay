using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_Personality : MvcBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;

    public void SetData(int personality, int level)
    {
        string id = $"{personality}{level}";
        nameText.text = App.factory.stringFactory.GetPersonality(id);
        iconImage.sprite = App.factory.catFactory.GetPersonalityImage(id);
    }
}
