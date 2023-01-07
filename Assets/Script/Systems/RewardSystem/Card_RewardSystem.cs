using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card_RewardSystem : MvcBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI countText;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private TextMeshProUGUI typeText;

    public void SetUI(Reward reward)
    {
        Item item = reward.item;

        icon.sprite = item.icon;
        nameText.text = item.Name;
        countText.text = "x" + reward.count;
        typeText.text = App.factory.stringFactory.GetItemTypeString(reward.item.itemType.ToString());
    }

    [Button]
    public void PlayParticle()
    {
        _particleSystem.Play();
    }
}
