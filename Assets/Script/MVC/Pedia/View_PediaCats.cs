using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Firebase.Firestore;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class View_PediaCats : ViewBehaviour
{
    [SerializeField] private UIView chooseCatView;
    [SerializeField] private UIView readCatView;

    // Cards
    [SerializeField] private Card_PediaCat[] cards;

    // ReadCat
    public CatSkin catSkin;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;

    public override void Init()
    {
        base.Init();
        App.model.pedia.OnUsingCatIdsChange += OnUsingPediaIdsChange;
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
    }

    private void CloseChooseCat()
    {
        chooseCatView.InstantHide();
    }

    public void CloseReadCat()
    {
        readCatView.InstantHide();
    }
    
    private void OnUsingPediaIdsChange(object value)
    {
        var ids = (List<string>)value;
        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= ids.Count)
            {
                cards[i].gameObject.SetActive(false);
                continue;
            }

            cards[i].gameObject.SetActive(true);
            cards[i].SetData(ids[i]);
        }
    }
    
    private void OnSelectedCatIdChange(object value)
    {
        string id = value.ToString();
        int level = App.system.inventory.KnowledgeCardDatas[id];

        CloudCatData cloudCatData = new CloudCatData();
        cloudCatData.CatData = new CloudSave_CatData();
        cloudCatData.CatData.Variety = id;
        cloudCatData.CatData.BodyScale = 1;
        cloudCatData.CatData.BornTime = Timestamp.FromDateTime(DateTime.MinValue);

        cloudCatData.CatSkinData = new CloudSave_CatSkinData();
        cloudCatData.CatHealthData = new CloudSave_CatHealthData();
            
        catSkin.ChangeSkin(cloudCatData);
        
        catSkin.GetComponentInChildren<SkeletonGraphic>().timeScale =
            level >= 3 ? 1 : 0;
        
        titleText.text = App.factory.stringFactory.GetCatVariety(id);
        contentText.text = App.factory.stringFactory.GetPediaContent(id).Replace(" ", "");
    }
}
