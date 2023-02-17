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
    [SerializeField] private TextMeshProUGUI colorLevelText;
    [SerializeField] private GameObject receiveMask;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI noUnlockText;
    
    [Title("IsSelect")]
    [SerializeField] private Image bg;
    [SerializeField] private Sprite noSelectBg;
    [SerializeField] private Sprite selectBg;
    [SerializeField] private GameObject normalUnlockTitle;
    [SerializeField] private GameObject selectUnlockTitle;
    [SerializeField] private GameObject normalRewardTitle;
    [SerializeField] private GameObject selectRewardTitle;
    
    [Title("Color")]
    [SerializeField] private Color32 selectTextColor;
    [SerializeField] private Color32 normalTextColor;
    
    [Title("Unlock")]
    [SerializeField] private TextMeshProUGUI[] unlockTexts;

    [Title("Receive")]
    [SerializeField] private Transform content;
    [SerializeField] private LevelRewardObject levelRewardObject;
    public Scrollbar scrollbar;

    public void SetData(int level)
    {
        Item[] unlocks = App.factory.itemFactory.GetUnlocksByLevel(level);
        Reward[] receives = App.factory.itemFactory.GetRewardsByLevel(level);

        noUnlockText.gameObject.SetActive(unlocks.Length <= 0);
        
        for (int i = 0; i < unlockTexts.Length; i++)
        {
            if (i >= unlocks.Length)
            {
                unlockTexts[i].gameObject.SetActive(false);
                continue;
            }

            unlockTexts[i].gameObject.SetActive(true);
            string key = GetUnlockKey(unlocks[i].id);
            string unlockHead = App.factory.stringFactory.GetUnlockHead(key);
            unlockTexts[i].text = unlockHead + unlocks[i].Name;
        }

        for (int i = 0; i < receives.Length; i++)
        {
            var tmp = Instantiate(levelRewardObject, content);
            tmp.SetData(receives[i]);
        }
        
        levelText.text = level.ToString("00");
        colorLevelText.text = level.ToString("00");

        scrollbar.value = 0;
    }

    public void SetCanReceive(bool value)
    {
        button.interactable = value;
        
        bg.sprite = value ? selectBg : noSelectBg;
        bg.transform.localScale = value ? new Vector2(1.04f, 1.04f) : Vector2.one;
        
        selectUnlockTitle.SetActive(value);
        normalUnlockTitle.SetActive(!value);
        
        selectRewardTitle.SetActive(value);
        normalRewardTitle.SetActive(!value);

        Color32 textColor = value ? selectTextColor : normalTextColor;
        noUnlockText.color = textColor;
        for (int i = 0; i < unlockTexts.Length; i++)
            unlockTexts[i].color = textColor;
    }

    public void SetReceive(bool value)
    {
        receiveMask.SetActive(value);
        canvasGroup.alpha = value ? 0.8f : 1f;
    }

    public void ReceiveReward()
    {
        App.controller.levelReward.ReceiveReward();
    }
    
    private string GetUnlockKey(string id)
    {
        if (id.Contains("IRM"))
            return "ULK003";
        if (id.Contains("ICL"))
            return "ULK005";
        return id;
    }
}
