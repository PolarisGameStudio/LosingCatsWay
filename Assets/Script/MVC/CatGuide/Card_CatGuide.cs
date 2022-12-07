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
            infoIcons[i].sprite = (reward.item.itemType == ItemType.Unlock) ? iconSprites[0] : iconSprites[1];
            infoTexts[i].text = (reward.item.itemType == ItemType.Unlock) ? App.factory.stringFactory.GetUnlock(reward.item.id) : reward.item.Name;
            infoObjects[i].SetActive(true);
        }
        
        //Best reward preview
        Reward best = new Reward();
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i].item.id == "ULK001" || rewards[i].item.id == "ULK002") //Unlock
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

    public void SetLevel(int level)
    {
        if (!IsTopCard)
            levelText.text = $"LV.{level:00}";
        else
            levelText.text = level.ToString("00");
    }

    public void SetSelect(bool value)
    {
        if (IsTopCard) return;
        selectObject.SetActive(value);
        bgImage.sprite = (value) ? bgSprites[1] : bgSprites[0];
    }

    public void IsGet(bool value)
    {
        if (IsTopCard) return;
        getObject.SetActive(value);
        topIcon.sprite = (value) ? topIconSprites[1] : topIconSprites[0];
    }

    public void DoFlip(float delay)
    {
        RectTransform rt = transform as RectTransform;
        rt.DOScaleY(1, 0);
        rt.DOScaleX(1, 0.25f).From(0).SetEase(Ease.OutExpo).SetDelay(delay);
    }
}
