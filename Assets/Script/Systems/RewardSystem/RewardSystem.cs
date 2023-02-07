using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;

public class RewardSystem : MvcBehaviour
{
    public UIView view;
    public Callback OnClose;

    [Title("Animator")] 
    public Animator animator;

    private bool isAnimationEnd = false;
    private Reward[] rewards;
    private Item[] items;

    [Title("UI")] 
    public Transform content;
    public Card_RewardSystem itemObject;
    public TextMeshProUGUI continueText;

    private int siblingIndex;

    [Button]
    public void Test()
    {
        Open(App.factory.itemFactory.LevelRewards[9]);
    }

    public void Open(Reward[] value, bool setData = true)
    {
        SetLastSibling();
        view.InstantShow();
        rewards = value;

        if (setData)
            SetDatas(rewards);

        isAnimationEnd = false;

        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);
        
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i].item.itemType == ItemType.Unlock)
                continue;
            if (rewards[i].count <= 0) // todo 這是解鎖
                continue;

            Card_RewardSystem tmp = Instantiate(itemObject, content);
            tmp.SetUI(rewards[i]);
            DOVirtual.DelayedCall(0.1675f, tmp.PlayParticle);
        }

        animator.Play("Get_Item", 0, 0);
        InvokeRepeating("WaitAnimationEnd", 0.25f, 0.1f);

        continueText.DOKill();
        continueText.DOFade(1, 0.75f).From(0).SetLoops(-1, LoopType.Yoyo).SetDelay(1);
    }

    public void Close()
    {
        if (!isAnimationEnd)
            return;
        
        view.InstantHide();
        OnClose?.Invoke();
        OnClose = null;
        ResetSibling();
    }

    public void SetDatas(Reward[] rewards)
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i].item.itemType == ItemType.Unlock)
                continue;

            SetData(rewards[i]);
        }
    }
    
    private void SetData(Reward reward)
    {
        var item = reward.item;

        if (item.id == "Money")
        {
            App.system.player.AddMoney(reward.count);
            return;
        }
        
        if (item.id == "Diamond")
        {
            App.system.player.AddDiamond(reward.count);
            return;
        }
        
        item.Count += reward.count;
    }

    private void WaitAnimationEnd()
    {
        if (!(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
            return;

        isAnimationEnd = true;
        CancelInvoke("WaitAnimationEnd");
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