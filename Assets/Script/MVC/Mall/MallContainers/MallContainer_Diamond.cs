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
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            if (App.system.player.ReduceDiamond(20))
                App.system.reward.Open(cash1000);
            else
                App.system.confirm.Active(ConfirmTable.NotEnoughDiamond);
        });
    }

    public void Buy3000MoneyByDiamond()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            if (App.system.player.ReduceDiamond(60))
                App.system.reward.Open(cash3000);
            else
                App.system.confirm.Active(ConfirmTable.NotEnoughDiamond);
        });
    }

    public void Buy5000CashByDiamond()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            if (App.system.player.ReduceDiamond(100))
                App.system.reward.Open(cash5000);
            else
                App.system.confirm.Active(ConfirmTable.NotEnoughDiamond);
        });
    }

    public void Buy25000CashByDiamond()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            if (App.system.player.ReduceDiamond(500))
                App.system.reward.Open(cash25000);
            else
                App.system.confirm.Active(ConfirmTable.NotEnoughDiamond);
        });
    }
}
