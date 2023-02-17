using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class View_LevelReward : ViewBehaviour
{
    public CardLevelReward[] cards;
    [SerializeField] private Scrollbar scrollbar;

    private Vector2 tweenOrigin = new Vector2(1.1f, 1.1f);

    public override void Open()
    {
        base.Open();

        DOVirtual.DelayedCall(0.1f, () =>
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].scrollbar.value = 0;
                cards[i].transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack)
                    .SetDelay(0.12f * i);
            }
        });

        scrollbar.value = 1;
    }

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < cards.Length; i++)
            cards[i].transform.localScale = tweenOrigin;
    }

    public override void Init()
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].transform.localScale = tweenOrigin;
        
        base.Init();

        App.system.player.OnLevelChange += OnLevelChange;
        App.model.levelReward.OnMaxLevelChange += OnMaxLevelChange;
    }

    private void OnMaxLevelChange(object value)
    {
        int maxLevel = (int)value;
        int nowLevel = App.system.player.Level;
        int receiveProgress = App.system.quest.QuestReceivedStatusData["LR001"];
        
        for (int i = 0; i < maxLevel; i++)
        {
            int index = i + 1;
            var card = cards[i];
            card.SetData(index);
            
            bool isReceive = receiveProgress > i;
            bool isReach = index <= nowLevel;
            
            card.SetCanReceive(isReach && !isReceive);
            card.SetReceive(isReceive);
        }
    }

    private void OnLevelChange(object value)
    {
        int level = (int)value;
        Reward[] rewards = App.factory.itemFactory.GetRewardsByLevel(level);
        
        Item bestItem = null;
        for (int i = 0; i < rewards.Length; i++)
            if (rewards[i].item.id.Contains("IRM"))
            {
                bestItem = rewards[i].item;
                break;
            }

        if (bestItem == null)
            for (int i = 0; i < rewards.Length; i++)
                if (rewards[i].item.id.Contains("Diamond"))
                {
                    bestItem = rewards[i].item;
                    break;
                }
        
        if (bestItem == null)
            bestItem = App.factory.itemFactory.GetItem("Money");

        App.view.lobby.nextLevelBestItemText.text = bestItem.Name;
        
        CheckLobbyRed();
    }
    
    public void CheckLobbyRed()
    {
        int receiveIndex = App.system.quest.QuestReceivedStatusData["LR001"];
        int level = App.system.player.Level;
        App.view.lobby.lobbyLevelRewardRed.SetActive(receiveIndex < level);
    }
}
