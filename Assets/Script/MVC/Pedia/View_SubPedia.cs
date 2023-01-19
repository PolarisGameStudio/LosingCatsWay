using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class View_SubPedia : ViewBehaviour
{
    [Title("UIView")] 
    public View_ChoosePedia choosePedia;
    public View_ReadPedia readPedia;
    
    [Title("UI")]
    [SerializeField] private Card_PediaType[] cards;
    [SerializeField] private Image[] typeBackImages;
    [SerializeField] private TextMeshProUGUI[] typeBackTexts;

    [Title("Sprites")] [SerializeField] private Sprite[] buttonColors;
    [SerializeField] private Material[] typeBackMaterials;
    [SerializeField] private Color32[] typeBackColors;
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnSelectedPediaTypeChange += OnSelectedPediaTypeChange;
    }

    private void OnSelectedPediaTypeChange(object value)
    {
        int index = (int)value;

        if (index < 0)
            return;

        for (int i = 0; i < typeBackImages.Length; i++)
            typeBackImages[i].sprite = buttonColors[index];

        for (int i = 0; i < typeBackTexts.Length; i++)
            typeBackTexts[i].fontMaterial = typeBackMaterials[index];

        for (int i = 0; i < typeBackTexts.Length; i++)
            typeBackTexts[i].color = typeBackColors[index];

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].ChangeColor(i);
            
            if (i == index)
                cards[i].SetSelect(true);
            else
                cards[i].SetSelect(false);
        }
    }
}
