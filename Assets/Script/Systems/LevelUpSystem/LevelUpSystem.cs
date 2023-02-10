using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;

public class LevelUpSystem : MvcBehaviour
{
    public TextMeshProUGUI levelUpText;
    public TextMeshProUGUI levelDownText;

    public UIView view;

    [Title("Animator")] public Animator animator;

    [Title("UI")] public GameObject[] unlocks;
    public TextMeshProUGUI[] unlockContents;

    private int siblingIndex;

    [Button]
    public void Up()
    {
        App.system.player.Level++;
    }

    public void Open(int lastLevel)
    {
        SetLastSibling();
        view.InstantShow();

        int level = lastLevel;

        levelUpText.text = (level + 1).ToString();
        levelDownText.text = level.ToString();

        var unlockItems = App.factory.itemFactory.GetUnlockItemsByLevel(level + 1);
        var levelRewards = App.factory.itemFactory.GetRewardsByLevel(level + 1);
        
        App.system.reward.SetDatas(levelRewards);

        if (unlockItems.Count <= 0)
        {
            animator.Play("SwitchText(NoUnlock)", 0, 0);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < unlockItems.Count)
                {
                    unlocks[i].SetActive(true);
                    
                    var unlock = unlockItems[i];
                    unlockContents[i].text = unlock.Name;
                    continue;
                }
                
                unlocks[i].SetActive(false);
            }
            
            animator.Play("SwitchText", 0, 0);
        }

        InvokeRepeating("WaitAnimationEnd", 0.25f, 0.1f);
    }
    
    public void Open()
    {
        SetLastSibling();
        view.InstantShow();

        int level = App.system.player.Level;

        levelUpText.text = (level + 1).ToString();
        levelDownText.text = level.ToString();

        var unlockItems = App.factory.itemFactory.GetUnlockItemsByLevel(level + 1);
        var levelRewards = App.factory.itemFactory.GetRewardsByLevel(level + 1);
        
        App.system.reward.SetDatas(levelRewards);

        if (unlockItems.Count <= 0)
        {
            animator.Play("SwitchText(NoUnlock)", 0, 0);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < unlockItems.Count)
                {
                    unlocks[i].SetActive(true);
                    
                    var unlock = unlockItems[i];
                    unlockContents[i].text = unlock.Name;
                    continue;
                }
                
                unlocks[i].SetActive(false);
            }
            
            animator.Play("SwitchText", 0, 0);
        }

        InvokeRepeating("WaitAnimationEnd", 0.25f, 0.1f);
    }

    private void WaitAnimationEnd()
    {
        if (!(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
            return;

        Close();
        CancelInvoke("WaitAnimationEnd");
    }

    public void Close()
    {
        ResetSibling();
        view.InstantHide();
        App.system.reward.Open(App.factory.itemFactory.LevelRewards[App.system.player.Level], false);
    }

    private void SetLastSibling()
    {
        siblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }

    private void ResetSibling()
    {
        if (siblingIndex == -1)
            return;
        transform.SetSiblingIndex(siblingIndex);
        siblingIndex = -1;
    }
}