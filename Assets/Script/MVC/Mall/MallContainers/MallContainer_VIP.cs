using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MallContainer_VIP : MallContainer
{
    public TextMeshProUGUI timeText;
    public CanvasGroup page1;
    public CanvasGroup page2;
    public Button backButton;
    public Button nextButton;

    public GameObject block;
    public GameObject buyButton;

    private DateTime _monthLastTime;

    public void BuyGhostBlack()
    {
        App.system.confirm.Active(ConfirmTable.BuyConfirm, () =>
        {
            if (!App.system.player.ReduceDiamond(360))
            {
                Reward[] rewards = new Reward[1];
                // rewards[0] = new Reward()
                App.system.reward.Open(rewards);
            }else
                App.system.confirm.Active(ConfirmTable.NotEnoughDiamond);
        });
    }

    public override void Refresh()
    {
        if (!App.model.mall.PurchaseRecords.ContainsKey("IMP00001"))
        {
            block.SetActive(false);
            buyButton.SetActive(false);
            return;
        }

        CheckTerm();

        var purchaseRecords = App.model.mall.PurchaseRecords["IMP00001"];

        bool flag = purchaseRecords.BuyCount > 0;

        block.SetActive(flag);
        buyButton.SetActive(flag);
    }

    public override void Open()
    {
        base.Open();

        nextButton.interactable = true;
        backButton.interactable = false;

        page1.alpha = 1;
        page2.alpha = 0;

        SetTime();
        Refresh();
    }

    public override void Close()
    {
        base.Close();
        CancelInvoke("TimeCount");
    }

    public void TurnRight()
    {
        page1.DOFade(0, 0.25f);
        page2.DOFade(1, 0.25f);

        nextButton.interactable = false;
        backButton.interactable = true;
    }

    public void TurnLeft()
    {
        page1.DOFade(1, 0.25f);
        page2.DOFade(0, 0.25f);

        nextButton.interactable = true;
        backButton.interactable = false;
    }

    private void CheckTerm()
    {
        if (!App.model.mall.PurchaseRecords.ContainsKey("IMP00001"))
            return;

        var lastBuyTime = App.model.mall.PurchaseRecords["IMP00001"].LastBuyTime.ToDateTime();
        var notTime = DateTime.Now;

        if (lastBuyTime.Year != notTime.Year || lastBuyTime.Month != notTime.Month)
        {
            App.model.mall.PurchaseRecords["IMP00001"].BuyCount = 0;
            App.system.player.UsingAvatar = "PAT001";
        }
    }

    private void SetTime()
    {
        DateTime now = DateTime.Now;
        now = now.AddHours(-now.Hour);
        now = now.AddMinutes(-now.Minute);
        now = now.AddSeconds(-now.Second);

        _monthLastTime = now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
        InvokeRepeating("TimeCount", 0, 1);
    }

    public void OnPurchaseSetAvatar()
    {
        App.system.player.UsingAvatar = "PAT002";
    }

    private void TimeCount()
    {
        var t = _monthLastTime - DateTime.Now;
        timeText.text = t.Days + "D " + t.Hours + ":" + t.Minutes + ":" + t.Seconds;
    }
}