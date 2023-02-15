using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_2", menuName = "Factory/Quests/MEQ/Create MEQ0002_2")]
public class MEQ0002_2 : Quest
{
    public override void Init()
    {
        base.Init();
        App.controller.lobby.OnLobbyOpen += Bind;
    }

    public void Bind()
    {
        int level = App.system.player.Level;
        
        if (level >= 10)
        {
            Progress = 1;
            App.controller.lobby.OnLobbyOpen -= Bind;
        }
    }
}
