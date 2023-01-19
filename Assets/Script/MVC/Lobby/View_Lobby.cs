using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public override void Open()
    {
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
        App.system.player.OnExpChange += OnExpChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
        App.system.player.OnUsingIconChange += OnUsingIconChange;
        App.system.player.OnUsingAvatarChange += OnUsingAvatarChange;

        App.system.cat.OnCatsChange += OnCatsChange;
        App.system.room.OnRoomsChange += OnRoomsChange;
        
        App.model.dailyQuest.OnQuestsChange += OnQuestsChange;
        App.model.catGuide.OnCurrentLevelBestRewardChange += OnCurrentLevelBestRewardChange;

        #region TweenConfig

        downButtonsOrigins = new List<Vector2>();
        for (int i = 0; i < downButtons.Length; i++)
        {
            downButtonsOrigins.Add(downButtons[i].anchoredPosition);
        }

        #endregion
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

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;
        catCountText.text = cats.Count.ToString("00");
    }

    private void OnPlayerNameChange(object value)
    {
        string playerName = Convert.ToString(value);
        playerNameText.text = playerName;
    }

    private void OnLevelChange(object value)
    {
        int level = Convert.ToInt32(value);
        levelText.text = level.ToString("00");
        
        nextRewardChat.SetActive(level < 40);
        nextRewardLevelText.text = (level + 1).ToString();
    }

    private void OnExpChange(object from, object to)
    {
        int lastExp = Convert.ToInt32(from);
        int newExp = Convert.ToInt32(to);
        int fullExp = App.system.player.NextLevelExp;

        expFill.fillAmount = 1f / fullExp * lastExp;
        expFill.DOFillAmount(1f / fullExp * newExp, .3f);
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