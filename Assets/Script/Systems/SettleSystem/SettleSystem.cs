using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using Doozy.Runtime.UIManager.Containers;
using DG.Tweening;
using Random = UnityEngine.Random;

public class SettleSystem : MvcBehaviour
{
    [SerializeField] private UIView uiView;

    [Title("Top")] [SerializeField] private Image expFill;
    [SerializeField] private TextMeshProUGUI remainExpText;
    [SerializeField] private TextMeshProUGUI addExpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Title("Bot")] [SerializeField] private TextMeshProUGUI expText_Bottom;
    [SerializeField] private TextMeshProUGUI coinText_Bottom;

    [Title("DoTween/Top")] [SerializeField]
    private RectTransform scoreRect;

    [SerializeField] private RectTransform levelRect;
    [SerializeField] private RectTransform topTitleRect;
    [SerializeField] private RectTransform barRect;
    [SerializeField] private RectTransform remainExpRect;

    [Title("DoTween/Bot")] [SerializeField]
    private RectTransform botTitleRect;

    [SerializeField] private RectTransform expRect;
    [SerializeField] private RectTransform coinRect;
    [SerializeField] private RectTransform buttonRect;

    private Callback OnClose;

    private float _score;

    public void Init()
    {
        App.system.myTime.OnFirstLogin += () =>
        {
            PlayerPrefs.SetInt("KnowledgeCard", 0);
        };
    }

    [Button]
    private void Open()
    {
        uiView.Show();
    }

    public void Close()
    {
        uiView.Hide();
        OnClose?.Invoke();
        OnClose = null;

        CheckKnowledgeCard();
    }

    public void Active(int exp, int coins, float score, Callback onClose = null)
    {
        float lastExp = App.system.player.Exp;
        float newExp = lastExp + exp;
        int level = App.system.player.Level;
        float nextLevelExp = App.system.player.playerDataSetting.GetNextLevelUpExp(level);

        //Callback
        OnClose += onClose;

        //Top
        levelText.text = level.ToString("00");
        scoreText.text = $"{score}%";

        //Bot
        expText_Bottom.text = $"x{exp}";
        coinText_Bottom.text = $"x{coins}";

        expFill.fillAmount = lastExp / nextLevelExp;

        remainExpText.text = (nextLevelExp - newExp).ToString();
        addExpText.text = $"+{newExp - lastExp}";

        _score = score;

        TweenInit();
        Open();
        TweenIn(() =>
        {
            if (newExp >= nextLevelExp)
            {
                float newNextLevelExp = App.system.player.playerDataSetting.GetNextLevelUpExp(level + 1);
                float endExp = newNextLevelExp - newExp;
                expFill.DOFillAmount(1f, 0.3f).OnComplete(() =>
                {
                    level += 1;
                    levelText.text = level.ToString("00");
                    remainExpText.text = (newNextLevelExp - newExp).ToString();
                    addExpText.text = $"+{newExp - lastExp}";

                    //TODO Show LevelUpObject

                    expFill.fillAmount = 0;
                });
                expFill.DOFillAmount(1f / newNextLevelExp * endExp, 0.3f).SetDelay(0.8f).OnComplete(() =>
                {
                    buttonRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);
                });
            }
            else
            {
                expFill.DOFillAmount(1f / nextLevelExp * newExp, 0.3f).OnComplete(() =>
                {
                    buttonRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);
                });
            }
        });
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
        
        Reward[] rewards = new Reward[1];
        rewards[0] = new Reward(App.factory.itemFactory.GetItem("KLC0001"), 1);
        
        App.system.reward.Open(rewards);
        PlayerPrefs.SetInt("KnowledgeCard", knowledgeCard);
    }

    #region DoTween

    private void TweenInit()
    {
        scoreRect.localScale = Vector2.zero;
        levelRect.localScale = Vector2.zero;
        topTitleRect.localScale = Vector2.zero;
        barRect.localScale = Vector2.zero;
        remainExpRect.localScale = Vector2.zero;

        botTitleRect.localScale = Vector2.zero;
        expRect.localScale = Vector2.zero;
        coinRect.localScale = Vector2.zero;
        buttonRect.localScale = Vector2.zero;
    }

    private void TweenIn(TweenCallback endCallback = null)
    {
        scoreRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);
        levelRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f);
        topTitleRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f * 2);
        barRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f * 3);
        remainExpRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f * 4);

        botTitleRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f * 5);
        expRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f * 6);
        coinRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1f * 7)
            .OnComplete(endCallback);
    }

    #endregion
}