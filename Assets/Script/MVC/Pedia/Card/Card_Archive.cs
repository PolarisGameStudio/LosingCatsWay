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

    private string questId;
    
    public override void SetData(Quest quest)
    {
        base.SetData(quest);

        questId = quest.id;
        
        progressText.text = $"{quest.Progress}/{quest.TargetCount}";
        titleText.text = App.factory.stringFactory.GetQuestTitle(quest.id);
        descriptText.text = quest.Content;

        for (int i = 0; i < 3; i++)
            reachObjects[i].SetActive(false);
        
        string id = quest.id;
        var strings = id.Split('_');
        int index = Int32.Parse(strings[1]) - 2;

        if (index < 0)
            return;

        index++;

        for (int i = 0; i <= index; i++)
            reachObjects[i].SetActive(true);
        
        receiveMask.SetActive(index == 2 && quest.IsReceived);
    }

    public void GetReward()
    {
        int index = transform.GetSiblingIndex();
        App.controller.pedia.GetArchiveReward(index);
    }
}
