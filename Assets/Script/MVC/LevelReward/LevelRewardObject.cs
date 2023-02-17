using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelRewardObject : MvcBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetData(Reward reward)
    {
        icon.sprite = reward.item.icon;
        countText.text = reward.count.ToString();
    }
}
