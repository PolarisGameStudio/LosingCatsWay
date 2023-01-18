using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Pedia : ControllerBehavior
{
    [SerializeField] private Button pediaLeftArrow;
    [SerializeField] private Button pediaRightArrow;
    
    public void Init()
    {
        ArchiveInit();
    }

    public void SelectTab(int index)
    {
        App.model.pedia.TabIndex = index;
        
        if (index == 0)
            OpenArchive();
        else if (index == 1)
            OpenPediaCats();
        else
            OpenPedia();
    }

    public void PediaToLeft()
    {
        switch (App.model.pedia.TabIndex)
        {
            case 0:
                App.model.pedia.ArchivePageIndex--;
                break;
            case 1:
                break;
            case 2:
                App.model.pedia.PediaPageIndex -= 1;
                RefreshPediaItems();
                break;
        }
    }

    public void PediaToRight()
    {
        switch (App.model.pedia.TabIndex)
        {
            case 0:
                App.model.pedia.ArchivePageIndex++;
                break;
            case 1:
                break;
            case 2:
                App.model.pedia.PediaPageIndex += 1;
                RefreshPediaItems();
                break;
        }
    }
    
    #region Pedia
    
    private void OpenPedia()
    {
        CloseArchive();
        ClosePediaCats();
        
        App.view.pedia.Open();
        App.view.pedia.subPedia.Open();
        CloseChoosePedia();

        DOVirtual.DelayedCall(0.1f, () => { SelectPediaType(0); });
        App.model.pedia.PediaPageIndex = 0;
        RefreshPediaItems();
    }

    private void ClosePedia()
    {
        App.view.pedia.subPedia.Close();
        SelectPediaType(-1);
    }

    public void Close()
    {
        if (!App.view.pedia.IsVisible)
            return;
        
        ClosePedia();
        CloseArchive();
        App.view.pedia.Close();
    }

    public void SelectPediaType(int index)
    {
        if (App.model.pedia.SelectedPediaType == index)
            return;

        CloseReadPedia();
        App.model.pedia.SelectedPediaType = index;
        App.model.pedia.PediaPageIndex = 0;
        RefreshPediaItems();
        OpenChoosePedia();
    }

    public void OpenChoosePedia()
    {
        App.view.pedia.subPedia.choosePedia.Open();
    }

    public void CloseChoosePedia()
    {
        App.view.pedia.subPedia.choosePedia.Close();
    }

    public void ChoosePedia(int index)
    {
        App.model.pedia.SelectedPediaId = App.model.pedia.UsingPediaIds[index];
        OpenReadPedia();
    }

    public void UnlockPedia(int index)
    {
        Item klc0001 = App.factory.itemFactory.GetItem("KLC0001");

        if (klc0001.Count < 10)
        {
            App.system.confirm.Active(ConfirmTable.Fix);
            return;
        }
        
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            string pediaId = App.model.pedia.UsingPediaIds[index];
            App.system.inventory.KnowledgeCardDatas[pediaId]++;
            klc0001.Count -= 10;
            RefreshPediaItems();
        });
    }

    public void OpenReadPedia()
    {
        pediaLeftArrow.gameObject.SetActive(false);
        pediaRightArrow.gameObject.SetActive(false);
        CloseChoosePedia();
        App.view.pedia.subPedia.readPedia.Open();
    }

    public void CloseReadPedia()
    {
        App.view.pedia.subPedia.readPedia.Close();
        RefreshPediaItems();
        OpenChoosePedia();
    }

    private void RefreshPediaItems()
    {
        int type = App.model.pedia.SelectedPediaType;
        int index = App.model.pedia.PediaPageIndex;
        List<string> tmp = App.factory.pediaFactory.GetPediaIds(type);
        
        if (index < 0)
            index = 0;

        int end = Mathf.CeilToInt(tmp.Count / 8f);
        if (index > end)
            index = end;

        pediaLeftArrow.gameObject.SetActive(true);
        pediaRightArrow.gameObject.SetActive(true);
        
        pediaLeftArrow.interactable = index > 0;
        pediaRightArrow.interactable = index < end - 1;
        
        List<string> result = new List<string>();
        for (int i = index * 8; i < index * 8 + 8; i++)
        {
            if (i >= tmp.Count)
                break;
            
            result.Add(tmp[i]);
        }

        App.model.pedia.UsingPediaIds = result;
    }
    
    #endregion

    #region Archive

    private void OpenArchive()
    { 
        App.model.pedia.ArchiveQuests = App.model.pedia.ArchiveQuests;
        
        ClosePedia();
        ClosePediaCats();
        
        App.view.pedia.Open();
        App.view.pedia.archive.Open();

        DOVirtual.DelayedCall(0.1f, () => { SelectArchiveType(0); });
        App.model.pedia.ArchivePageIndex = 0;

        pediaLeftArrow.gameObject.SetActive(false);
        pediaRightArrow.gameObject.SetActive(false);
    }

    private void CloseArchive()
    {
        App.view.pedia.archive.Close();
        SelectArchiveType(-1);
    }

    public void SelectArchiveType(int index)
    {
        if (App.model.pedia.SelectedArchiveType == index)
            return;

        App.model.pedia.SelectedArchiveType = index;
    }

    public void GetArchiveReward(int index)
    {
        Quest quest = App.model.pedia.ArchiveQuests[index];

        if (!quest.IsReach)
            return;

        if (quest.IsReceived)
            return;

        // quest.IsReceived = true;
        var strings = quest.id.Split('_');
        App.system.quest.QuestReceivedStatusData[strings[0]] += 1;
        
        App.system.reward.Open(quest.Rewards);
        ArchiveInit();
    }

    private void ArchiveInit()
    {
        ClearAchieveQuests();

        List<Quest> quests = new List<Quest>();

        for (int i = 1; i <= 8; i++)
        {
            string id = "ACR000" + i;
            Quest quest = GetAchieveQuest(id);
            quest.Init();
            quests.Add(quest);

            // if (quest.IsReceived)
            //     quest.Init();
        }

        App.model.pedia.ArchiveQuests = quests;
    }

    private void ClearAchieveQuests()
    {
        List<Quest> prevQuests = App.model.pedia.ArchiveQuests;

        if (prevQuests == null)
            return;

        for (int i = 0; i < prevQuests.Count; i++)
            prevQuests[i].CancelBind();
    }

    private Quest GetAchieveQuest(string id)
    {
        Quest result = null;

        for (int i = 1; i < 4; i++)
        {
            result = App.factory.questFactory.AchieveQuests[id + "_" + i];

            if (!result.IsReceived)
                return result;
        }

        return result;
    }

    #endregion

    #region PediaCats

    private void OpenPediaCats()
    {
        ClosePedia();
        CloseArchive();
        
        App.view.pedia.pediaCats.Open();
    }

    private void ClosePediaCats()
    {
        App.view.pedia.pediaCats.Close();
    }

    #endregion
}