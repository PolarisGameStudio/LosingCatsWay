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
    }

    private void OnLevelChange(object value)
    {
        int level = (int)value;
        Reward[] rewards = App.factory.itemFactory.GetRewardsByLevel(level + 1);

        #region 貓居顯示下等級獎勵

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

        App.view.lobby.nextdLevelText.text = (level + 1).ToString();
        App.view.lobby.nextLevelBestItemText.text = bestItem.Name;

        #endregion

        #region 刷新可領取獎勵

        int receiveProgress = App.system.quest.QuestReceivedStatusData["LR001"];
        
        for (int i = 0; i < 40; i++)
        {
            int index = i + 1;
            var card = cards[i];
            card.SetData(index);
            
            bool isReceive = receiveProgress > i;
            bool isReach = index <= level;
            
            card.SetCanReceive(isReach && !isReceive);
            card.SetReceive(isReceive);
        }

        #endregion
        
        CheckLobbyRed();
    }
    
    public void CheckLobbyRed()
    {
        int receiveIndex = App.system.quest.QuestReceivedStatusData["LR001"];
        int level = App.system.player.Level;
        App.view.lobby.lobbyLevelRewardRed.SetActive(receiveIndex < level);
    }
}
