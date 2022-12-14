using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Controller_Pedia : ControllerBehavior
{
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    
    private bool isPedia;
    
    public void Init()
    {
        ArchiveInit();
    }

    public void ToLeft()
    {
        if (!IsCanToLeft())
            return;
        
        if (isPedia)
        {
            App.model.pedia.PediaPageIndex -= 1;
            RefreshPediaItems();
        }
        else
            App.model.pedia.ArchivePageIndex--;
    }

    public void ToRight()
    {
        if (!IsCanToRight())
            return;
        
        if (isPedia)
        {
            App.model.pedia.PediaPageIndex += 1;
            RefreshPediaItems();
        }
        else
            App.model.pedia.ArchivePageIndex++;
    }
    
    #region Pedia
    
    public void OpenPedia()
    {
        isPedia = true;
        
        CloseArchive();
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

    public void OpenReadPedia()
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);

        App.view.pedia.subPedia.readPedia.Open();
        App.model.pedia.SelectedPediaType = App.model.pedia.SelectedPediaType; // 跳動
    }

    public void CloseReadPedia()
    {
        App.view.pedia.subPedia.readPedia.Close();
        RefreshPediaItems();
        App.model.pedia.SelectedPediaType = App.model.pedia.SelectedPediaType; // 跳動
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

        leftArrow.SetActive(index > 0);
        rightArrow.SetActive(index < end - 1);
        
        List<string> result = new List<string>();
        for (int i = index * 8; i < index * 8 + 8; i++)
        {
            if (i >= tmp.Count)
                break;
            
            result.Add(tmp[i]);
        }

        App.model.pedia.UsingPediaIds = result;
    }

    private bool IsCanToLeft()
    {
        int type = App.model.pedia.SelectedPediaType;
        int index = App.model.pedia.PediaPageIndex;
        List<string> tmp = App.factory.pediaFactory.GetPediaIds(type);
        
        if (index < 0)
            index = 0;

        int end = Mathf.CeilToInt(tmp.Count / 8f);
        if (index > end)
            index = end;

        return index > 0;
    }
    
    private bool IsCanToRight()
    {
        int type = App.model.pedia.SelectedPediaType;
        int index = App.model.pedia.PediaPageIndex;
        List<string> tmp = App.factory.pediaFactory.GetPediaIds(type);
        
        if (index < 0)
            index = 0;

        int end = Mathf.CeilToInt(tmp.Count / 8f);
        if (index > end)
            index = end;

        return index < end - 1;
    }
    
    #endregion

    #region Archive

    public void OpenArchive()
    {
        isPedia = false;

        App.model.pedia.ArchiveQuests = App.model.pedia.ArchiveQuests;
        
        ClosePedia();
        App.view.pedia.Open();
        App.view.pedia.archive.Open();

        DOVirtual.DelayedCall(0.1f, () => { SelectArchiveType(0); });
        App.model.pedia.ArchivePageIndex = 0;

        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
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

        quest.IsReceived = true;
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
            quests.Add(quest);

            if (quest.IsReceived)
                quest.Init();
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

        for (int i = 1; i <= 3; i++)
        {
            result = App.factory.questFactory.AchieveQuests[id + "_" + i];

            if (!result.IsReceived)
                return result;
        }

        return result;
    }

    #endregion
}