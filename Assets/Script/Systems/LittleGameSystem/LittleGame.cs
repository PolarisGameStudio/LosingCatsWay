using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class LittleGame : MvcBehaviour
{
    [Title("Basic")]
    public string notifyId;
    public int littleGameIndex;

    [Title("Confirm")] 
    public string titleId;
    public string descriptionId;
    // public ConfirmTable endId;

    protected Cat cat;
    protected Animator anim;

    public virtual void StartGame(Cat cat)
    {
        this.cat = cat;
        anim = this.cat.GetComponent<Animator>();
        
        anim.SetBool(CatAnimTable.IsCanExit.ToString(), false);
        anim.SetInteger(CatAnimTable.LittleGameIndex.ToString(), littleGameIndex);
        anim.Play(CatAnimTable.LittleGame.ToString());
    }

    protected void Close()
    {
        App.system.littleGame.Close();
        cat.ChangeSkin();
    }

    protected void OpenLobby()
    {
        App.controller.followCat.CloseByOpenLobby();
    }

    public void Success()
    {
        int exp = App.system.player.playerDataSetting.LittleGameExp;
        int coin = App.system.player.playerDataSetting.GetLittleGameCoinsByLevel(App.system.player.Level);
        cat.CatRewardCanvas.PopReward(exp, coin);
    }

    public void Failed()
    {
        int exp = App.system.player.playerDataSetting.LittleGameExp;
        cat.CatRewardCanvas.PopReward(exp, 0);
    }
}

