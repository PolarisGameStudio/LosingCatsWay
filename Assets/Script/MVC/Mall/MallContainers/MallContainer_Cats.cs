using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MallContainer_Cats : MallContainer
{
    public GameObject itemBuyMask;
    public TextMeshProUGUI itemBuyCountText;

    public BuyCatSubView buyCatSubView;

    public override void Refresh()
    {
        int itemCount = App.factory.itemFactory.GetItem("ISL00005").Count;

        itemBuyCountText.text = itemCount.ToString();
        itemBuyMask.SetActive(itemCount <= 0);
    }

    public void BuyCat_Diamond()
    {
        if (CheckCatCount())
            App.system.confirm.Active(ConfirmTable.Hints_Buy2, BuyCat_DiamondOk);
        else
            App.system.confirm.OnlyConfirm().Active(GetCatCountConfirmTable());
    }

    private async void BuyCat_DiamondOk()
    {
        // Item item = App.factory.itemFactory.GetItem("Diamond");

        if (!App.system.player.ReduceDiamond(300))
        {
            DOVirtual.DelayedCall(0.1f,
                () => App.system.confirm.Active(ConfirmTable.Hints_NoDiamond, () => App.controller.mall.SelectPage(6)));
            return;
        }

        // item.Count -= 300; // reduceDiamond
        CloudCatData cloudCatData = await CreateCat();
        Refresh();
        App.SaveData();

        buyCatSubView.Open(cloudCatData);
        
        App.controller.mall.OnBuyCat?.Invoke(cloudCatData);
    }

    public void BuyCat_Bottle()
    {
        if (CheckCatCount())
            App.system.confirm.Active(ConfirmTable.Hints_Buy2, BuyCat_BottleOk);
        else
            App.system.confirm.OnlyConfirm().Active(GetCatCountConfirmTable());
    }

    private async void BuyCat_BottleOk()
    {
        Item item = App.factory.itemFactory.GetItem("ISL00005");
        if (item.Count < 1) 
            return;

        item.Count--;
        CloudCatData cloudCatData = await CreateCat();
        Refresh();
        App.SaveData();

        buyCatSubView.Open(cloudCatData);
        
        App.controller.mall.OnBuyCat?.Invoke(cloudCatData);
    }

    private bool CheckCatCount()
    {
        return App.system.player.CanAdoptCatCount > 0;
    }

    private ConfirmTable GetCatCountConfirmTable()
    {
        ConfirmTable table;
        int count = App.system.room.FeatureRoomsCount;
        if (App.system.player.CatSlot >= count)
            table = ConfirmTable.Hints_NeedFeedRoom1;
        else
            table = ConfirmTable.Hints_NeedCatSlot1;
        return table;
    }

    private async Task<CloudCatData> CreateCat()
    {
        DebugTool_Cat debugToolCat = new DebugTool_Cat();

        CloudCatData cloudCatData = await debugToolCat.CreateCat(App.system.player.PlayerId, false, 1);
        cloudCatData.CatDiaryData.AdoptLocation = "Home";
        App.system.cat.CreateCatObject(cloudCatData);

        return cloudCatData;
    }
}