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

    public Callback OnClose;

    [Button]
    public void Up()
    {
        App.system.player.Level++;
    }

    public void Open(int beforeLevel, int afterLevel)
    {
        SetLastSibling();
        view.InstantShow();

        // int level = afterLevel;
        App.system.soundEffect.Play("ED00054");

        levelUpText.text = afterLevel.ToString();
        levelDownText.text = beforeLevel.ToString();

        var unlockArray = App.factory.itemFactory.GetUnlocksByLevel(afterLevel);
        var unlockItems = new List<Item>(unlockArray);
        // var levelRewards = App.factory.itemFactory.GetRewardsByLevel(afterLevel);
        
        // App.system.reward.SetDatas(levelRewards);

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
                    string key = GetUnlockKey(unlock.id);
                    string unlockHead = App.factory.stringFactory.GetUnlockHead(key);
                    unlockContents[i].text = unlockHead + unlock.Name;
                    
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
        OnClose?.Invoke();
        App.system.soundEffect.Play("ED00008");
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

    private string GetUnlockKey(string id)
    {
        if (id.Contains("IRM"))
            return "ULK003";
        if (id.Contains("ICL"))
            return "ULK005";
        return id;
    }
}