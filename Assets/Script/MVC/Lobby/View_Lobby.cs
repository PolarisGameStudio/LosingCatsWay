﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Lobby : ViewBehaviour
{
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image playerAvatar;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private Image expFill;
    [SerializeField] private TextMeshProUGUI catCountText;
    [SerializeField] private TextMeshProUGUI roomCountText;

    [Title("Tween-Top")] public Transform topBg;

    [Title("Tween-Bottom")] public Transform downBg;
    public RectTransform buildButton;
    public RectTransform[] downButtons;
    private List<Vector2> downButtonsOrigins; //因爲按鈕還沒播完動畫再被Open的話 origin會偏移(動畫被打斷的位置變成新Origin)

    [Title("Tween-Left")] public Transform[] leftButtons;

    [Title("Tween-Right")] public RectTransform tip;

    [Title("TopRight")]
    [SerializeField] private TextMeshProUGUI nextRewardText;
    [SerializeField] private TextMeshProUGUI nextRewardLevelText;
    [SerializeField] private GameObject nextRewardChat;
    [SerializeField] private TextMeshProUGUI nextQuestText;
    [SerializeField] private GameObject nextQuestChat;
    
    [Title("CardCatNotifies")] 
    public Transform cardCatNotifyContent;

    [Title("Particle")]
    public UIParticle expParticle;
    public UIParticle moneyParticle;
    public UIParticle diamondParticle;

    [Title("Red")]
    public GameObject lobbyDailyQuestRed;
    public GameObject archiveRedPoint;
    public GameObject mailRedPoint;
    public GameObject mallRedPoint;
    public GameObject bagRedPoint;
    public GameObject catRedPoint;
    public GameObject friendRedPoint;

    public override void Open()
    {
        catCountText.text = App.system.cat.GetCats().Count.ToString("00");
        
        UIView.InstantShow();

        // Top
        topBg.DOLocalMoveY(0, 0.25f).SetEase(Ease.OutBack).From(300);

        // Down
        downBg.DOScale(1, 0.25f).SetEase(Ease.OutBack).From(1.5f);

        Vector2 buildButtonPos = buildButton.anchoredPosition + new Vector2(30, 30);
        for (int i = 0; i < downButtons.Length; i++)
            downButtons[i].DOAnchorPos(downButtonsOrigins[i], 0.25f).From(buildButtonPos).SetEase(Ease.OutBack).SetDelay(0.25f + i * 0.0625f);

        // Left
        for (int i = 0; i < leftButtons.Length; i++)
            leftButtons[i].DOLocalMoveX(45, 0.25f).SetEase(Ease.OutBack).From(-150).SetDelay(i * 0.0625f);
        
        // Right
        tip.DOAnchorPosX(-34f, 0.25f).SetEase(Ease.OutBack).From(new Vector2(tip.sizeDelta.x * 2 + 34f, tip.anchoredPosition.y));
        nextQuestChat.transform.DOScale(Vector2.one, 0.4f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1625f);
        nextRewardChat.transform.DOScale(Vector2.one, 0.4f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1625f);
        
        CheckRedActivate();
    }

    public override void Close()
    {
        UIView.InstantHide();
    }

    public override void Init()
    {
        base.Init();
        App.system.player.OnPlayerNameChange += OnPlayerNameChange;
        App.system.player.OnLevelChange += OnLevelChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
        App.system.player.OnUsingIconChange += OnUsingIconChange;
        App.system.player.OnUsingAvatarChange += OnUsingAvatarChange;
        
        App.system.player.OnReduceCoinChange += OnReduceCoinChange;
        App.system.player.OnReduceDiamondChange += OnReduceDiamondChange;

        App.system.room.OnRoomsChange += OnRoomsChange;
        
        App.model.dailyQuest.OnQuestsChange += OnQuestsChange;
        App.model.catGuide.OnCurrentLevelBestRewardChange += OnCurrentLevelBestRewardChange;
        
        App.model.lobby.OnTmpMoneyChange += OnTmpMoneyChange;
        App.model.lobby.OnTmpDiamondChange += OnTmpDiamondChange;
        App.model.lobby.OnTmpLevelChange += OnTmpLevelChange;
        
        #region TweenConfig

        downButtonsOrigins = new List<Vector2>();
        for (int i = 0; i < downButtons.Length; i++)
        {
            downButtonsOrigins.Add(downButtons[i].anchoredPosition);
        }

        #endregion
    }

    private void OnTmpLevelChange(object from, object to)
    {
        int beforeLevel = (int)from;
        int afterLevel = (int)to;
        
        DOVirtual.DelayedCall(2f, () =>
        {
            levelText.text = afterLevel.ToString("00");
        });
        
        DOVirtual.DelayedCall(2.5f, () =>
        {
            App.system.levelUp.Open(beforeLevel, afterLevel);
        });
    }

    private void OnTmpDiamondChange(object from, object to)
    {
        int diamond = (int)to;
        DOVirtual.DelayedCall(1.75f, () =>
        {
            diamondText.text = diamond.ToString();
        });
    }

    private void OnTmpMoneyChange(object from, object to)
    {
        int money = (int)to;
        DOVirtual.DelayedCall(1.75f, () =>
        {
            coinText.text = money.ToString();
        });
    }
    
    private void OnReduceDiamondChange(object value)
    {
        diamondText.text = App.system.player.Diamond.ToString();
    }

    private void OnReduceCoinChange(object value)
    {
        coinText.text = App.system.player.Coin.ToString();
    }

    private void OnUsingAvatarChange(object value)
    {
        string id = value.ToString();
        playerAvatar.sprite = App.factory.itemFactory.GetItem(id).icon;

        if (!App.factory.itemFactory.avatarEffects.ContainsKey(id))
            return;
        
        GameObject effectObject = App.factory.itemFactory.avatarEffects[id];
        Instantiate(effectObject, playerAvatar.transform);
    }

    private void OnUsingIconChange(object value)
    {
        string id = value.ToString();
        playerIcon.sprite = App.factory.itemFactory.GetItem(id).icon;
    }

    private void OnCurrentLevelBestRewardChange(object value)
    {
        var reward = (Reward)value;
        nextRewardText.text = reward.item.Name;
    }

    private void OnQuestsChange(object value)
    {
        var quests = (List<Quest>)value;

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].IsReach) continue;
            if (quests[i].IsReceived) continue;

            nextQuestChat.SetActive(true);
            Quest q = quests[i];
            nextQuestText.text = q.Content;
            return;
        }

        nextQuestChat.SetActive(false);
    }

    private void OnRoomsChange(object value)
    {
        List<Room> rooms = (List<Room>)value;
        
        int count = 0;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomData.roomType != RoomType.Features) continue;
            count++;
        }
        roomCountText.text = count.ToString("00");
    }

    private void OnPlayerNameChange(object value)
    {
        string playerName = Convert.ToString(value);
        playerNameText.text = playerName;
    }

    private void OnLevelChange(object value)
    {
        // 只要上線SetData用
        App.system.player.OnLevelChange -= OnLevelChange;
        
        int level = Convert.ToInt32(value);
        levelText.text = level.ToString("00");
        
        nextRewardChat.SetActive(level < 40);
        nextRewardLevelText.text = (level + 1).ToString();
    }

    private void OnCoinChange(object value)
    {
        // 只要上線SetData用
        App.system.player.OnCoinChange -= OnCoinChange;
        int coin = Convert.ToInt32(value);
        coinText.text = coin.ToString();
    }

    private void OnDiamondChange(object value)
    {
        // 只要上線SetData用
        App.system.player.OnDiamondChange -= OnDiamondChange;
        int diamond = Convert.ToInt32(value);
        diamondText.text = diamond.ToString();
    }

    public void SetExpFill(int nowExp, int nextExp)
    {
        if (nowExp >= nextExp)
        {
            expFill.fillAmount = 0;
            expFill.DOFillAmount(1f / nextExp * nowExp, 0.25f).SetEase(Ease.OutExpo).SetDelay(2f);
            return;
        }
        expFill.DOFillAmount(1f / nextExp * nowExp, 0.25f).SetEase(Ease.OutExpo).SetDelay(1.75f);
    }

    private void CheckRedActivate() // 檢查紅點有沒有在用
    {
        if (lobbyDailyQuestRed.activeSelf)
        {
            lobbyDailyQuestRed.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => lobbyDailyQuestRed.SetActive(true));
        }

        if (archiveRedPoint.activeSelf)
        {
            archiveRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => archiveRedPoint.SetActive(true));
        }

        if (mailRedPoint.activeSelf)
        {
            mailRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => mailRedPoint.SetActive(true));
        }

        if (mallRedPoint.activeSelf)
        {
            mallRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => mallRedPoint.SetActive(true));
        }
        
        if (bagRedPoint.activeSelf)
        {
            bagRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => bagRedPoint.SetActive(true));
        }
        
        if (catRedPoint.activeSelf)
        {
            catRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => catRedPoint.SetActive(true));
        }
        
        if (friendRedPoint.activeSelf)
        {
            friendRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => friendRedPoint.SetActive(true));
        }
    }
}