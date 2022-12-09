using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Spine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class CatchCatMap : MvcBehaviour
{
    public Callback OnGameEnd;
    
    #region Variable

    [SerializeField] private HowToPlayData howToPlayData;

    [Title("UIView")]
    [SerializeField] private UIView uiView;

    [Title("Items")] [SerializeField] private Item_CatchCat[] items;

    [Title("CatSkin")] [SerializeField] private CatSkin catSkin;

    [Title("UI")] 
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI runChanceText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] chooseTypeButtons; //選擇種類按鈕*4
    [SerializeField] private Button catchButton; //捕捉按鈕
    [SerializeField] private Card_CatchPersonality[] cardPersonalitys; //個性卡*3
    [SerializeField] private Card_CatchItem[] cardItems; //道具卡*5
    [SerializeField] private GameObject[] chooseTypeMasks;
    [SerializeField] private GameObject blockRaycastObject;
    
    [TabGroup("Top")] [SerializeField] private RectTransform turnRect;
    [TabGroup("Top")] [SerializeField] private RectTransform chanceRect;
    [TabGroup("Top")] [SerializeField] private RectTransform chanceIconRect;
    [TabGroup("Top")] [SerializeField] private Image turnDarkMask;
    [TabGroup("Top")] [SerializeField] private Image chanceDarkMask;
    [TabGroup("Top")] [SerializeField] private CanvasGroup topFullMask;
    [TabGroup("Top")] [SerializeField] private GameObject topBlur;
    [TabGroup("Top")] [SerializeField] private RectTransform topGroupRect;
    [TabGroup("Top")] [SerializeField] private RectTransform turnTextRect;
    [TabGroup("Top")] [SerializeField] private RectTransform chanceTextRect;

    [TabGroup("TopLeft")] [SerializeField] private RectTransform exitButtonRect;
    [TabGroup("TopLeft")] [SerializeField] private RectTransform aboutButtonRect;
    [TabGroup("TopLeft")] [SerializeField] private Image exitDarkMask;
    [TabGroup("TopLeft")] [SerializeField] private Image aboutDarkMask;

    [TabGroup("TopRight")] [SerializeField] private RectTransform ropeRect;

    [TabGroup("Center")] [SerializeField] private RectTransform hpBarRect;
    [TabGroup("Center")] [SerializeField] private RectTransform barHeartRect;
    [TabGroup("Center")] [SerializeField] private RectTransform barTitleRect;

    [TabGroup("Item")] [SerializeField]
    private Image itemImage;

    [TabGroup("Bot")] [SerializeField] private Image botDarkMask;
    [TabGroup("Bot")] [SerializeField] private GameObject lastTurnCatchButton;

    [Title("Particles")] [SerializeField] private UIParticle loveParticle;
    [SerializeField] private UIParticle hateParticle;
    [SerializeField] private UIParticle bigLoveParticle;
    [SerializeField] private UIParticle catchButtonParticle;

    [Title("Animator")] [SerializeField] private Animator catchFlowerAnimator;

    private int turn;
    private float[] runChance = { 0f, 0f, 0.03f, 0.05f, 0.1f, 0.2f, 1f };
    private float hp; //生命值
    private CloudCatData cloudCatData;
    private List<Item_CatchCat> selectedItems = new List<Item_CatchCat>();
    private float cardOriginY = -65f;
    private int selectedType;
    
    private List<Item> usedItems = new List<Item>();
    private int exp;
    private int coin;

    [HideInInspector] public bool IsTutorial;

    [Title("G8")] [SerializeField] private bool isG8;

    #endregion

    #region 基本開關+暫停離開

    public void Open(CloudCatData cloudCatData)
    {
        App.system.bgm.FadeIn().Play("Catch");
        uiView.Show();
        
        this.cloudCatData = cloudCatData;
        catSkin.ChangeSkin(cloudCatData);

        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string title = howToPlayData.titleData[country];
        string[] descripts = howToPlayData.descriptData[country];
        Sprite[] sprites = howToPlayData.sprites;
        App.system.howToPlay.SetData(title, descripts, sprites).Open(true, null, Init);
    }

    private void Close()
    {
        App.system.catchCat.Close();
        uiView.InstantHide();
    }

    private void CloseToLobby()
    {
        App.system.tnr.OnDoAdopt -= CloseToLobby;
        App.system.tnr.OnDoRelease -= CloseToLobby;
        App.system.tnr.OnDoShelter -= CloseToLobby;

        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            cardPersonalitys[i].isCanFlip = false;
            cardPersonalitys[i].DoPopCard();
        }

        App.system.transition.Active(1, () =>
        {
            Close();
            App.controller.lobby.Open();
        }, GameEndAction);
    }

    private void CloseToMap()
    {
        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            cardPersonalitys[i].isCanFlip = false;
            cardPersonalitys[i].DoPopCard();
        }

        App.system.transition.Active(1, () =>
        {
            Close();
            App.controller.map.Open();
        }, GameEndAction);
    }

    public void OpenPause()
    {
        App.system.howToPlay.Open(false);
    }

    public void Exit()
    {
        if (isG8)
        {
            App.system.confirm.Active(ConfirmTable.ExitComfirm, CloseToG8);
            return;
        }
        
        App.system.confirm.Active(ConfirmTable.ExitComfirm, () =>
        {
            SetCatNotUse();
            CloseToMap();
        });
    }

    private void GameEndAction()
    {
        if (isG8)
            return;
        
        App.system.player.AddExp(exp);
        App.system.player.Coin += coin;

        for (int i = 0; i < usedItems.Count; i++)
            usedItems[i].Count--;
    }

    private void CloseToG8()
    {
        if (!isG8)
            return;
        
        App.system.cloudSave.DeleteCloudCatData(cloudCatData);
        
        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            cardPersonalitys[i].isCanFlip = false;
            cardPersonalitys[i].DoPopCard();
        }

        App.system.transition.OnlyOpen();
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Close();
            App.system.findCat.ActiveCurrentGate();
        });
    }

    #endregion

    #region Get

    private List<Item_CatchCat> GetItems(int personality)
    {
        List<Item_CatchCat> result = new List<Item_CatchCat>();
        
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].personality != personality) continue;
            result.Add(items[i]);
        }

        return result;
    }

    private int GetTmpItemCount(Item_CatchCat itemCatchCat)
    {
        int count = itemCatchCat.Count;
        for (int i = 0; i < usedItems.Count; i++)
        {
            if (usedItems[i].id != itemCatchCat.id)
                continue;
            count--;
        }

        return count;
    }

    #endregion
    
    #region GameMethod
    
    private void Init()
    {
        usedItems.Clear();
        exp = 0;
        coin = 0;
        
        turn = 0;
        turnText.text = "0/7";
        runChanceText.text = "0%";

        lastTurnCatchButton.transform.DOKill();
        lastTurnCatchButton.SetActive(false);
        catchButtonParticle.Stop();
        
        hp = 100f;
        hpBar.DOFillAmount(hp / 100, 0.35f).From(0).SetEase(Ease.OutExpo);

        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            cardPersonalitys[i].isCanFlip = false;
            cardPersonalitys[i].DoPopCard();
            cardPersonalitys[i].SetActive(false);
        }
        
        catSkin.SetActive(true);
        
        CloseAction();
        DoResetTween();
        
        DoTopLeftTween();
        DOVirtual.DelayedCall(0.0625f, DoTopRightTween);
        DoPawTween();
        DoCenterTween();

        for (int i = 0; i < cardItems.Length; i++)
        {
            var rt = cardItems[i].transform as RectTransform;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, rt.anchoredPosition.y - rt.sizeDelta.y * 2);
        }
        
        //不要自動開啓交互所以不用SelectType
        selectedItems = GetItems(0);
        selectedType = 0;
        RefreshType();
        RefreshItemCount();
        DoCardsTween();
        
        DOVirtual.DelayedCall(2f, NextTurn);
    }

    private void NextTurn()
    {
        if (turn >= 7) return;

        catSkin.SetActive(true);
        
        if (turn < cardPersonalitys.Length)
        {
            if (turn < cloudCatData.CatData.PersonalityTypes.Count)
            {
                cardPersonalitys[turn].isCanFlip = true;
                cardPersonalitys[turn].SetActive(true);
                int personality = cloudCatData.CatData.PersonalityTypes[turn];
                int level = cloudCatData.CatData.PersonalityLevels[turn];
                cardPersonalitys[turn].SetData(personality, level);
            }
        }

        for (int i = 0; i < cardPersonalitys.Length; i++)
        {
            cardPersonalitys[i].DoPopCard();
        }

        DoTopGroupTween(() =>
        {
            if (turn == 7) //Last
            {
                CloseAction();
                
                lastTurnCatchButton.SetActive(true);
                catchButtonParticle.Play();
                
                Vector2 endSize = new Vector2(1.1f, 1.1f);
                lastTurnCatchButton.transform.DOScale(endSize, 0.1f).From(Vector2.one).SetEase(Ease.OutExpo)
                    .SetLoops(-1).SetDelay(0.5f);
                lastTurnCatchButton.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.InOutSine).SetDelay(0.6f)
                    .SetLoops(-1);
            }
            else
                OpenAction();
        });
    }

    private void RefreshTurn()
    {
        turn++;
        turnText.text = $"{turn}/7";
    }

    private void RefreshChance()
    {
        int tmp = turn - 1;
        if (tmp <= 0)
            tmp = 0;
        if (tmp >= 6)
            tmp = 6;
        runChanceText.text = $"{runChance[tmp] * 100:0}%";
    }

    // 允許玩家行動
    private void OpenAction()
    {
        for (int i = 0; i < chooseTypeButtons.Length; i++)
        {
            chooseTypeButtons[i].interactable = true;
        }

        for (int i = 0; i < cardItems.Length; i++)
        {
            cardItems[i].SetInteractable(true);
        }

        catchButton.interactable = true;
        blockRaycastObject.SetActive(false);
        
        DoTopLeftLight();
        DoTopLight();
        DoTopRightLight();
        DoBotLight();
    }

    // 不允許玩家行動
    private void CloseAction()
    {
        blockRaycastObject.SetActive(true);
        catchButton.interactable = false;

        for (int i = 0; i < chooseTypeButtons.Length; i++)
        {
            chooseTypeButtons[i].interactable = false;
        }

        for (int i = 0; i < cardItems.Length; i++)
        {
            cardItems[i].SetInteractable(false);
        }
        
        DoTopLeftDark();
        DoTopDark();
        DoTopRightDark();
        DoBotDark();
    }

    public void SelectType(int type)
    {
        App.system.soundEffect.Play("Button");
        
        if (selectedType == type)
            return;
        
        selectedType = type;
        selectedItems = GetItems(type);

        RefreshType();
        CloseAction();
        DoCardsTween();

        DOVirtual.DelayedCall(0.07f * 5, () =>
        {
            RefreshItemCount();
        });

        DOVirtual.DelayedCall(0.7f, OpenAction);
    }

    public void UseItem(int index)
    {
        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");
        
        var item = selectedItems[index];
        int tmpCount = GetTmpItemCount(item);
        
        if (item.level != 0 && tmpCount <= 0) //物品不是最低等級（無限使用） //物品數量0
            return;
        
        CloseAction();

        if (item.level != 0)
            usedItems.Add(item);
        
        float value;
        switch (index)
        {
            case 0:
                value = 6f;
                break;
            case 1:
                value = 10f;
                break;
            case 2:
                value = 14f;
                break;
            case 3:
                value = 18f;
                break;
            case 4:
                value = 30f;
                break;
            default:
                value = 6f;
                break;
        }

        var personality = cloudCatData.CatData.PersonalityTypes;
        var level = cloudCatData.CatData.PersonalityLevels;
        var availablePersonality = new List<int>(); //當前回合可以對比的個性
        var availableLevel = new List<int>();

        //取得當前回合可用的個性
        for (int i = 0; i < personality.Count; i++)
        {
            if (i >= turn) continue;
            availablePersonality.Add(personality[i]);
            availableLevel.Add(level[i]);
        }
        
        itemImage.sprite = item.icon;
        DoItemTween(() =>
        {
            if (availablePersonality.Contains(item.personality)) //有匹配的個性
            {
                for (int i = 0; i < availablePersonality.Count; i++)
                {
                    if (availablePersonality[i] != item.personality)
                        continue;

                    //看等級給值
                    switch (availableLevel[i])
                    {
                        case 0:
                            value *= 0f;
                            hateParticle.Play();
                            SpineCatAngry();
                            break;

                        case 1:
                            value *= 0.5f;
                            hateParticle.Play();
                            SpineCatAngry();
                            break;

                        case 2:
                            value *= 1.5f;
                            loveParticle.Play();
                            SpineCatHappy();
                            break;

                        case 3:
                            value *= 2f;
                            bigLoveParticle.Play();
                            SpineCatHappy();
                            break;

                        default:
                            loveParticle.Play();
                            SpineCatHappy();
                            break;
                    }

                    break;
                }
            }
            else
            {
                loveParticle.Play();
                SpineCatHappy();
            }
            
            //扣除生命值
            hp = Mathf.Clamp(hp - value, 0f, 100f);
            hpBar.DOFillAmount(hp / 100, 0.35f).SetEase(Ease.OutExpo);
        });

        DoCardTween(index);
        DOVirtual.DelayedCall(0.6f, () => RefreshItemCount());
    }

    public void Catch()
    {
        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");
        
        CloseAction();
        lastTurnCatchButton.SetActive(false);
        
        catchFlowerAnimator.SetTrigger("Catch");

        if (IsTutorial) //新手教學必定失敗
        {
            SpineCatCatchFail();
            return;
        }
        
        float chance = hp / 100;
        if (Random.value > chance)
            SpineCatCatchWin();
        else
            SpineCatCatchFail();
    }
    
    private bool CheckIsCatRun()
    {
        int index = turn - 1;
        if (index < 0)
            index = 0;
        
        float chance = runChance[index];
        if (isG8 && index < runChance.Length - 1)
            chance = 0;
        
        if (Random.value < chance)
            return true;
        return false;
    }

    private void RefreshItemCount()
    {
        for (int i = 0; i < selectedItems.Count; i++)
        {
            int tmpCount = GetTmpItemCount(selectedItems[i]);
            cardItems[i].SetData(selectedItems[i], tmpCount);
        }
    }

    private void RefreshType()
    {
        int type = selectedType;
        
        for (int i = 0; i < chooseTypeMasks.Length; i++)
        {
            if (i == type)
                chooseTypeMasks[i].SetActive(true);
            else
                chooseTypeMasks[i].SetActive(false);
        }
    }
    
    #endregion

    #region GameResult
    
    private void Gotcha()
    {
        catSkin.SetActive(false);

        if (isG8)
        {
            App.system.catchCat.ActiveG8End(cloudCatData, CloseToG8);
            return;
        }
        
        SetCatNotUse();
        
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.CatchGameSuccess, () =>
        {
            OnGameEnd?.Invoke();
            exp = App.system.player.playerDataSetting.CatchCatExp;
            coin = App.system.player.playerDataSetting.CatchCatCoin;
            App.system.settle.Active(exp, coin, 100, () =>
            {
                App.system.tnr.OnDoAdopt += CloseToLobby;
                App.system.tnr.OnDoRelease += CloseToLobby;
                App.system.tnr.OnDoShelter += CloseToLobby;
                App.system.tnr.Active(cloudCatData);
            });
        });
    }

    private void RunAway()
    {
        catSkin.SetActive(false);

        if (turn >= 7)
        {
            if (isG8)
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.CatchGameFailed, CloseToG8);
                return;
            }
            
            SetCatNotUse();
            
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.CatchCatGameEnd, () =>
            {
                if (hp <= 51)
                {
                    exp = App.system.player.playerDataSetting.CatchCatExp;
                    coin = App.system.player.playerDataSetting.CatchCatCoin;
                }

                App.system.settle.Active(exp, coin, 0, () =>
                {
                    OnGameEnd?.Invoke();
                    CloseToMap();
                });
            });

            return;
        }
        
        App.system.catchCat.runAway.Active(cloudCatData, NextTurn, () =>
        {
            SetCatNotUse();
            
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.CatchGameFailed, () =>
            {
                if (hp <= 51)
                {
                    exp = App.system.player.playerDataSetting.CatchCatExp;
                    coin = App.system.player.playerDataSetting.CatchCatCoin;
                }

                App.system.settle.Active(exp, coin, 0, () =>
                {
                    OnGameEnd?.Invoke();
                    CloseToMap();
                });
            });
        });
    }

    /// 把貓還回伺服器
    private void SetCatNotUse()
    {
        cloudCatData.CatSurviveData.IsUseToFind = false;
        App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);
    }
    
    #endregion

    #region Tween

    /// 所有動態的預設
    private void DoResetTween()
    {
        topFullMask.alpha = 0;
        topBlur.SetActive(false);
    }
    
    private void DoCardTween(int index)
    {
        var rt = cardItems[index].rectTransform;
        var canvasGroup = cardItems[index].canvasGroup;
        var origin = new Vector2(rt.anchoredPosition.x, cardOriginY);
        var upOffset = new Vector2(rt.anchoredPosition.x, cardOriginY + rt.sizeDelta.y);
        var downOffset = new Vector2(rt.anchoredPosition.x, cardOriginY - rt.sizeDelta.y);
        
        //Out
        rt.DOAnchorPos(upOffset, 0.5f).From(origin).SetEase(Ease.OutExpo);
        canvasGroup.DOFade(0, 0.5f).From(1).SetEase(Ease.OutExpo);
        
        //In
        rt.DOAnchorPos(origin, 0.2f).From(downOffset).SetEase(Ease.OutSine).SetDelay(0.6f);
        canvasGroup.DOFade(1, 0.2f).From(0).SetEase(Ease.OutSine).SetDelay(0.6f);
    }
    
    private void DoCardsTween()
    {
        //Out
        for (int i = 0; i < cardItems.Length; i++)
        {
            float offsetY = cardOriginY - cardItems[i].rectTransform.sizeDelta.y * 2;
            cardItems[i].rectTransform
                .DOAnchorPosY(offsetY, 0.07f)
                .SetEase(Ease.InExpo)
                .SetDelay(0.07f * i);
            cardItems[i].canvasGroup
                .DOFade(0, 0.07f)
                .SetDelay(0.07f * i);
        }

        DOVirtual.DelayedCall(0.07f * 5, () =>
        {
            //In
            for (int i = 0; i < cardItems.Length; i++)
            {
                cardItems[i].rectTransform
                    .DOAnchorPosY(cardOriginY, 0.07f)
                    .SetEase(Ease.OutExpo)
                    .SetDelay(0.07f * i);
                cardItems[i].canvasGroup
                    .DOFade(1, 0.1f)
                    .SetEase(Ease.InSine)
                    .SetDelay(0.07f * i);
            }
        });
    }

    private void DoPawTween()
    {
        for (int i = 0; i < chooseTypeButtons.Length; i++)
        {
            chooseTypeButtons[i].transform.DOScale(Vector2.one, 0.1f).From(Vector2.zero).SetDelay(i * 0.1f + 0.1f);
        }

        catchButton.transform.DOScale(Vector2.one, 0.1f).From(Vector2.zero).SetDelay(0.1f * 5);
    }
    
    private void DoTopLeftTween()
    {
        Vector2 exitOrigin = exitButtonRect.anchoredPosition;
        Vector2 exitOffset = new Vector2(exitOrigin.x, exitOrigin.y + exitButtonRect.sizeDelta.y * 2);
        exitButtonRect.DOAnchorPos(exitOrigin, 0.15f).From(exitOffset).SetEase(Ease.OutBack);
        
        Vector2 aboutOrigin = aboutButtonRect.anchoredPosition;
        Vector2 aboutOffset = new Vector2(aboutOrigin.x, aboutOrigin.y + aboutButtonRect.sizeDelta.y * 2);
        aboutButtonRect.DOAnchorPos(aboutOrigin, 0.15f).From(aboutOffset).SetEase(Ease.OutBack).SetDelay(0.0625f);
    }

    private void DoTopRightTween()
    {
        Vector2 ropeOrigin = ropeRect.anchoredPosition;
        Vector2 ropeOffset = new Vector2(ropeOrigin.x, ropeOrigin.y + ropeRect.sizeDelta.y * 2);
        ropeRect
            .DOAnchorPos(ropeOrigin, 0.15f).From(ropeOffset).SetEase(Ease.OutBack)
            .OnStart((() =>
            {
                for (int i = 0; i < cardPersonalitys.Length; i++)
                {
                    cardPersonalitys[i].transform.localScale = Vector2.zero;
                }
            }));

        DOVirtual.DelayedCall(0.15f, () =>
        {
            for (int i = 0; i < cardPersonalitys.Length; i++)
            {
                cardPersonalitys[i].transform.DOScale(Vector2.one, 0.15f).SetEase(Ease.OutBack).SetDelay(0.1f * i);
            }
        });
    }

    private void DoCenterTween()
    {
        catSkin.skeletonGraphic.DOColor(Color.white, 0.7f).From(Color.black).SetEase(Ease.InSine);
        hpBarRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);

        Vector2 barHeartOrigin = new Vector2(barHeartRect.anchoredPosition.x, 0);
        Vector2 barHeartOffset = new Vector2(barHeartOrigin.x, hpBarRect.sizeDelta.y + 2);
        barHeartRect.DOAnchorPos(barHeartOffset, 0.2f).From(barHeartOrigin).SetEase(Ease.OutBack).SetDelay(0.0625f + 0.25f);
        
        Vector2 barTitleOrigin = new Vector2(barTitleRect.anchoredPosition.x, 0);
        Vector2 barTitleOffset = new Vector2(barTitleOrigin.x, hpBarRect.sizeDelta.y + 2);
        barTitleRect.DOAnchorPos(barTitleOffset, 0.2f).From(barTitleOrigin).SetEase(Ease.OutBack).SetDelay(0.0625f * 2 + 0.25f);
    }

    private void DoItemTween(TweenCallback centerAction = null)
    {
        var itemRect = itemImage.transform as RectTransform;
        itemRect.localScale = Vector2.one;
        var origin = itemRect.anchoredPosition;
        var offset = new Vector2(origin.x - 1200f, origin.y);
        
        //In
        itemRect.DOAnchorPos(origin, 0.6f).From(offset).SetEase(Ease.OutExpo);
        itemRect.GetComponent<Image>().DOFade(255, 0.6f).From(0).SetEase(Ease.InExpo).OnComplete(centerAction);
        
        //Out
        itemRect.DOScale(new Vector2(1.5f, 1.5f), 0.15f).SetDelay(0.0625f + 0.6f);
        itemRect.DOScale(Vector2.zero, 0.2f).SetEase(Ease.InExpo).SetDelay(0.0625f + 0.6f + 0.15f);
    }

    private void DoTopBlurTween(bool tweenIn)
    {
        if (tweenIn)
        {
            topBlur.SetActive(true);
            topFullMask.DOFade(1, 0.25f).From(0).SetEase(Ease.OutExpo);
        }
        else
            topFullMask.DOFade(0, 0.25f).From(1).SetEase(Ease.OutExpo).OnComplete(() => topBlur.SetActive(false));
    }

    private void DoTopGroupTween(TweenCallback OnComplete = null)
    {
        float startY = -80;
        float endY = -250;
        topGroupRect.anchoredPosition = new Vector2(0, startY);
        Sequence seq = DOTween.Sequence();

        seq
            .AppendCallback(() => DoTopBlurTween(true))
            .AppendCallback(DoTopLight)
            .Append(topGroupRect.DOAnchorPosY(endY, 0.3f).SetEase(Ease.InOutCubic))
            .Join(topGroupRect.DOScale(new Vector2(0.98f, 0.98f), 0.1f).From(Vector2.one).SetEase(Ease.InOutSine))
            .Join(topGroupRect.DOScale(new Vector2(1.3f, 1.3f), 0.2f).SetEase(Ease.InOutSine).SetDelay(0.1f))
            .AppendInterval(0.4f)
            .Append(turnTextRect.DOScale(new Vector2(1.3f, 1.3f), 0.1f).From(Vector2.one).SetLoops(2, LoopType.Yoyo)
                .OnComplete(RefreshTurn))
            .AppendInterval(0.25f)
            .Append(chanceTextRect.DOScale(new Vector2(1.3f, 1.3f), 0.1f).From(Vector2.one)
                .SetLoops(2, LoopType.Yoyo).OnComplete(RefreshChance))
            .AppendInterval(0.4f)
            .Append(topGroupRect.DOAnchorPosY(startY, 0.3f).SetEase(Ease.InOutCubic))
            .Join(topGroupRect.DOScale(Vector2.one, 0.2f).SetEase(Ease.InOutCubic))
            .Join(topGroupRect.DOScale(new Vector2(1.03f, 1.03f), 0.2f).From(Vector2.one).SetEase(Ease.InOutCubic)
                .SetLoops(2, LoopType.Yoyo).SetDelay(0.3f))
            .OnComplete(() =>
            {
                DoTopBlurTween(false);
                OnComplete?.Invoke();
            });
    }
    
    #endregion

    #region DarkUI

    private void DoTopLeftDark()
    {
        exitDarkMask.DOFade(0.5f, 0.25f);
        aboutDarkMask.DOFade(0.5f, 0.25f);
    }

    private void DoTopDark()
    {
        turnDarkMask.DOFade(0.5f, 0.25f);
        chanceDarkMask.DOFade(0.5f, 0.25f);
    }

    private void DoTopRightDark()
    {
        for (int i = 0; i < cardPersonalitys.Length; i++)
            cardPersonalitys[i].DoDark();
    }

    private void DoBotDark()
    {
        botDarkMask.DOFade(0.5f, 0.25f).OnStart(() => botDarkMask.gameObject.SetActive(true));
    }

    #endregion

    #region LightUI

    private void DoTopLeftLight()
    {
        exitDarkMask.DOFade(0, 0.25f);
        aboutDarkMask.DOFade(0, 0.25f);
    }

    private void DoTopLight()
    {
        turnDarkMask.DOFade(0, 0.25f);
        chanceDarkMask.DOFade(0, 0.25f);
    }

    private void DoTopRightLight()
    {
        for (int i = 0; i < cardPersonalitys.Length; i++)
            cardPersonalitys[i].DoLight();
    }

    private void DoBotLight()
    {
        botDarkMask.DOFade(0, 0.25f).OnComplete(() => botDarkMask.gameObject.SetActive(false));
    }

    #endregion

    #region Spine

    private void SpineCatHappy()
    {
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Win", false);
        t.Complete += WaitSpineIdle;
    }

    private void SpineCatAngry()
    {
        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0)
            catSkin.SetAngry();
        
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", false);
        t.Complete += WaitSpineIdle;
    }

    private void WaitSpineIdle(TrackEntry trackEntry)
    {
        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0)
            catSkin.ChangeSkin(cloudCatData);
        
        trackEntry.Complete -= WaitSpineIdle;
        catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
        
        //檢查貓是否逃跑，若否開始下回合
        if (CheckIsCatRun())
            RunAway();
        else
            NextTurn();
    }

    private void SpineCatCatchFail()
    {
        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0)
            catSkin.SetAngry();
        
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", false);
        t.Complete += WaitSpineCatCatchFail;
    }

    private void SpineCatCatchWin()
    {
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Win", false);
        t.Complete += WaitSpineCatCatchWin;
    }

    private void WaitSpineCatCatchFail(TrackEntry trackEntry)
    {
        trackEntry.Complete -= WaitSpineCatCatchFail;
        
        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0)
            catSkin.ChangeSkin(cloudCatData);
        
        //檢查貓是否逃跑，若否開始下回合
        if (CheckIsCatRun())
            RunAway();
        else
            NextTurn();
    }

    private void WaitSpineCatCatchWin(TrackEntry trackEntry)
    {
        trackEntry.Complete -= WaitSpineCatCatchWin;
        Gotcha();
    }
    
    #endregion
}
