using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_DailyQuest : ViewBehaviour
{
    public Card_DailyQuest[] card_DailyQuests;

    [Title("TotalQuest")] [SerializeField] private TextMeshProUGUI totalContent;
    [SerializeField] private TextMeshProUGUI totalRewardText;
    [SerializeField] private TextMeshProUGUI totalCountText;
    [SerializeField] private Image totalFill;
    [SerializeField] private GameObject totalQuestRed;
    [SerializeField] private GameObject totalMask;
    [SerializeField] private GameObject receiveMask;

    [Title("UI")] [SerializeField] private Transform bg;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject getAllMask;

    [Title("Effects")] [SerializeField] private ParticleSystem totalReceiveEffects;

    [Title("DoTween")] [SerializeField] private RectTransform[] cardRects;
    [SerializeField] private CanvasGroup[] cardCanvasGroups;
    [SerializeField] private RectTransform cardOrigin;

    [Title("View")] public View_DailyQuestWatchAd watchAd;
    
    public override void Init()
    {
        base.Init();
        App.model.dailyQuest.OnQuestsChange += OnQuestsChange;
        App.model.dailyQuest.OnTotalQuestsChange += OnTotalQuestsChange;
    }

    public override void Open()
    {
        UIView.InstantShow();
        bg.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo);

        for (int i = 0; i < cardRects.Length; i++)
        {
            RectTransform rt = cardRects[i].transform as RectTransform;
            Vector2 end = rt.anchoredPosition;
            cardCanvasGroups[i].alpha = 0;

            rt.DOAnchorPos(cardOrigin.anchoredPosition, 0);
            rt.DOAnchorPos(end, 0.25f).SetEase(Ease.OutSine).SetEase(Ease.OutBack).SetDelay(0.1f * i);
            cardCanvasGroups[i].DOFade(1, 0.25f).SetEase(Ease.InCubic).SetDelay(0.1f * i);
        }
    }

    public void OnQuestsChange(object value)
    {
        var quests = (List<Quest>)value;
        
        if (quests.Count <= 0)
            return;

        int count = 0;
        int receiveCount = 0;

        for (int i = 0; i < quests.Count; i++)
        {
            Quest quest = quests[i];

            if (quest.IsReach && !quest.IsReceived)
            {
                red.SetActive(true);
                continue;
            }

            if (quest.IsReceived)
            {
                receiveCount++;
                red.SetActive(false);
            }
        }
        
        for (int i = 0; i < card_DailyQuests.Length; i++)
        {
            var quest = App.model.dailyQuest.Quests[i];
            card_DailyQuests[i].SetData(quest);

            if (!quest.IsReach) 
                continue;
            
            if (quest.IsReceived)
            {
                card_DailyQuests[i].PlayGetTween();
                continue;
            }

            count++;
        }

        // TODO 月卡
        getAllMask.SetActive(count <= 0);

        totalCountText.text = $"{receiveCount}/{quests.Count}";
        totalFill.fillAmount = (float)receiveCount / quests.Count;
    }

    public void OnTotalQuestsChange(object value)
    {
        var quest = App.model.dailyQuest.TotalQuest;

        totalContent.text = quest.Content;
        totalRewardText.text = quest.Rewards[0].count.ToString();
        // totalFill.fillAmount = (float)quest.Progress / quest.TargetCount;

        receiveMask.SetActive(!quest.IsReach);
        totalMask.SetActive(quest.IsReceived);
        totalQuestRed.SetActive(!quest.IsReach && quest.IsReceived);

        if (quest.IsReach && !quest.IsReceived)
        {
            totalReceiveEffects.gameObject.SetActive(true);
            if (!totalReceiveEffects.isPlaying)
                totalReceiveEffects.Play();
        }
        else //省效能
        {
            totalReceiveEffects.gameObject.SetActive(false);
            if (totalReceiveEffects.isPlaying)
                totalReceiveEffects.Stop();
        }
    }
}