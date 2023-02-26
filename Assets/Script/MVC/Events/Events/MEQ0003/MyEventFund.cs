using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventFund : MyEvent
{
    public Quest[] quests;
    public MEQ0003_Card[] cards;

    public override void Open()
    {
        RefreshUI();
    }

    public override void Init()
    {
    }

    public override bool CheckRedPoint()
    {
        return !quests[0].IsReceived;
    }

    public void Recive(int index)
    {
        if (quests[index].IsReceived)
            return;

        quests[index].IsReceived = true;
        App.system.reward.Open(quests[index].Rewards);
        RefreshUI();
        App.controller.events.RefreshRedPoint();
    }

    private void RefreshUI()
    {
        Item syb0001 = App.factory.itemFactory.GetItem("SYB0001");

        if (syb0001.Count > 0)
        {
            bool flag = quests[0].IsReceived;
            cards[0].SetData(true, flag);
        }
        else
        {
            cards[0].SetData(false, false);
        }
    }
}