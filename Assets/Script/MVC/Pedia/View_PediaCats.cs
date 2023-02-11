using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_PediaCats : ViewBehaviour
{
    [SerializeField] private UIView chooseCatView;
    [SerializeField] private UIView readCatView;
    [SerializeField] private Card_ChooseCat[] cards;

    #region ReadCat

    public Sprite[] starStatusSprites;
    public Image[] starStatusImages;
    public TextMeshProUGUI[] levelCountTexts;
    public TextMeshProUGUI catTypeText;
    public CatSkin catSkin;

    #endregion

    public override void Init()
    {
        base.Init();
        App.model.pedia.OnUsingCasIdsChange += OnUsingCasIdsChange;
        App.model.pedia.OnSelectedCatIdChange += OnSelectedCatIdChange;
    }

    public override void Open()
    {
        base.Open();
        CloseReadCat();
        OpenChooseCat();
    }

    public override void Close()
    {
        CloseChooseCat();
        CloseReadCat();
        base.Close();
    }

    private void OpenChooseCat()
    {
        chooseCatView.Show();
    }

    public void OpenReadCat()
    {
        readCatView.Show();
        CloseChooseCat();
    }

    private void CloseChooseCat()
    {
        chooseCatView.InstantHide();
    }

    public void CloseReadCat()
    {
        readCatView.InstantHide();
        OpenChooseCat();
    }

    private void OnUsingCasIdsChange(object value)
    {
        List<string> usingCasIds = (List<string>)value;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= usingCasIds.Count)
            {
                cards[i].gameObject.SetActive(false);
                continue;
            }

            cards[i].gameObject.SetActive(true);
            cards[i].SetData(usingCasIds[i]);
        }
    }

    private void OnSelectedCatIdChange(object value)
    {
        string variety = value.ToString();
        int count = App.system.quest.KnowledgeCardData[variety];

        if (count >= 10)
        {
            starStatusImages[1].sprite = starStatusSprites[1];
            catSkin.PlayAnimation();
        }
        else
        {
            starStatusImages[1].sprite = starStatusSprites[0];
            catSkin.StopAnimation();
        }

        if (count >= 5)
            starStatusImages[0].sprite = starStatusSprites[1];
        else
            starStatusImages[0].sprite = starStatusSprites[0];

        levelCountTexts[0].text = "(" + Math.Clamp(count, 0, 5) + "/5)";
        levelCountTexts[1].text = "(" + Math.Clamp(count, 0, 10) + "/10)";

        catTypeText.text = App.factory.stringFactory.GetCatVariety(variety);
        catSkin.ChangeSkin(variety);
    }
}