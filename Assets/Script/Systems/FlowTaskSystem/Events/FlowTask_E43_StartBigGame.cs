using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using System.Runtime.InteropServices.ComTypes;

public class FlowTask_E43_StartBigGame : FlowTask
{
    //public UIView eventView;

    public override void Enter()
    {
        base.Enter();
        //eventView.Show();

        //App.system.bigGame.OnGameEnd += Exit;
        //App.system.transition.FadeLoadScene(App.system.bigGame.RandomGame().ToString());
        
        App.system.bigGames.OnClose += Exit;
        App.system.bigGames.OpenRandomGame();
        // App.system.cat.CloseCatsGame();

        //var cat = App.system.cat.GetCats()[0];
        //App.system.lobbyNotify.ClearNotify(cat.catData.CatId);
    }

    public override void Exit()
    {
        //App.system.bigGame.OnGameEnd -= Exit;
        App.system.bigGames.OnClose -= Exit;

        App.controller.followCat.CloseByOpenLobby();
        App.system.grid.SetCameraToOrigin();

        base.Exit();
        //eventView.Hide();
    }
}
