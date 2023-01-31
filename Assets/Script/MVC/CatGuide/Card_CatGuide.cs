using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Card_CatGuide : MvcBehaviour
{
    [SerializeField] private bool IsTopCard = false;
    
    [Title("Level")]
    [SerializeField] private TextMeshProUGUI levelText;
    [ShowIf("@IsTopCard"), SerializeField] private TextMeshProUGUI maskLevelText;
    
    [Title("Info")]
    [SerializeField] private GameObject[] infoObjects;
    [SerializeField] private Image[] infoIcons;
    [SerializeField] private TextMeshProUGUI[] infoTexts;
    [SerializeField] private Sprite[] iconSprites; //0:Unlock 1:Star

    [Title("Preview")] [SerializeField] private GameObject previewObject;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image previewIcon;
    
    [HideIf("IsTopCard")] [Title("Select")]
    [HideIf("IsTopCard")] [SerializeField] private GameObject selectObject;
    [HideIf("IsTopCard")] [SerializeField] private Image bgImage;
    [HideIf("IsTopCard")] [SerializeField] private Sprite[] bgSprites; //0:NoSelect 1:Selected
    
    [HideIf("IsTopCard")] [Title("Get")]
    [HideIf("IsTopCard")] [SerializeField] private GameObject getObject;
    [HideIf("IsTopCard")] [SerializeField] private Image topIcon;
    [HideIf("IsTopCard")] [SerializeField] private Sprite[] topIconSprites; //0:NoGet 1:Get

    [HideIf("IsTopCard")] [Title("TextColor")] [HideIf("IsTopCard")] [SerializeField]
    private Color32 normalColor;
    [SerializeField] private Color32 selectColor;
    
    public void SetData(Reward[] rewards)
    {
        if (rewards == null)
        {
            if (!IsTopCard) return;

            for (int i = 0; i < infoObjects.Length; i++)
            {
                infoObjects[i].SetActive(false);
            }
            infoObjects[0].SetActive(true);

            infoIcons[0].sprite = iconSprites[1];
            infoTexts[0].text = "敬請期待";
            
            previewObject.SetActive(false);
            
            return;
        }
        
        //Reward info
        for (int i = 0; i < infoObjects.Length; i++)
        {
            if (i > rewards.Length - 1)
            {
                infoObjects[i].SetActive(false);
                continue;
            }

            Reward reward = rewards[i];
            
            //Info text
            if (reward.item.itemType == ItemType.Unlock || reward.count <= 0) //Unlock
            {
                infoIcons[i].sprite = iconSprites[0];
                string content = reward.item.itemType == ItemType.Unlock
                    ? App.factory.stringFactory.GetUnlock(reward.item.id)
                    : reward.item.Name;
                infoTexts[i].text = content;
            }
            else
            {
                infoIcons[i].sprite = iconSprites[1];
                infoTexts[i].text = reward.item.Name;
            }

            infoObjects[i].SetActive(true);
        }
        
        //Best reward preview
        Reward best = new Reward();
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i].item.id is "ULK001" or "ULK002") //Unlock
            {
                best = rewards[i];
                break;
            }

            if (rewards[i].item.id.Contains("IRM")) //Rooms
            {
                best = rewards[i];
                break;
            }

            best = rewards[i];
        }

        App.model.catGuide.CurrentLevelBestReward = best; //TODO 解偶
        
        //Preview
        previewObject.SetActive(true);
        previewIcon.sprite = best.item.icon;
        countText.text = best.count.ToString();
        
        //TopIcon
        if (!IsTopCard)
            topIcon.sprite = topIconSprites[0];
            
        //Bg
        if (!IsTopCard)
            bgImage.sprite = bgSprites[0];
    }

    public void SetLevelRewards(Reward[] rewards) //todo 上面兩張單獨做
    {
        if (rewards == null) // 還沒有這個等級的獎勵
        {
            for (int i = 0; i < infoObjects.Length; i++)
                infoObjects[i].SetActive(false);
            
            infoObjects[0].SetActive(true);

            infoIcons[0].sprite = iconSprites[1];
            infoTexts[0].text = "敬請期待";
            
            previewObject.SetActive(false);
            
            return;
        }

        Dictionary<string, int> items = new Dictionary<string, int>();

        // 得到的東西
        for (int i = 0; i < rewards.Length; i++)
        {
            items.Add(rewards[i].item.id, rewards[i].count);
        }
        
        // 解鎖的東西
        List<Item> unlockItems = App.factory.itemFactory.GetUnlockItemsByLevel(App.system.player.Level);
        for (int i = 0; i < unlockItems.Count; i++)
        {
            items.Add(unlockItems[i].id, 0);
        }
        
        // 顯示在UI
        // for (int i = 0; i < infoObjects.Length; i++)
        // {
        //     
        // }
    }
    
    public void SetLevel(int level)
    {
        if (!IsTopCard)
            levelText.text = $"LV.{level:00}";
        else
        {
            levelText.text = level.ToString("00");
            maskLevelText.text = level.ToString("00");
        }
    }

    public void SetSelect(bool value)
    {
        if (IsTopCard)
        {
            for (int i = 0; i < infoIcons.Length; i++)
                infoIcons[i].color = normalColor;
            for (int i = 0; i < infoTexts.Length; i++)
                infoTexts[i].color = normalColor;
            return;
        }

        selectObject.SetActive(value);
        bgImage.sprite = value ? bgSprites[1] : bgSprites[0];

        Color32 color = value ? selectColor : normalColor;
        for (int i = 0; i < infoIcons.Length; i++)
            infoIcons[i].color = color;
        for (int i = 0; i < infoTexts.Length; i++)
            infoTexts[i].color = color;
    }

    public void IsGet(bool value)
    {
        if (IsTopCard) return;
        getObject.SetActive(value);
        topIcon.sprite = value ? topIconSprites[1] : topIconSprites[0];
    }

    public void DoFlip(float delay)
    {
        RectTransform rt = transform as RectTransform;
        rt.DOScaleY(1, 0);
        rt.DOScaleX(1, 0.25f).From(0).SetEase(Ease.OutExpo).SetDelay(delay);
    }
}
