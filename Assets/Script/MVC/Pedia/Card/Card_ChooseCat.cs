using System;
using TMPro;
using UnityEngine;

public class Card_ChooseCat : MvcBehaviour
{
    public TextMeshProUGUI catTypeText;
    public CatSkin catSkin;
    public GameObject lockMask;
    public TextMeshProUGUI remainLigationText;

    public void SetData(string variety)
    {
        catTypeText.text = App.factory.stringFactory.GetCatVariety(variety);
        catSkin.ChangeSkin(variety);
        
        int level = App.system.quest.KnowledgeCardStatus[variety];

        if (level >= 3)
            catSkin.PlayAnimation();
        else
            catSkin.StopAnimation();

        if (level > 0)
        {
            lockMask.SetActive(false);
        }
        else
        {
            lockMask.SetActive(true);

            int count = App.system.quest.KnowledgeCardData[variety];
            int needCount = 1;

            remainLigationText.text = Math.Clamp(needCount - count, 0, 1).ToString();
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
        App.controller.pedia.UnlockPediaCat(index);
    }
}
