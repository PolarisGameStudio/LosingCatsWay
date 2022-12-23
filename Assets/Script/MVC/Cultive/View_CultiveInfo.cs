using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class View_CultiveInfo : ViewBehaviour
{
    public CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI idText;
    
    [Title("View")]
    public View_CultiveInfo_Status status;
    public View_CultiveInfo_ChooseSkin chooseSkin;

    [Title("Tab")] [SerializeField] private GameObject[] tabMasks;
    [SerializeField] private UIButton chooseSkinButton; //Tab

    public override void Init()
    {
        base.Init();
        App.model.cultive.OnSelectedTabChange += OnSelectedTabChange;
        App.model.cultive.OnSelectedCatChange += OnSelectedCatChange;
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

    private void OnSelectedCatChange(object value)
    {
        var cat = (Cat)value;
        catSkin.ChangeSkin(cat.cloudCatData);
        idText.text = $"ID:{cat.cloudCatData.CatData.CatId}";

        chooseSkinButton.interactable = false;
        if (CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays) == 0)
            return;
        if (!string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId) || cat.cloudCatData.CatHealthData.IsBug)
            return;
        chooseSkinButton.interactable = true;
    }

    private void OnSelectedTabChange(object value)
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
