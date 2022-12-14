using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_MonthSign : MvcBehaviour
{
    [SerializeField] private GameObject signMark;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image icon;

    bool isSign;

    public void SetDate(int date)
    {
        dateText.text = date.ToString("00");
    }

    public void SetReward(Reward reward)
    {
        if (reward == null) return;

        icon.sprite = reward.item.icon;
        countText.text = $"X{reward.count:00}";
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public bool IsSign
    {
        get => isSign;
        set
        {
            isSign = value;
            signMark.SetActive(value);
        }
    }
}
