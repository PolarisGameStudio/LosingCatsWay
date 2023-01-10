using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using System;

public class View_Shelter : ViewBehaviour
{
    [Title("Views")]
    public View_SubShelter subShelter;

    [Title("Cages")]
    public Object_Cage[] cages;
    [SerializeField] private GameObject blankCages;

    [Title("UI")]
    public Scrollbar shelterScrollBar;
    public TMP_InputField inputField;

    [Title("Nav")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("Spine")] 
    public GameObject npc;

    [Title("Refresh")] [SerializeField] private TextMeshProUGUI freeCount;
    [SerializeField] private TextMeshProUGUI adsCount;
    [SerializeField] private GameObject freeTitle;
    [SerializeField] private GameObject adsTitle;
    [SerializeField] private GameObject noCountTitle;
    [SerializeField] private GameObject cooldownObject;
    [SerializeField] private GameObject refreshObject;

    public override void Open()
    {
        base.Open();
        shelterScrollBar.value = 0;
        npc.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        npc.SetActive(false);
    }
    
    public override void Init()
    {
        base.Init();
        App.model.shelter.OnCloudCatDatasChange += OnCloudCatDatasChange;
        App.model.shelter.OnFreeRefreshChange += OnFreeRefreshChange;
        App.model.shelter.OnAdsRefreshChange += OnAdsRefreshChange;
        App.model.shelter.OnCooldownChange += OnCooldownChange;
        
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnCooldownChange(object value)
    {
        DateTime cooldown = (DateTime)value;
        if (cooldown > App.system.myTime.MyTimeNow)
        {
            cooldownObject.SetActive(true);
            refreshObject.SetActive(false);
        }
        else
        {
            cooldownObject.SetActive(false);
            refreshObject.SetActive(true);
        }
    }

    private void OnAdsRefreshChange(object value)
    {
        int count = (int)value;
        if (count <= 0)
        {
            adsTitle.SetActive(false);
            adsCount.gameObject.SetActive(false);
            noCountTitle.SetActive(true);
            return;
        }

        noCountTitle.SetActive(false);
        adsTitle.SetActive(true);
        adsCount.gameObject.SetActive(true);
        adsCount.text = $"({count}/5)";
    }

    private void OnFreeRefreshChange(object value)
    {
        int count = (int)value;
        if (count <= 0)
        {
            freeTitle.SetActive(false);
            freeCount.gameObject.SetActive(false);
            return;
        }

        noCountTitle.SetActive(false);
        freeTitle.SetActive(true);
        freeCount.gameObject.SetActive(true);
        freeCount.text = $"({count}/3)";
    }

    private void OnCloudCatDatasChange(object value)
    {
        List<CloudCatData> cats = (List<CloudCatData>) value;

        for (int i = 0; i < cages.Length; i++)
        {
            if (i < cats.Count)
            {
                cages[i].SetActive(true);
                cages[i].RefreshCat(cats[i]);

                int index = i;
                cages[i].Select(index);
            }
            else
            {
                cages[i].SetActive(false);
            }
        }
        
        blankCages.SetActive((cats.Count % 2) == 1);
    }

    private void OnCoinChange(object value)
    {
        int coin = Convert.ToInt32(value);
        coinText.text = coin.ToString();
    }

    private void OnDiamondChange(object value)
    {
        int diamond = Convert.ToInt32(value);
        diamondText.text = diamond.ToString();
    }
}
