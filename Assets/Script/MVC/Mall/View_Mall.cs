using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class View_Mall : ViewBehaviour
{
    [Title("Top")] 
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI catMemoryText;

    [Title("Sections")] public Transform selectedBlock;
    public Transform[] sections;
    public TextMeshProUGUI selectedBlockText;

    [Title("Page")] 
    public MallContainer[] pages;

    [Title("PreviewPackage")] 
    public UIView previewPackageView;
    public Transform previewPackageContent;
    public Item_Mall_Preview previewPackageObject;
    public Scrollbar previewPackageScrollbar;
    [SerializeField] private TextMeshProUGUI previewPackageName;

    [Title("Rules")]
    public UIView ruleView;
    public UIView mainRule;
    public UIView serviceRule;
    public Scrollbar serviceBar;
    public UIView privacyRule;
    public Scrollbar privacyBar;

    public override void Init()
    {
        base.Init();

        App.model.mall.onSelectedPageIndexChange += OnSelectedPageIndexChange;
        App.model.mall.onPreviewPackageRewardsChange += OnPreviewPackageRewardsChange;
        App.model.mall.OnRuleIndexChange += OnRuleIndexChange;
        App.model.mall.OnPreviewPackageIdChange += OnPreviewPackageIdChange;

        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
        App.system.player.OnCatMemoryChange += OnCatMemoryChange;
    }

    private void OnPreviewPackageIdChange(object value)
    {
        string id = value.ToString();
        string packageName = App.factory.stringFactory.GetMallItemName(id);
        previewPackageName.text = packageName;
    }

    public void OpenPreviewPackageView()
    {
        previewPackageView.Show();
        previewPackageScrollbar.value = 1;
    }

    public void ClosePreviewPackageView()
    {
        previewPackageView.InstantHide();
    }
    
    private void OnSelectedPageIndexChange(object from, object to)
    {
        int value1 = Convert.ToInt32(from);
        int value2 = Convert.ToInt32(to);

        if (value1 != -1)
            pages[value1].Close();

        pages[value2].Open();
        
        selectedBlock.DOMove(sections[value2].position + new Vector3(0, 12.75f, 0), 0.25f);

        // Text
        var text = sections[value2].GetComponentInChildren<TextMeshProUGUI>().text;
        selectedBlockText.text = text;
    }
    
    private void OnPreviewPackageRewardsChange(object value)
    {
        Reward[] rewards = (Reward[])value;

        for (int i = 0; i < previewPackageContent.childCount; i++)
            Destroy(previewPackageContent.GetChild(i).gameObject);

        for (int i = 0; i < rewards.Length; i++)
        {
            Item_Mall_Preview itemMallPreview = Instantiate(previewPackageObject, previewPackageContent);
            itemMallPreview.SetData(rewards[i]);
        }
    }
    
    private void OnCoinChange(object value)
    {
        int coin = (int)value;
        coinText.text = coin.ToString();
    }
    
    private void OnDiamondChange(object value)
    {
        int diamond = (int)value;
        diamondText.text = diamond.ToString();
    }

    private void OnCatMemoryChange(object value)
    {
        int catMemory = (int)value;
        catMemoryText.text = catMemory.ToString();
    }

    private void OnRuleIndexChange(object value)
    {
        int ruleIndex = (int)value;
        
        if (ruleIndex == 0)
            App.view.mall.OpenMainRule();
        if (ruleIndex == 1)
            App.view.mall.OpenServiceRule();
        if (ruleIndex == 2)
            App.view.mall.OpenPrivacyRule();
    }

    private void OpenMainRule()
    {
        serviceRule.InstantHide();
        privacyRule.InstantHide();
        
        ruleView.Show();
        mainRule.Show();
    }

    private void OpenServiceRule()
    {
        mainRule.InstantHide();
        privacyRule.InstantHide();
        
        ruleView.Show();
        serviceRule.Show();
        serviceBar.value = 1;
    }

    private void OpenPrivacyRule()
    {
        mainRule.InstantHide();
        serviceRule.InstantHide();
        
        ruleView.Show();
        privacyRule.Show();
        privacyBar.value = 1;
    }

    public void CloseRule()
    {
        ruleView.InstantHide();
    }
}