using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class View_Information : ViewBehaviour
{
    public View_SubInformation view_SubInformation;

    [Title("Player")]
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image playerAvatar;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private Image expFill;
    [SerializeField] private TextMeshProUGUI playerExpText;

    [Title("Nav")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("TopRight")]
    [SerializeField] private TextMeshProUGUI slotText;

    [Title("Cats")]
    [SerializeField] private CatInformationCard[] catInformations;
    [SerializeField] private GameObject diamondUnlockBlock;
    [SerializeField] private GameObject levelUnlockBlock;
    [SerializeField] private TextMeshProUGUI levelUnlockSlotText;
    [SerializeField] private TextMeshProUGUI diamondUnlockSlotText;

    public override void Open()
    {
        base.Open();
        slotText.text = App.system.player.CatSlot.ToString();
    }

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < catInformations.Length; i++)
        {
            catInformations[i].gameObject.SetActive(false);
        }
    }

    public override void Init()
    {
        base.Init();
        App.model.information.OnMyCatsChange += OnMyCatsChange;
        App.model.information.OnNextDiamondSlotPriceChange += OnNextDiamondSlotPriceChange;
        
        App.system.player.OnExpChange += OnPlayerExpChange;
        App.system.player.OnPlayerIdChange += OnPlayerIdChange;
        App.system.player.OnLevelChange += OnPlayerLevelChange;
        App.system.player.OnPlayerNameChange += OnPlayerNameChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
        App.system.player.OnDiamondCatSlotChange += OnDiamondCatSlotChange;
        App.system.player.OnUsingIconChange += OnUsingIconChange;
        App.system.player.OnUsingAvatarChange += OnUsingAvatarChange;
    }

    private void OnNextDiamondSlotPriceChange(object value)
    {
        int price = (int)value;
        diamondUnlockSlotText.text = price.ToString();
    }

    private void OnUsingAvatarChange(object value)
    {
        string id = value.ToString();
        playerAvatar.sprite = App.factory.itemFactory.GetItem(id).icon;

        if (!App.factory.itemFactory.avatarEffects.ContainsKey(id))
            return;
        GameObject effectObject = App.factory.itemFactory.avatarEffects[id];
        GameObject tmp = Instantiate(effectObject, playerAvatar.transform);
        
        UIParticle uiParticle = tmp.GetComponent<UIParticle>();
        if (uiParticle != null)
            uiParticle.scale = 48;
    }

    private void OnUsingIconChange(object value)
    {
        string id = value.ToString();
        playerIcon.sprite = App.factory.itemFactory.GetItem(id).icon;
    }

    private void OnDiamondCatSlotChange(object value)
    {
        int slot = (int)value;
        diamondUnlockBlock.SetActive(slot < 12);
        slotText.text = App.system.player.CatSlot.ToString();
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
        slotText.text = App.system.player.CatSlot.ToString();

        int catSlotLevel = App.system.player.playerDataSetting.GetCatSlotByLevel(level);

        if (catSlotLevel >= 6)
            levelUnlockSlotText.text = "45";
        else
            levelUnlockSlotText.text = App.system.player.playerDataSetting.catSlotUnlockLevels[catSlotLevel - 1].ToString();
    }

    private void OnPlayerExpChange(object from, object to)
    {
        //todo ?????????
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