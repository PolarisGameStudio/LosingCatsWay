using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    protected void SuccessToLobby()
    {
        Success();
        App.controller.followCat.CloseByOpenLobby();
    }

    protected void FailedToLobby()
    {
        Failed();
        App.controller.followCat.CloseByOpenLobby();
    }

    private void Success()
    {
        int exp = App.system.player.playerDataSetting.LittleGameExp;
        int coin = App.system.player.playerDataSetting.GetLittleGameCoinsByLevel(App.system.player.Level);
        
        App.system.player.AddExp(exp);
        App.system.player.AddMoney(coin);
        
        cat.catCanvas.ActiveExp(exp, () =>
        {
            cat.catCanvas.ActiveMoney(coin);
        });
    }

    private void Failed()
    {
        int exp = App.system.player.playerDataSetting.LittleGameExp;
        App.system.player.AddExp(exp);
        cat.catCanvas.ActiveExp(exp);
    }
}

