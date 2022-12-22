using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card_Quest : MvcBehaviour
{
    [Title("RewardIcon")]
    public Image rewardIcon;
    public TextMeshProUGUI rewardText;

    [Title("Title")] //任務圖示
    public TextMeshProUGUI titleText; //標題字

    public virtual void SetData(Quest quest)
    {
        if (rewardIcon != null)
            rewardIcon.sprite = quest.Rewards[0].item.icon;
        if (titleText != null)
            titleText.text = quest.Content;
        if (rewardText != null)
            rewardText.text = quest.Rewards[0].count.ToString();
    }
}
