using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;

public class MyEvent7Days : MyEvent
{
    public GameObject[] masks;
    public List<Reward[]> Rewards;
    [SerializeField] private UIParticle[] uiParticles;

    public override void Open()
    {
        int receivedStatus = App.system.quest.QuestReceivedStatusData[id];

        for (int i = 0; i < receivedStatus; i++)
            masks[i].SetActive(true);

        for (int i = 0; i < uiParticles.Length; i++)
            uiParticles[i].gameObject.SetActive(false);

        uiParticles[receivedStatus].gameObject.SetActive(true);
        uiParticles[receivedStatus].Play();
    }

    public override void Init()
    {
    }

    public void Click()
    {
        int receivedStatus = App.system.quest.QuestReceivedStatusData[id];

        if (receivedStatus >= 7)
            return;

        int nowTotalDay = (Timestamp.GetCurrentTimestamp().ToDateTime() - new DateTime(1970,1,1)).Days;
        int lastTotalDay = App.system.quest.QuestProgressData[id];

        if (nowTotalDay - lastTotalDay > 0)
        {
            App.system.soundEffect.Play("ED00011");

            App.system.quest.QuestReceivedStatusData[id] = receivedStatus + 1;
            App.system.quest.QuestProgressData[id] = nowTotalDay;

            App.system.reward.Open(Rewards[receivedStatus]);
            masks[receivedStatus].SetActive(true);
            uiParticles[receivedStatus].gameObject.SetActive(false);
        }
    }
}