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
    private int _diamond;
    private List<Reward> _rewards;

    private CloudCatData _cloudCatData;

    protected virtual void Init()
    {
        _score = 0;
        _exp = 0;
        _coins = 0;
        
        App.system.bgm.FadeOut(0).FadeIn(1).Play("BG00001");
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
        App.system.howToPlay.Open(false, Pause, Resume);
    }

    public virtual void Pause()
    {
    }

    public virtual void Resume()
    {
    }

    public virtual void OpenPause()
    {
        Pause();
    }

    public virtual void ClosePause()
    {
    }

    public virtual void Close()
    {
        App.system.reward.OnClose -= Close;

        App.system.bgm.FadeOut();
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
        _diamond = Random.value < 0.05f ? 2 : 0;
        
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string gameName = howToPlayData.titleData[country];

        _rewards = new List<Reward>();
        CheckKnowledgeCard();
        CheckSnack();
        
        App.system.settle.Active(gameName, _cloudCatData, _exp, _coins, _diamond, chance, _rewards.ToArray(), Close);
    }
    
    private void CheckKnowledgeCard()
    {
        if (App.system.tutorial.isTutorial)
            return;

        if (_score < 30)
            return;

        int knowledgeCard = PlayerPrefs.GetInt("KnowledgeCard");

        if (knowledgeCard >= 7)
            return;

        if (Random.value > 0.5f)
            return;
        
        var reward = new Reward(App.factory.itemFactory.GetItem("KLC0001"), 1);
        _rewards.Add(reward);
        
        // 加數量
        reward.item.Count += reward.count;
        PlayerPrefs.SetInt("BagRedPoint" + 6, 1);
        
        App.controller.bag.RefreshReds();
        
        PlayerPrefs.SetInt("KnowledgeCard", knowledgeCard);
    }

    private void CheckSnack()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (_score < 30)
            return;
        
        if (Random.value > 0.2f)
            return;

        string id = "ISK0000" + Random.Range(1, 4);
        var reward = new Reward(App.factory.itemFactory.GetItem(id), 3);
        _rewards.Add(reward);

        // 加數量
        reward.item.Count += reward.count;
        PlayerPrefs.SetInt("BagRedPoint" + 1, 1);
        
        App.controller.bag.RefreshReds();
    }
    
    private void GameEndAction()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (_exp > 0)
            App.system.player.AddExp(_exp);
        if (_coins > 0)
            App.system.player.AddMoney(_coins);
        if (_diamond > 0)
            App.system.player.AddDiamond(_diamond);
    }
}