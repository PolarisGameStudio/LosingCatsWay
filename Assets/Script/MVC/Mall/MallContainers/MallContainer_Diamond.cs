using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MallContainer_Diamond : MallContainer
{
    public Reward[] cash1000;
    public Reward[] cash3000;
    public Reward[] cash5000;
    public Reward[] cash25000;
    
    public void Buy1000MoneyByDiamond()
    {
        Item money = App.factory.itemFactory.GetItem("Money");
        string contentInsert = 1000 + money.Name;
        App.system.confirm.ActiveByInsert(ConfirmTable.Hints_Buy1, string.Empty, contentInsert, () =>
        {
            if (App.system.player.ReduceDiamond(20))
                GetItem(cash1000);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }

    public void Buy3000MoneyByDiamond()
    {
        Item money = App.factory.itemFactory.GetItem("Money");
        string contentInsert = 3000 + money.Name;
        App.system.confirm.ActiveByInsert(ConfirmTable.Hints_Buy1, string.Empty, contentInsert, () =>
        {
            if (App.system.player.ReduceDiamond(60))
                GetItem(cash3000);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }

    public void Buy5000CashByDiamond()
    {
        Item money = App.factory.itemFactory.GetItem("Money");
        string contentInsert = 5000 + money.Name;
        App.system.confirm.ActiveByInsert(ConfirmTable.Hints_Buy1, string.Empty, contentInsert, () =>
        {
            if (App.system.player.ReduceDiamond(100))
                GetItem(cash5000);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }

    public void Buy25000CashByDiamond()
    {
        Item money = App.factory.itemFactory.GetItem("Money");
        string contentInsert = 25000 + money.Name;
        App.system.confirm.ActiveByInsert(ConfirmTable.Hints_Buy1, string.Empty, contentInsert, () =>
        {
            if (App.system.player.ReduceDiamond(500))
                GetItem(cash25000);
            else
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
        });
    }
}
