using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_DailyQuest : Card_Quest
{
    [Title("Exp")]
    [SerializeField] private TextMeshProUGUI expText;
    
    [Title("Progress")]
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject red;
    [SerializeField] private TextMeshProUGUI progressBarText;

    [Title("Cover")] 
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject receiveMask;
    [SerializeField] private MyTween_Scale getTween;

    public override void SetData(Quest quest)
    {
        base.SetData(quest);

        progressBar.fillAmount = (float) quest.Progress / quest.TargetCount;
        progressBarText.text = quest.Progress + "/" + quest.TargetCount;
        expText.text = "+" + quest.exp;

        receiveMask.SetActive(!quest.IsReach);
        mask.SetActive(quest.IsReceived);
        red.SetActive(!quest.IsReceived && quest.IsReach);
    }

    public void GetReward(int index)
    {
        App.controller.dailyQuest.OnGetReward += PlayGetTween;
        App.controller.dailyQuest.OnGetAdsReward += PlayGetTween;
        App.controller.dailyQuest.SelectReward(index);
    }

    private void PlayGetTween()
    {
        mask.SetActive(true);
        getTween.delay = 0.2f;
        getTween.Play();
    }
}
