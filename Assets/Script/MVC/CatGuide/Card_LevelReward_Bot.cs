using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_LevelReward_Bot : MvcBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject getObject;
    [SerializeField] private Image topIcon;
    [SerializeField] private Sprite isGetTopIcon;
    [SerializeField] private Sprite noGetTopIcon;
    
    [Title("Items")] 
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Sprite unlockSprite;
    [SerializeField] private Sprite rewardSprite;
    [SerializeField] private TextMeshProUGUI[] itemTexts;

    [Title("BestReward")] 
    [SerializeField] private Image bestImage;
    [SerializeField] private TextMeshProUGUI bestCountText;
    [SerializeField] private GameObject bestCountObject;
    
    [Title("Select")]
    [SerializeField] private GameObject selectObject;
    [SerializeField] private Image bg;
    [SerializeField] private Sprite noSelectBg;
    [SerializeField] private Sprite selectBg;

    [Title("Color")]
    [SerializeField] private Color32 selectColor;
    [SerializeField] private Color32 normalColor;

    public void SetData(int level)
    {
        Reward[] rewards = App.factory.itemFactory.GetRewardsByLevel(level);
        List<Item> unlockItems = App.factory.itemFactory.GetUnlockItemsByLevel(level);
        
        // 顯示這張卡的等級
        levelText.text = $"LV.{level:00}";

        Queue<TextMeshProUGUI> texts = new Queue<TextMeshProUGUI>(itemTexts);
        Queue<Image> icons = new Queue<Image>(itemIcons);
        
        // 顯示獎勵
        for (int i = 0; i < rewards.Length; i++)
        {
            if (icons.Count <= 0)
                break;
            if (texts.Count <= 0)
                break;

            var tmpIcon = icons.Dequeue();
            tmpIcon.sprite = rewardSprite;
            tmpIcon.gameObject.SetActive(true);
            
            var tmpReward = rewards[i];
            var tmpText = texts.Dequeue();
            tmpText.text = tmpReward.item.Name;
            tmpText.gameObject.SetActive(true);
        }

        // 顯示解鎖
        for (int i = 0; i < unlockItems.Count; i++)
        {
            if (icons.Count <= 0)
                break;
            if (texts.Count <= 0)
                break;
            
            var tmpIcon = icons.Dequeue();
            tmpIcon.sprite = unlockSprite;
            tmpIcon.gameObject.SetActive(true);
            
            var tmpUnlock = unlockItems[i];
            var tmpText = texts.Dequeue();
            tmpText.text = tmpUnlock.Name;
            tmpText.gameObject.SetActive(true);
        }

        // 剩下的字關掉
        while (icons.Count > 0)
            icons.Dequeue().gameObject.SetActive(false);
        while (texts.Count > 0)
            texts.Dequeue().gameObject.SetActive(false);
        
        // 最好的獎勵
        Item bestItem = null;
        int bestCount = -1;

        if (unlockItems.Count > 0) //有解鎖
        {
            // 先找解鎖的
            for (int i = 0; i < unlockItems.Count; i++)
            {
                if (unlockItems[i].id.Contains("ULK"))
                {
                    bestItem = unlockItems[i];
                    break;
                }
            }

            // 沒有的話再找房間的
            if (bestItem == null)
            {
                for (int i = 0; i < unlockItems.Count; i++)
                {
                    if (unlockItems[i].id.Contains("IRM"))
                    {
                        bestItem = unlockItems[i];
                        break;
                    }
                }
            }
            
            // 再沒有就第一個
            if (bestItem == null)
                bestItem = unlockItems[0];
        }
        else
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                if (rewards[i].item.id.Contains("IRM"))
                {
                    bestItem = rewards[i].item;
                    bestCount = rewards[i].count;
                }

                bestItem = rewards[i].item;
                bestCount = rewards[i].count;
                break;
            }
        }

        bestImage.sprite = bestItem.icon;
        bestCountText.text = bestCount.ToString("00");
        bestCountObject.SetActive(bestCount > 0);
    }

    public void SetSelect(bool value)
    {
        selectObject.SetActive(value);
        bg.sprite = value ? selectBg : noSelectBg;
        
        topIcon.sprite = value ? isGetTopIcon : noGetTopIcon;

        Color32 color = value ? selectColor : normalColor;
        for (int i = 0; i < itemIcons.Length; i++)
            itemIcons[i].color = color;
        for (int i = 0; i < itemTexts.Length; i++)
            itemTexts[i].color = color;
    }

    public void SetIsGet(bool value)
    {
        getObject.SetActive(value);
    }

    public void DoFlip(float delay)
    {
        RectTransform rt = transform as RectTransform;
        rt.DOScaleY(1, 0);
        rt.DOScaleX(1, 0.25f).From(0).SetEase(Ease.OutExpo).SetDelay(delay);
    }
}
