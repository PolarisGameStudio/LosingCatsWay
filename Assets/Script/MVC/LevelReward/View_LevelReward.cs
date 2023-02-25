using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class View_LevelReward : ViewBehaviour
{
    public CardLevelReward[] cards;
    [SerializeField] private Scrollbar scrollbar;

    private Vector3 tweenOrigin = new Vector3(1.1f, 1.1f, 1.1f);

    public override void Open()
    {
        base.Open();

        DOVirtual.DelayedCall(0.1f, () =>
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].scrollbar.value = 0;
                cards[i].transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack)
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

        if (level >= 40)
            return;
        
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

        CheckCardsReceive();
        CheckLobbyRed();
    }
    
    public void CheckLobbyRed()
    {
        int receiveIndex = App.system.quest.QuestReceivedStatusData["LR001"];
        int level = App.system.player.Level;
        App.view.lobby.lobbyLevelRewardRed.SetActive(receiveIndex < level);
    }
    
    public void CheckCardsReceive()
    {
        int level = App.system.player.Level;
        int receiveProgress = App.system.quest.QuestReceivedStatusData["LR001"];

        for (int i = 0; i < App.view.levelReward.cards.Length; i++)
        {
            var card = App.view.levelReward.cards[i];
            
            if (i < receiveProgress)
            {
                card.SetReceive(true);
                card.SetCanReceive(false);
                continue;
            }
            
            card.SetReceive(false);

            if (i < level)
            {
                card.SetCanReceive(true);
                continue;
            }

            card.SetCanReceive(false);
        }
    }
}
