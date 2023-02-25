using System;
using TMPro;
using UnityEngine;

public class Card_ChooseCat : MvcBehaviour
{
    public TextMeshProUGUI catTypeText;
    public CatSkin catSkin;
    public GameObject lockMask;
    public TextMeshProUGUI remainLigationText;
    [SerializeField] private GameObject isUnlockEffect;
    [SerializeField] private GameObject isUnlockEffect02;
    public GameObject clickUnlockEffect;

    public void SetData(string variety)
    {
        catTypeText.text = App.factory.stringFactory.GetCatVariety(variety);
        catSkin.ChangeSkin(variety);
        
        int level = App.system.quest.KnowledgeCardStatus[variety];
        int count = App.system.quest.KnowledgeCardData[variety];

        clickUnlockEffect.SetActive(false);

        if (count >= 10)
            catSkin.PlayAnimation();
        else
            catSkin.StopAnimation();

        if (level > 0)
        {
            lockMask.SetActive(false);
            isUnlockEffect.SetActive(false);
            isUnlockEffect02.SetActive(false);
        }
        else
        {
            lockMask.SetActive(true);

            int needCount = 1;
            remainLigationText.text = Math.Clamp(needCount - count, 0, 1).ToString();
            
            isUnlockEffect.SetActive(needCount - count <= 0);
            isUnlockEffect02.SetActive(needCount - count <= 0);
        }
    }

    public void Click()
    {
        int index = transform.GetSiblingIndex();
        App.controller.pedia.ChoosePediaCat(index);
    }

    public void Unlock()
    {
        int index = transform.GetSiblingIndex();
        if (!App.controller.pedia.UnlockPediaCat(index))
            return;
        clickUnlockEffect.SetActive(true);
    }
}
