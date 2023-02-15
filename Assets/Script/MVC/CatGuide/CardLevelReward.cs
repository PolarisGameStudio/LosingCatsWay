using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelReward : MvcBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject receiveMask;
    [SerializeField] private GameObject noReceiveIcon;
    [SerializeField] private GameObject isReceiveIcon;
    [SerializeField] private Scrollbar scrollbar;
    
    [Title("IsSelect")]
    [SerializeField] private GameObject outlineObject;
    [SerializeField] private Image bg;
    [SerializeField] private Sprite noSelectBg;
    [SerializeField] private Sprite selectBg;
    [SerializeField] private Color32 selectColor = Color.white;
    [SerializeField] private Color32 noSelectColor = Color.white;
    
    [Title("Unlock")]
    [SerializeField] private GameObject unlockParent;
    [SerializeField] private GameObject[] unlockObjects;
    [SerializeField] private TextMeshProUGUI[] unlockTexts;
    [SerializeField] private Image[] unlockIcons;

    [Title("Receive")]
    [SerializeField] private GameObject receiveParent;
    [SerializeField] private GameObject[] receiveObjects;
    [SerializeField] private TextMeshProUGUI[] receiveTexts;
    [SerializeField] private Image[] receiveIcons; // 文字旁邊的icon
    [SerializeField] private GameObject[] receivePreviewObjects;
    [SerializeField] private Image[] receivePreviews; // 預覽
    [SerializeField] private TextMeshProUGUI[] receiveCountTexts; // 預覽數量
    
    public void SetData(int level)
    {
        Item[] unlocks = App.factory.itemFactory.GetUnlocksByLevel(level);
        Reward[] receives = App.factory.itemFactory.GetRewardsByLevel(level);

        unlockParent.SetActive(unlocks.Length > 0);
        receiveParent.SetActive(receives.Length > 0);
        
        for (int i = 0; i < unlockObjects.Length; i++)
        {
            if (i >= unlocks.Length)
            {
                unlockObjects[i].SetActive(false);
                continue;
            }

            unlockObjects[i].SetActive(true);
            unlockTexts[i].text = unlocks[i].Name;
        }

        for (int i = 0; i < receiveObjects.Length; i++)
        {
            if (i >= receives.Length)
            {
                receiveObjects[i].SetActive(false);
                receivePreviewObjects[i].SetActive(false);
                continue;
            }

            receiveObjects[i].SetActive(true);
            receiveTexts[i].text = receives[i].item.Name;
            
            receivePreviewObjects[i].SetActive(receives[i].item.id.Contains("IRM"));
            receivePreviews[i].sprite = receives[i].item.icon;
            receiveCountTexts[i].text = receives[i].count.ToString();
        }
        
        levelText.text = $"LV.{level:00}";
        int currentLevel = App.system.player.Level;

        receiveMask.SetActive(level <= currentLevel);
        SetSelect(level == currentLevel + 1);

        scrollbar.value = 1;
    }

    private void SetSelect(bool value)
    {
        outlineObject.SetActive(value);
        bg.sprite = value ? selectBg : noSelectBg;
        
        isReceiveIcon.SetActive(value);
        noReceiveIcon.SetActive(!value);

        Color32 color = value ? selectColor : noSelectColor;
        for (int i = 0; i < unlockIcons.Length; i++)
            unlockIcons[i].color = color;
        for (int i = 0; i < unlockTexts.Length; i++)
            unlockTexts[i].color = color;
        for (int i = 0; i < receiveIcons.Length; i++)
            receiveIcons[i].color = color;
        for (int i = 0; i < receiveTexts.Length; i++)
            receiveTexts[i].color = color;
    }
    
    public void DoFlip(float delay)
    {
        RectTransform rt = transform as RectTransform;
        rt.DOScaleY(1, 0);
        rt.DOScaleX(1, 0.25f).From(0).SetEase(Ease.OutExpo).SetDelay(delay);
    }
}
