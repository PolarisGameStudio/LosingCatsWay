using System;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(UIView))]
public class BigGameBehaviour : MvcBehaviour
{
    [EnumToggleButtons, HideLabel, Title("GameType")] public RoomGameType gameType;

    [Title("Config")]
    public string notifyId;
    public HowToPlayData howToPlayData;

    [Title("UI")]
    [SerializeField] private UIView uIView;
    public GameObject[] hearts;

    protected int score = 0;
    protected int exp = 0;
    protected int chance;
    protected bool isPause;
    
    private int coins;

    [Button(30)]
    public virtual void Init()
    {
    }

    [Button(30)]
    public virtual void Open()
    {
        App.system.bigGames.Open();
        uIView.Show();
        
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string title = howToPlayData.titleData[country];
        string[] descripts = howToPlayData.descriptData[country];
        Sprite[] sprites = howToPlayData.sprites;
        App.system.howToPlay.SetData(title, descripts, sprites).Open(true, null, Init);
    }

    public virtual void OpenAbout()
    {
        App.system.howToPlay.Open(false, null, () => isPause = false);
        //TODO Pause every game
    }

    public virtual void OpenPause()
    {
        isPause = true;
        //TODO Pause every game
    }

    public virtual void ClosePause()
    {
        isPause = false;
        //TODO Unpause every game
    }

    public virtual void Close()
    {
        App.system.transition.Active(0, () =>
        {
            uIView.InstantHide();
            App.system.bigGames.Close();
            App.controller.lobby.Open();
        }, GameEndAction);
    }

    public void OpenSettle()
    {
        exp = App.system.player.playerDataSetting.GetBigGameExpByChance(chance);
        coins = App.system.player.playerDataSetting.GetBigGameCoinsByChance(App.system.player.Level, chance);
        score = Convert.ToInt32((100f / hearts.Length * chance));
        App.system.settle.Active(exp, coins, score, Close);
    }

    private void GameEndAction()
    {
        App.system.player.AddExp(exp);
        App.system.player.Coin += coins;
    }
}