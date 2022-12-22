using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailRewardUI : MvcBehaviour
{
    public Image roomImage;
    public TextMeshProUGUI roomCountText;

    public void SetData(Reward reward)
    {
        roomImage.sprite = reward.item.icon;
        roomCountText.text = "X" + reward.count;
    }
}
