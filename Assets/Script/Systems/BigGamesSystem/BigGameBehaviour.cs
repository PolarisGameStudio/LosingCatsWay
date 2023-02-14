using System;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UIView))]
public class BigGameBehaviour : MvcBehaviour
{
    [EnumToggleButtons, HideLabel, Title("GameType")]
    public RoomGameType gameType;

    [Title("Config")]
    public string notifyId;
    public HowToPlayData howToPlayData;

    [Title("UI")]
    [SerializeField] private UIView uIView;
    public GameObject[] hearts;

    protected int chance;
    
    private int _score;
    private int _exp;
    private int _coins;

    private CloudCatData _cloudCatData;

    protected virtual void Init()
    {
        _score = 0;
        _exp = 0;
        _coins = 0;
    }

    public void SetCloudCatData(CloudCatData cloudCatData)
    {
        _cloudCatData = cloudCatData;
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
        App.system.reward.OnClose -= Close;
        
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
        _exp = App.system.player.playerDataSetting.GetBigGameExpByChance(chance);
        _coins = App.system.player.playerDataSetting.GetBigGameCoinsByChance(App.system.player.Level, chance);
        _score = Convert.ToInt32(100f / hearts.Length * chance);
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string gameName = howToPlayData.titleData[country];
        App.system.settle.Active(gameName, _cloudCatData, _exp, _coins, 0, _score, null, CheckKnowledgeCard);
    }
    
    private void CheckKnowledgeCard()
    {
        if (App.system.tutorial.isTutorial)
        {
            Close();
            return;
        }

        if (_score < 30)
        {
            Close();
            return;
        }

        int knowledgeCard = PlayerPrefs.GetInt("KnowledgeCard");

        if (knowledgeCard >= 7)
        {
            Close();
            return;
        }

        if (Random.value > 0.5f)
        {
            Close();
            return;
        }
        
        Reward[] rewards = new Reward[1];
        rewards[0] = new Reward(App.factory.itemFactory.GetItem("KLC0001"), 1);
        
        App.system.reward.Open(rewards);
        App.system.reward.OnClose += Close;
        
        PlayerPrefs.SetInt("KnowledgeCard", knowledgeCard);
    }

    private void GameEndAction()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (_exp > 0)
            App.system.player.AddExp(_exp);
        if (_coins > 0)
            App.system.player.AddMoney(_coins);
    }
}