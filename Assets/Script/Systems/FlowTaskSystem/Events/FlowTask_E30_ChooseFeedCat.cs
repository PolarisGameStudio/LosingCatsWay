using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class FlowTask_E30_ChooseFeedCat : FlowTask
{
    public Card_FeedItem card_FeedItem;
    public UIView eventView;

    public override void Enter()
    {
        base.Enter();
        eventView.Show();

        var cat = App.system.cat.GetCats()[0];
        card_FeedItem.SetData(cat);
    }

    public override void Exit()
    {
        base.Exit();
        eventView.InstantHide();

        App.controller.feed.Close();
        App.controller.followCat.CloseByOpenLobby();
        App.controller.feed.Select(0);
    }
}
