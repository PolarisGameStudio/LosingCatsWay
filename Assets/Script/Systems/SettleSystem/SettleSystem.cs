using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using Doozy.Runtime.UIManager.Containers;
using DG.Tweening;

public class SettleSystem : MvcBehaviour
{
    [SerializeField] private UIView uiView;
    [SerializeField] private CatSkin catSkin;
    
    [Title("UI")]
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI levelColorText;
    [SerializeField] private TextMeshProUGUI expPercentText;
    [SerializeField] private Image expFill;
    [SerializeField] private GameObject levelUpObject;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject[] hearts;

    [Title("Reward")]
    [SerializeField] private GameObject expObject; // icon位置不同所以不用cardSettle
    [SerializeField] private GameObject moneyObject;
    [SerializeField] private GameObject diamondObject;
    [SerializeField] private TextMeshProUGUI getExpText;
    [SerializeField] private TextMeshProUGUI getMoneyText;
    [SerializeField] private TextMeshProUGUI getDiamondText;
    [SerializeField] private Transform content;
    [SerializeField] private CardSettle cardSettle;

    [Title("Tween")]
    [SerializeField] private Transform topTransform;

    private List<GameObject> _tmpObjects = new List<GameObject>();

    private Callback _onClose;

    public void Init()
    {
        App.system.myTime.OnFirstLogin += () =>
        {
            PlayerPrefs.SetInt("KnowledgeCard", 0);
        };
    }

    private void Open()
    {
        uiView.Show();
    }

    public void Close()
    {
        uiView.InstantHide();
        _onClose?.Invoke();
        _onClose = null;

        for (int i = 0; i < _tmpObjects.Count; i++)
            Destroy(_tmpObjects[i]);
        _tmpObjects.Clear();

        // CheckKnowledgeCard();
    }

    public void Active(string gameName, CloudCatData cloudCatData, int exp, int money, int diamond, int chance, Reward[] rewards, Callback onClose, bool isCatchGame = false)
    {
        App.system.soundEffect.Play("ED00044");
        float beforeExp = App.system.player.Exp;
        float afterExp = beforeExp + exp;
        int level = App.system.player.Level;
        float fullExp = App.system.player.playerDataSetting.GetNextLevelUpExp(level);
        bool isKitty = false;

        _onClose += onClose;

        closeButton.SetActive(false);
        levelUpObject.SetActive(false);
        
        if (cloudCatData == null)
            Debug.LogError("CloudCatData is null");
        else
        {
            catSkin.ChangeSkin(cloudCatData);
            isKitty = cloudCatData.CatData.SurviveDays <= 3;
        }

        if (rewards != null)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                var tmp = Instantiate(cardSettle, content);
                tmp.SetData(rewards[i]);
                _tmpObjects.Add(tmp.gameObject);
            }
        }
        
        Open();
        
        // SetData
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(false);

        topTransform.DOScaleX(1, 0.25f).From(0.3f).SetEase(Ease.OutBack).SetDelay(0.2f);

        if (chance < 1)
        {
            if (isCatchGame)
                SetCatRunAway(isKitty);
            else
                SetCatBad(isKitty);
        }
        if (chance >= 1)
        {
            SetCatGood(isKitty);
            
            hearts[0].SetActive(true);
            hearts[0].transform.DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetDelay(0.5f);
        }
        if (chance >= 2)
        {
            SetCatGood(isKitty);
            
            hearts[1].SetActive(true);
            hearts[1].transform.DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetDelay(0.7f);
        }
        if (chance >= 3)
        {
            if (isCatchGame)
                SetCatGotcha(isKitty);
            else
                SetCatPerfect(isKitty);
            
            hearts[2].SetActive(true);
            hearts[2].transform.DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetDelay(0.9f);
        }

        gameNameText.text = gameName;
        levelText.text = $"LV{level:00}";
        levelColorText.text = $"LV{level:00}";

        expPercentText.text = $"{100 / fullExp * afterExp:0}%";
        expFill.fillAmount = 1f / fullExp * beforeExp;
        expFill.DOFillAmount(1f / fullExp * afterExp, 0.25f).SetEase(Ease.OutExpo).SetDelay(0.7f).OnComplete(() =>
        {
            if (expFill.fillAmount >= 1)
            {
                levelText.text = $"LV{level + 1:00}";
                levelColorText.text = $"LV{level + 1:00}";
                float nextFullExp = App.system.player.playerDataSetting.GetNextLevelUpExp(level + 1);
                float moreExp = afterExp - fullExp;
                expFill.fillAmount = 0;
                expPercentText.text = $"{100 / nextFullExp * moreExp:0}%";
                expFill.DOFillAmount(1f / nextFullExp * moreExp, 0.25f).SetEase(Ease.OutExpo);
                levelUpObject.SetActive(true);
            }
        });

        expObject.SetActive(exp > 0);
        moneyObject.SetActive(money > 0);
        diamondObject.SetActive(diamond > 0);
        
        getExpText.text = exp.ToString();
        getMoneyText.text = money.ToString();
        getDiamondText.text = diamond.ToString();

        for (int i = 0; i < content.childCount; i++)
            content.GetChild(i).transform.DOScale(Vector2.one, 0.2f).From(0).SetEase(Ease.OutBack).SetDelay(i * 0.25f);

        DOVirtual.DelayedCall(1f, () => closeButton.SetActive(true));
    }

    #region CatAnim

    private void SetCatPerfect(bool isKitty)
    {
        if (isKitty)
        {
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Common_Main/Kitty_Skill", true);
        }
        else
        {
            catSkin.SetLove();
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Win", true);
        }
    }

    private void SetCatGood(bool isKitty)
    {
        if (isKitty)
        {
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Win", true);
        }
        else
        {
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Rearing_Cat/Rearing_Smile_IDLE", true);
        }
    }

    private void SetCatBad(bool isKitty)
    {
        if (isKitty)
        {
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", true);
        }
        else
        {
            catSkin.SetCold();
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Rearing_Cat/Rearing_Reject", true);
        }
    }

    private void SetCatGotcha(bool isKitty)
    {
        if (isKitty)
        {
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Win", true);
        }
        else
        {
            catSkin.SetDocile();
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Individuality_Main/Individuality_Docile", true);
        }
    }

    private void SetCatRunAway(bool isKitty)
    {
        if (isKitty)
        {
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", true);
        }
        else
        {
            catSkin.SetAngry();
            catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", true);
        }
    }

    #endregion
}