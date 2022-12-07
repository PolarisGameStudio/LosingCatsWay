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
    
    public override void Init()
    {
        base.Init();
        App.model.shelter.OnCloudCatDatasChange += OnCloudCatDatasChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

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
