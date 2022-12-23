using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class View_SubInformation : ViewBehaviour
{
    public CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI catIdText;

    [Title("SubInfo")] [SerializeField] private View_SubInfo_Status status;
    [SerializeField] private View_SubInfo_ChooseSkin chooseSkin;

    [Title("Tab")] [SerializeField] private GameObject[] tabMasks;
    [SerializeField] private UIButton chooseSkinButton;

    public override void Init()
    {
        base.Init();
        App.model.information.OnSelectedCatChange += OnSelectedCatChange;
        App.model.information.OnTabIndexChange += OnTabIndexChange;
    }

    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
    }

    public void OpenStatus()
    {
        status.Open();
    }

    public void CloseStatus()
    {
        status.Close();
    }

    public void OpenChooseSkin()
    {
        chooseSkin.Open();
    }

    public void CloseChooseSkin()
    {
        chooseSkin.Close();
    }

    private void OnSelectedCatChange(object value)
    {
        var cat = (Cat)value;
        string tmpID = "ID:" + cat.cloudCatData.CatData.CatId;
        catIdText.text = tmpID;
        catSkin.ChangeSkin(cat.cloudCatData);

        chooseSkinButton.interactable = false;
        if (CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays) == 0)
            return;
        if (!string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId) || cat.cloudCatData.CatHealthData.IsBug)
            return;
        chooseSkinButton.interactable = true;
    }

    private void OnTabIndexChange(object value)
    {
        int index = (int)value;

        for (int i = 0; i < tabMasks.Length; i++)
        {
            if (i == index)
                tabMasks[i].SetActive(true);
            else
                tabMasks[i].SetActive(false);
        }
    }
}