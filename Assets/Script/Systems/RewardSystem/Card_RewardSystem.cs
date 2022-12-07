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

    public void SetUI(Reward reward)
    {
        Item item = reward.item;

        icon.sprite = item.icon;
        nameText.text = item.Name;
        countText.text = "x" + reward.count;
    }

    [Button]
    public void PlayParticle()
    {
        _particleSystem.Play();
    }
}
