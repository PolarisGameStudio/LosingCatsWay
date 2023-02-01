using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_LevelReward_Top : MvcBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI secondColorLevelText;
    
    [Title("Items")] 
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Sprite unlockSprite;
    [SerializeField] private Sprite rewardSprite;
    [SerializeField] private TextMeshProUGUI[] itemTexts;
    
    [Title("BestReward")] 
    [SerializeField] private Image bestImage;
    [SerializeField] private TextMeshProUGUI bestCountText;
    [SerializeField] private GameObject bestCountObject;
    
    public void SetData(int level, bool waitUpdateVersion = false)
    {
        if (waitUpdateVersion) // 敬請期待
        {
            levelText.text = level.ToString("00");
            secondColorLevelText.text = level.ToString("00");

            for (int i = 0; i < itemTexts.Length; i++)
                itemTexts[i].gameObject.SetActive(false);
            for (int i = 0; i < itemIcons.Length; i++)
                itemIcons[i].gameObject.SetActive(false);

            itemIcons[0].sprite = rewardSprite;
            itemIcons[0].gameObject.SetActive(true);
            itemTexts[0].text = "TBA";
            itemTexts[0].gameObject.SetActive(true);

            bestImage.gameObject.SetActive(false);
            bestCountObject.SetActive(false);
            
            return;
        }
        
        Reward[] rewards = App.factory.itemFactory.GetRewardsByLevel(level);
        List<Item> unlockItems = App.factory.itemFactory.GetUnlockItemsByLevel(level);
        
        // 顯示這張卡的等級
        levelText.text = level.ToString("00");
        secondColorLevelText.text = level.ToString("00");

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

        bestImage.gameObject.SetActive(true);
        bestImage.sprite = bestItem.icon;
        bestCountText.text = bestCount.ToString("00");
        bestCountObject.SetActive(bestCount > 0);
    }
}
