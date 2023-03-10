using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Spine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class CatchCatMap : MvcBehaviour
{
    public Callback OnGameEnd;
    public Callback OnGotcha;

    #region Variable

    [SerializeField] private HowToPlayData howToPlayData;

    [Title("UIView")] [SerializeField] private UIView uiView;

    [Title("Items")] [SerializeField] private Item_CatchCat[] items;

    [Title("CatSkin")] [SerializeField] private CatSkin catSkin;

    [Title("UI")] [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button[] chooseTypeButtons; //選擇種類按鈕*4
    [SerializeField] private Button catchButton; //捕捉按鈕
    [SerializeField] private Card_CatchItem[] cardItems; //道具卡*5
    [SerializeField] private GameObject[] chooseTypeMasks;
    [SerializeField] private GameObject blockRaycastObject;

    [TabGroup("Top")] [SerializeField] private RectTransform topRect;
    [TabGroup("Top")] [SerializeField] private Image turnDarkMask;
    [TabGroup("Top")] [SerializeField] private CanvasGroup topFullMask;
    [TabGroup("Top")] [SerializeField] private GameObject topBlur;
    [TabGroup("Top")] [SerializeField] private RectTransform turnTextRect;

    [TabGroup("TopLeft")] [SerializeField] private RectTransform exitButtonRect;
    [TabGroup("TopLeft")] [SerializeField] private RectTransform aboutButtonRect;
    [TabGroup("TopLeft")] [SerializeField] private Image exitDarkMask;
    [TabGroup("TopLeft")] [SerializeField] private Image aboutDarkMask;

    [TabGroup("Item")] [SerializeField] private Image itemImage;

    [TabGroup("Bot")] [SerializeField] private CanvasGroup botDarkMask;
    [TabGroup("Bot")] [SerializeField] private GameObject lastTurnCatchButton;

    [Title("Animator")] [SerializeField] private Animator catchFlowerAnimator;

    [Title("UIParticle")]
    [SerializeField] private GameObject hateEffect;
    [SerializeField] private GameObject loveEffect;
    [SerializeField] private GameObject bigLoveEffect;

    [Title("Module")] [SerializeField] private CatchCatBubble bubble;
    [SerializeField] private CatchCatHealthBar healthBar;

    private int turn;
    private float[] runChances = { 0f, 0f, 0.03f, 0.05f, 0.1f, 0.2f, 1f };
    private float hp; //生命值
    private CloudCatData cloudCatData;
    private List<Item_CatchCat> selectedItems = new List<Item_CatchCat>();
    private float cardOriginY = 0f;
    private int selectedType;

    private List<Item> usedItems = new List<Item>();
    private int exp;
    private int money;

    private string gameName;

    #endregion

    #region 基本開關+暫停離開

    public void Open(CloudCatData cloudCatData)
    {
        App.system.bgm.FadeIn().Play("Catch");
        uiView.Show();

        this.cloudCatData = cloudCatData;
        catSkin.ChangeSkin(cloudCatData);
        
        hateEffect.SetActive(false);
        loveEffect.SetActive(false);
        bigLoveEffect.SetActive(false);

        if (cloudCatData.CatData.SurviveDays <= 3)
            bubble.transform.localScale = new Vector2(2f, 2f);
        else
            bubble.transform.localScale = Vector2.one;

        bubble.Init(cloudCatData.CatData.PersonalityTypes, cloudCatData.CatData.PersonalityLevels);
        healthBar.Init();

        if (!App.system.tutorial.isTutorial)
            ShowHowToPlay();
    }

    public void ShowHowToPlay()
    {
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        gameName = howToPlayData.titleData[country];
        string[] descripts = howToPlayData.descriptData[country];
        Sprite[] sprites = howToPlayData.sprites;
        App.system.howToPlay.SetData(gameName, descripts, sprites).Open(true, null, Init);
    }

    private void Close()
    {
        cloudCatData = null;
        App.system.catchCat.Close();
        uiView.InstantHide();
    }

    private void CloseToLobby()
    {
        App.system.tnr.OnDoAdopt -= CloseToLobby;
        App.system.tnr.OnDoRelease -= CloseToLobby;
        App.system.tnr.OnDoShelter -= CloseToLobby;

        App.system.transition.Active(0.5f, () =>
        {
            Close();
            GameEndAction();
            App.controller.lobby.Open();
        });
    }

    public void CloseToMap()
    {
        App.system.transition.Active(0.5f, () =>
        {
            Close();
            GameEndAction();
            App.controller.map.Open();
        });
    }

    public void OpenPause()
    {
        if (App.system.tutorial.isTutorial)
            return;
        App.system.howToPlay.Open(false);
    }

    public void Exit()
    {
        if (App.system.tutorial.isTutorial)
            return;
        App.system.confirm.Active(ConfirmTable.Hints_Leave, () =>
        {
            SetCatNotUse();
            CloseToMap();
        });
    }

    private void GameEndAction()
    {
        for (int i = 0; i < usedItems.Count; i++)
            usedItems[i].Count--;

        if (App.system.tutorial.isTutorial)
            return;

        if (exp > 0)
            App.system.player.AddExp(exp);
        if (money > 0)
            App.system.player.AddMoney(money);
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
        money = 0;

        turn = 0;
        turnText.text = "0/7";

        lastTurnCatchButton.transform.DOKill();
        lastTurnCatchButton.SetActive(false);

        hp = 100f;
        healthBar.ChangeBarValue(hp / 100f);

        catSkin.SetActive(true);

        CloseAction();
        DoResetTween();

        DoTopLeftTween();
        DoPawTween();
        DoCenterTween();

        healthBar.Open();

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
        DOVirtual.DelayedCall(3.75f, bubble.Open);
    }

    private void NextTurn()
    {
        if (turn >= 7) return;

        catSkin.SetActive(true);

        DoTopTween(() =>
        {
            if (turn == 7) //Last
            {
                CloseAction();

                lastTurnCatchButton.SetActive(true);

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
        App.system.soundEffect.Play("ED00041");
        App.system.soundEffect.Play("ED00057");
        
        turn++;
        turnText.text = $"{turn}/7";
    }

    // 允許玩家行動
    private void OpenAction()
    {
        for (int i = 0; i < chooseTypeButtons.Length; i++)
            chooseTypeButtons[i].interactable = true;

        for (int i = 0; i < cardItems.Length; i++)
            cardItems[i].SetInteractable(true);

        catchButton.interactable = true;
        blockRaycastObject.SetActive(false);

        bubble.Open();

        DoTopLeftLight();
        DoTopLight();
        DoBotLight();
    }

    // 不允許玩家行動
    private void CloseAction()
    {
        blockRaycastObject.SetActive(true);
        catchButton.interactable = false;

        for (int i = 0; i < chooseTypeButtons.Length; i++)
            chooseTypeButtons[i].interactable = false;

        for (int i = 0; i < cardItems.Length; i++)
            cardItems[i].SetInteractable(false);

        bubble.Close();

        DoTopLeftDark();
        DoTopDark();
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

        DOVirtual.DelayedCall(0.07f * 5, () => { RefreshItemCount(); });

        DOVirtual.DelayedCall(0.7f, OpenAction);
    }

    public void UseItem(int index)
    {
        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");

        var item = selectedItems[index];
        int tmpCount = GetTmpItemCount(item);
        int tmpTurn = turn - 1;

        if (item.level != 0 && tmpCount <= 0) //物品不是最低等級（無限使用） //物品數量0
            return;

        CloseAction();

        if (item.level != 0)
            usedItems.Add(item);

        float value;
        switch (index)
        {
            case 0:
                value = 5f;
                break;
            case 1:
                value = 8f;
                break;
            case 2:
                value = 12f;
                break;
            case 3:
                value = 15f;
                break;
            case 4:
                value = 30f;
                break;
            default:
                value = 5f;
                break;
        }

        hateEffect.SetActive(false);
        loveEffect.SetActive(false);
        bigLoveEffect.SetActive(false);
        
        var personalitys = cloudCatData.CatData.PersonalityTypes;
        var levels = cloudCatData.CatData.PersonalityLevels;
        
        itemImage.sprite = item.icon;
        DoItemTween(() =>
        {
            if (personalitys.Contains(item.personality)) //該回合有匹配的個性
            {
                for (int i = 0; i < personalitys.Count; i++)
                {
                    if (personalitys[i] != item.personality)
                        continue;

                    //看等級給值
                    switch (levels[i])
                    {
                        case 0:
                            value *= -0.5f;

                            if (index == 4) //廣告道具 + 負面個性
                                value *= 0.2f;

                            hateEffect.SetActive(true);
                            SpineCatAngry();
                            break;
                        case 1:
                            value *= -0.25f;

                            if (index == 4) //廣告道具 + 負面個性
                                value *= 0.2f;

                            hateEffect.SetActive(true);
                            SpineCatAngry();
                            break;
                        case 2:
                            value *= 1.5f;
                            loveEffect.SetActive(true);
                            SpineCatHappy();
                            break;
                        case 3:
                            value *= 2f;
                            bigLoveEffect.SetActive(true);
                            SpineCatHappy();
                            break;
                        default:
                            loveEffect.SetActive(true);
                            SpineCatHappy();
                            break;
                    }

                    break;
                }
            }
            else //該回合沒有匹配的個性
            {
                loveEffect.SetActive(true);
                SpineCatHappy();
            }

            var itemOffset = new List<float> { 1f, 1.04f, 1.1f, 1.18f, 1.28f, 1.4f, 1.4f };
            value *= itemOffset[tmpTurn];

            //扣除生命值
            hp = Mathf.Clamp(hp - value, 0f, 100f);
            healthBar.ChangeBarValue(hp / 100);
        });

        App.system.soundEffect.Play("ED00004");
        DoCardTween(index);
        DOVirtual.DelayedCall(0.6f, RefreshItemCount);
    }

    public void Catch()
    {
        App.system.soundEffect.Play("ED00043");
        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");

        CloseAction();
        lastTurnCatchButton.SetActive(false);

        catchFlowerAnimator.SetTrigger("Catch");

        if (App.system.tutorial.isTutorial) //新手教學必定失敗
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
        if (turn >= 7)
            return true;

        int index = turn - 1;
        if (index < 0)
            index = 0;

        float chance = runChances[index];
        chance = Mathf.Clamp(chance, 0f, 100f);

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
        bubble.Close();

        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CatCatchSuccess, () =>
        {
            OnGameEnd?.Invoke();
            OnGotcha?.Invoke();
            exp = App.system.player.playerDataSetting.CatchCatExp;
            money = App.system.player.playerDataSetting.CatchCatCoin;
            
            App.system.settle.Active(gameName, cloudCatData, exp, money, 0, 3, null, () =>
            {
                App.system.tnr.OnDoAdopt += CloseToLobby;
                App.system.tnr.OnDoRelease += CloseToLobby;
                App.system.tnr.OnDoShelter += CloseToLobby;
                App.system.tnr.Active(cloudCatData, App.system.catchCat.Location);
            });
        });
    }

    private void RunAway()
    {
        App.system.soundEffect.Play("ED00062");
        
        if (App.system.tutorial.isTutorial)
        {
            catSkin.SetActive(false);
            bubble.Close();

            App.system.cloudSave.DeleteCloudCatData(cloudCatData);
            cloudCatData = null;

            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CatCatchFail, () =>
            {
                // CloseToMap();
                App.system.tutorial.Next();
            });
            return;
        }

        if (turn >= 7)
        {
            catSkin.SetActive(false);
            bubble.Close();
            SetCatNotUse();

            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CatCatchFail, () =>
            {
                if (hp <= 51)
                {
                    exp = App.system.player.playerDataSetting.CatchCatExp / 2;
                    money = App.system.player.playerDataSetting.CatchCatCoin / 2;
                }

                App.system.settle.Active(gameName, cloudCatData, exp, money, 0, 0, null, () =>
                {
                    OnGameEnd?.Invoke();
                    CloseToMap();
                });
            });

            return;
        }

        UnityAction cancelEvent = () =>
        {
            catSkin.SetActive(false);
            bubble.Close();
            SetCatNotUse();

            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CatCatchFail, () =>
            {
                if (hp <= 51)
                {
                    exp = App.system.player.playerDataSetting.CatchCatExp;
                    money = App.system.player.playerDataSetting.CatchCatCoin;
                }

                App.system.settle.Active(gameName, cloudCatData, exp, money, 0, 0, null, () =>
                {
                    OnGameEnd?.Invoke();
                    CloseToMap();
                });
            });
        };

        App.system.catchCat.runAway.Active(cloudCatData,
            () => { App.system.ads.Active(AdsType.CatchCatRun, NextTurn); }, cancelEvent.Invoke);
    }

    /// 把貓還回伺服器
    //private void SetCloudCatDataToUse(bool value)
    // {
    //     if (App.system.tutorial.isTutorial)
    //         return;
    //     if (cloudCatData == null)
    //         return;
    //     cloudCatData.CatSurviveData.IsUseToFind = value;
    //     App.system.cloudSave.SaveCloudCatData(cloudCatData);
    // }

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

    private void DoCenterTween()
    {
        catSkin.skeletonGraphic.DOColor(Color.white, 0.7f).From(Color.black).SetEase(Ease.InSine);
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

    [Button]
    private void DoTopTween(TweenCallback OnComplete = null)
    {
        float startY = -80;
        float endY = -250;
        topRect.anchoredPosition = new Vector2(0, startY);
        Sequence seq = DOTween.Sequence();

        seq
            //模糊畫面
            .AppendCallback(() => DoTopBlurTween(true))
            //靈動島亮起
            .AppendCallback(DoTopLight)
            //靈動島往下
            .Append(topRect.DOAnchorPosY(endY, 0.3f).SetEase(Ease.InOutCubic))
            //往下過程內縮
            .Join(topRect.DOScale(new Vector2(0.95f, 0.95f), 0.1f).From(Vector2.one).SetEase(Ease.Linear))
            //往下抵達張開
            .Join(topRect.DOScale(new Vector2(1.5f, 1.5f), 0.2f).SetEase(Ease.Linear).SetDelay(0.1f))
            .AppendInterval(0.35f)
            //回合字跳動
            .Append(turnTextRect.DOScale(new Vector2(1.5f, 1.5f), 0.1f).From(Vector2.one).SetLoops(2, LoopType.Yoyo)
                .OnComplete(RefreshTurn))
            .AppendInterval(0.25f)
            //靈動島往上回原點
            .Append(topRect.DOAnchorPosY(startY, 0.3f).SetEase(Ease.InOutCubic))
            //往上過程恢復原比例
            .Join(topRect.DOScale(Vector2.one, 0.15f).SetEase(Ease.InOutCubic))
            //抵達時比例彈動
            .Join(topRect.DOScale(new Vector2(1.03f, 1.03f), 0.2f).From(Vector2.one).SetEase(Ease.InOutCubic)
                .SetLoops(2, LoopType.Yoyo).SetDelay(0.2f))
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
    }

    private void DoBotLight()
    {
        botDarkMask.DOFade(0, 0.25f).OnComplete(() => botDarkMask.gameObject.SetActive(false));
    }

    #endregion

    #region Spine

    private void SpineCatHappy()
    {
        App.system.soundEffect.Play("ED00061");
        
        string animationName = cloudCatData.CatData.SurviveDays <= 3
            ? "Catch_Cat/Catch_Win"
            : "Rearing_Cat/Rearing_Rub_IDLE";
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, animationName, false);
        t.Complete += WaitSpineIdle;
    }

    private void SpineCatAngry()
    {
        if (cloudCatData.CatData.SurviveDays > 3)
            catSkin.SetAngry();

        App.system.soundEffect.Play("ED00062");
        
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", false);
        t.Complete += WaitSpineIdle;
    }

    private void WaitSpineIdle(TrackEntry trackEntry)
    {
        if (cloudCatData.CatData.SurviveDays > 3)
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
        if (cloudCatData.CatData.SurviveDays > 3)
            catSkin.SetAngry();

        App.system.soundEffect.Play("ED00046");
        
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Lose", false);
        t.Complete += WaitSpineCatCatchFail;
    }

    private void SpineCatCatchWin()
    {
        TrackEntry t = catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Catch_Cat/Catch_Win", false);
        t.Complete += WaitSpineCatCatchWin;

        if (cloudCatData.CatData.SurviveDays > 3)
            catSkin.SetLove();

        App.system.soundEffect.Play("ED00029");
    }

    private void WaitSpineCatCatchFail(TrackEntry trackEntry)
    {
        trackEntry.Complete -= WaitSpineCatCatchFail;

        if (cloudCatData.CatData.SurviveDays > 3)
            catSkin.ChangeSkin(cloudCatData);

        //檢查貓是否逃跑，若否開始下回合
        if (CheckIsCatRun())
            RunAway();
        else
            NextTurn();
    }

    private void WaitSpineCatCatchWin(TrackEntry trackEntry)
    {
        catSkin.ChangeSkin(cloudCatData);

        trackEntry.Complete -= WaitSpineCatCatchWin;
        Gotcha();
    }

    #endregion

    #region ApplicationProcess

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (App.system.settle.IsActivate)
                SetCatNotUse();
            else
                ClearCat();
        }
        else
        {
            SetCatToUse();
        }
    }

    #endregion

    private void SetCatNotUse()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (cloudCatData == null)
            return;
        
        cloudCatData.CatSurviveData.IsUseToFind = false;
        App.system.cloudSave.SaveCloudCatData(cloudCatData);
    }

    private void SetCatToUse()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (cloudCatData == null)
            return;
        
        cloudCatData.CatSurviveData.IsUseToFind = true;
        App.system.cloudSave.SaveCloudCatData(cloudCatData);
    }
    
    private void ClearCat()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        if (cloudCatData == null)
            return;

        cloudCatData.CatSurviveData.IsUseToFind = false;
        App.system.cloudSave.SaveCloudCatData(cloudCatData);
        
        // 教學時
        App.system.howToPlay.ClearEventAndClose();
        App.system.tnr.Close();
        App.system.catchCat.runAway.Close();
        
        cloudCatData = null;
        App.system.howToPlay.ClearEventAndClose();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Left, CloseToMap);
    }
}