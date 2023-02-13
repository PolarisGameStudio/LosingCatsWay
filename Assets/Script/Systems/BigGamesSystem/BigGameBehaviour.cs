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

    protected int chance;
    
    private int score;
    private int exp;
    private int coins;

    [Button(30)]
    public virtual void Init()
    {
        score = 0;
        exp = 0;
        coins = 0;
    }

    public virtual void Open()
    {
        uIView.Show();
        
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string title = howToPlayData.titleData[country];
        string[] descripts = howToPlayData.descriptData[country];
        Sprite[] sprites = howToPlayData.sprites;
        App.system.howToPlay.SetData(title, descripts, sprites).Open(true, null, Init);
    }

    public virtual void OpenAbout()
    {
        if (App.system.tutorial.isTutorial)
            return;
        App.system.howToPlay.Open(false);
    }

    public virtual void OpenPause()
    {
    }

    public virtual void ClosePause()
    {
    }

    public virtual void Close()
    {
        App.system.transition.Active(0, () =>
        {
            uIView.InstantHide();
            App.system.bigGames.Close();
            GameEndAction();
            App.controller.lobby.Open();
        });
    }

    protected void OpenSettle()
    {
        exp = App.system.player.playerDataSetting.GetBigGameExpByChance(chance);
        coins = App.system.player.playerDataSetting.GetBigGameCoinsByChance(App.system.player.Level, chance);
        score = Convert.ToInt32(100f / hearts.Length * chance);
        App.system.settle.Active(exp, coins, score, Close);
    }

    private void GameEndAction()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (exp > 0)
            App.system.player.AddExp(exp);
        if (coins > 0)
            App.system.player.AddMoney(coins);
    }
}