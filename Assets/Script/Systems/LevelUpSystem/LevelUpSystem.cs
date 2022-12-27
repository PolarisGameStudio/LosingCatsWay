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

    public void Open()
    {
        SetLastSibling();
        view.InstantShow();

        int level = App.system.player.Level;

        levelUpText.text = (level + 1).ToString();
        levelDownText.text = level.ToString();

        var rewards = App.factory.itemFactory.LevelRewards[level];
        App.system.reward.SetDatas(rewards);
        
        List<string> unlockIds = new List<string>();

        for (int i = 0; i < rewards.Length; i++)
            if (rewards[i].item.itemType == ItemType.Unlock)
            {
                unlockIds.Add(rewards[i].item.id);
            }

        if (unlockIds.Count == 0)
        {
            animator.Play("SwitchText(NoUnlock)", 0, 0);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < unlockIds.Count)
                {
                    unlocks[i].SetActive(true);
                    unlockContents[i].text = App.factory.stringFactory.GetUnlock(unlockIds[i]);
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