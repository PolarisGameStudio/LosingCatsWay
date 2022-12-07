using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class View_Information : ViewBehaviour
{
    public View_SubInformation view_SubInformation;

    [Title("Player")]
    [SerializeField] private Image playerHeadImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private Image expFill;
    [SerializeField] private TextMeshProUGUI playerExpText;

    [Title("Nav")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("TopRight")]
    [SerializeField] private TextMeshProUGUI chapterNameText;
    [SerializeField] private TextMeshProUGUI chapterIndexText;
    [SerializeField] private TextMeshProUGUI slotText;

    [Title("Cats")]
    [SerializeField] private CatInformationCard[] catInformations;
    [SerializeField] private GameObject diamondUnlockBlock;
    [SerializeField] private GameObject levelUnlockBlock;
    [SerializeField] private TextMeshProUGUI levelUnlockSlotText;

    public override void Init()
    {
        base.Init();
        App.model.information.OnMyCatsChange += OnMyCatsChange;
        App.system.player.OnExpChange += OnPlayerExpChange;
        App.system.player.OnPlayerIdChange += OnPlayerIdChange;
        App.system.player.OnLevelChange += OnPlayerLevelChange;
        App.system.player.OnPlayerNameChange += OnPlayerNameChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
        App.system.player.OnCatSlotChange += OnCatSlotChange;
    }

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < catInformations.Length; i++)
        {
            catInformations[i].gameObject.SetActive(false);
        }
    }

    private void OnCatSlotChange(object value)
    {
        int slot = (int)value;
        var cats = App.system.cat.GetCats();
        
        slotText.text = slot.ToString();

        for (int i = 0; i < catInformations.Length; i++)
        {
            if (i >= slot)
            {
                catInformations[i].SetActive(false);
                continue;
            }

            if (i >= cats.Count)
            {
                catInformations[i].SetCat(null);
                catInformations[i].SetActive(true);
                continue;
            }
            
            catInformations[i].SetCat(cats[i].cloudCatData);
            catInformations[i].SetActive(true);
        }
        
        int slotByLevel = App.system.player.playerDataSetting.GetCatSlotByLevel(App.system.player.Level);
        diamondUnlockBlock.SetActive(slot - slotByLevel < 12);
    }

    private void OnMyCatsChange(object value)
    {
        var cats = (List<Cat>)value;
        int slot = App.system.player.CatSlot;

        for (int i = 0; i < catInformations.Length; i++)
        {
            if (i >= slot)
            {
                catInformations[i].SetActive(false);
                continue;
            }

            if (i >= cats.Count)
            {
                catInformations[i].SetCat(null);
                catInformations[i].SetActive(true);
                continue;
            }
            
            catInformations[i].SetCat(cats[i].cloudCatData);
            catInformations[i].SetActive(true);
        }

        diamondUnlockBlock.SetActive(App.system.player.DiamondCatSlot < 12);
    }

    private void OnPlayerIdChange(object playerId)
    {
        playerIdText.text = $"ID:{playerId}";
    }

    private void OnPlayerNameChange(object playerName)
    {
        playerNameText.text = (string) playerName;
    }

    private void OnPlayerLevelChange(object value)
    {
        int level = Convert.ToInt32(value);
        playerLevelText.text = level.ToString("00");
        
        levelUnlockBlock.SetActive(level < 85);

        if (level < 2)
        {
            levelUnlockSlotText.text = "2";
            return;
        }
        
        if (level < 10)
        {
            levelUnlockSlotText.text = "10";
            return;
        }
        
        if (level < 25)
        {
            levelUnlockSlotText.text = "25";
            return;
        }
        
        if (level < 40)
        {
            levelUnlockSlotText.text = "40";
            return;
        }
        
        if (level < 55)
        {
            levelUnlockSlotText.text = "55";
            return;
        }
        
        if (level < 70)
        {
            levelUnlockSlotText.text = "70";
            return;
        }
        
        if (level < 85)
        {
            levelUnlockSlotText.text = "85";
            return;
        }
    }

    private void OnPlayerExpChange(object from, object to)
    {
        //todo 要動畫
        int lastExp = Convert.ToInt32(from);
        int newExp = Convert.ToInt32(to);
        int fullExp = App.system.player.NextLevelExp;

        expFill.fillAmount = 1f / fullExp * lastExp;
        expFill.DOFillAmount((1f / fullExp * newExp), .3f);

        playerExpText.text = $"{newExp} / {fullExp}";
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