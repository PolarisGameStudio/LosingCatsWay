using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;

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

    [Button]
    public void Test()
    {
        Open(App.factory.itemFactory.LevelRewards[9]);
    }

    public void Open(Reward[] value, bool setData = true)
    {
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

            Card_RewardSystem tmp = Instantiate(itemObject, content);
            tmp.SetUI(rewards[i]);
            DOVirtual.DelayedCall(0.1675f, tmp.PlayParticle);
        }

        animator.Play("Get_Item", 0, 0);
        InvokeRepeating("WaitAnimationEnd", 0.25f, 0.1f);
    }

    public void Close()
    {
        if (!isAnimationEnd)
            return;
        
        view.InstantHide();
        OnClose?.Invoke();
        OnClose = null;
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
        
        switch (reward.item.itemType)
        {
            case ItemType.Feed:
                App.system.inventory.FoodData[item.id] += reward.count;
                break;
            case ItemType.Tool:
                App.system.inventory.ToolData[item.id] += reward.count;
                break;
            case ItemType.Litter:
                App.system.inventory.LitterData[item.id] += reward.count;
                break;
            case ItemType.Room:
                App.system.inventory.RoomData[item.id] += reward.count;
                break;
            case ItemType.Coin:
                App.system.player.Coin += reward.count;
                break;
            case ItemType.Diamond:
                App.system.player.Diamond += reward.count;
                break;
        }
    }

    private void WaitAnimationEnd()
    {
        if (!(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
            return;

        isAnimationEnd = true;
        CancelInvoke("WaitAnimationEnd");
    }
}