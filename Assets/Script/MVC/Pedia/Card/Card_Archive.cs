using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_Archive : Card_Quest
{
    [Title("Archive")]
    [SerializeField] private GameObject[] reachObjects;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private GameObject receiveMask;
    [SerializeField] private TextMeshProUGUI rewardCountText;

    public override void SetData(Quest quest)
    {
        base.SetData(quest);

        titleText.text = App.factory.stringFactory.GetQuestTitle(quest.id);
        descriptText.text = quest.Content;
        progressText.text = $"{quest.Progress}/{quest.TargetCount}";
        rewardCountText.text = quest.Rewards[0].count.ToString();

        for (int i = 0; i < 3; i++)
            reachObjects[i].SetActive(false);
        
        string id = quest.id;
        var strings = id.Split('_');

        for (int i = 0; i < 3; i++)
        {
            string tmpId = strings[0] + "_" + (i + 1);
            bool isReach = App.factory.questFactory.GetQuestById(tmpId).IsReach;
            bool isReceived = App.factory.questFactory.GetQuestById(tmpId).IsReceived;
            if (isReach && !isReceived)
            {
                reachObjects[i].SetActive(true);
                break;
            }
        }

        string lastId = strings[0] + "_" + 3;
        var lastQuest = App.factory.questFactory.GetQuestById(lastId);
        bool isLastReceived = lastQuest.IsReceived;
        if (isLastReceived)
        {
            receiveMask.SetActive(true);
            reachObjects[2].SetActive(true);
            progressText.text = $"{lastQuest.TargetCount}/{lastQuest.TargetCount}";
        }
    }

    public void GetReward()
    {
        int index = transform.GetSiblingIndex();
        App.controller.pedia.GetArchiveReward(index);
    }
}
