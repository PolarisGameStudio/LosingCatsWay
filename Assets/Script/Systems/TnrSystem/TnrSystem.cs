using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TnrSystem : MvcBehaviour
{
    [SerializeField] private UIView uiView;
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private GameObject infoMask;
    [SerializeField] private GameObject idMask;
    [SerializeField] private Card_ChipInfo info;
    [SerializeField] private Image locationBg;

    [Title("Currency")] [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("Masks")] [SerializeField] private GameObject adoptMask;
    [SerializeField] private GameObject ligationMask;

    [Title("Spine")] [SerializeField] private SkeletonGraphic functionGraphic;
    [SerializeField] private SkeletonGraphic catGraphic;

    [Title("Tween")] [SerializeField] private RectTransform currencyRect;
    [SerializeField] private RectTransform infoButtonRect;
    [SerializeField] private RectTransform idRect;
    [SerializeField] private RectTransform ligationButtonRect;
    [SerializeField] private RectTransform titleRect;
    [SerializeField] private RectTransform[] bottomButtonRects;
    
    private CloudCatData cloudCatData;
    private string _location;
    
    public Callback OnDoAdopt;
    public Callback OnDoRelease;
    public Callback OnDoShelter;
    public Callback OnDoLigation;
    
    public CallbackValue OnAdoptCat;

    public void Init()
    {
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnDiamondChange(object value)
    {
        int diamond = (int)value;
        diamondText.text = diamond.ToString();
    }

    private void OnCoinChange(object value)
    {
        int coins = (int)value;
        coinText.text = coins.ToString();
    }

    private void Open()
    {
        adoptMask.SetActive(App.system.player.CanAdoptCatCount <= 0);
        ligationMask.SetActive(cloudCatData.CatHealthData.IsLigation);
        
        uiView.Show();
        
        TweenInit();
        TweenIn();
    }

    public void Close()
    {
        uiView.InstantHide();
    }

    public void Active(CloudCatData cloudCatData, string location)
    {
        this.cloudCatData = cloudCatData;
        catSkin.ChangeSkin(cloudCatData);

        if (cloudCatData.CatHealthData.IsChip)
            info.SetData(cloudCatData);
        
        infoMask.SetActive(!cloudCatData.CatHealthData.IsChip);
        idMask.SetActive(!cloudCatData.CatHealthData.IsChip);
        idText.text = cloudCatData.CatHealthData.IsChip ? $"ID:{cloudCatData.CatData.CatId}" : "ID:--";

        _location = location;
        locationBg.sprite = App.factory.catFactory.GetCatLocationSprite(location);
        
        Open();
    }

    public void ToggleInfo() //顯示或關閉晶片內容
    {
        info.ToggleInfo();
    }

    public void CopyId()
    {
        string catId = cloudCatData.CatData.CatId;
        catId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
    }

    public void DoAdopt()
    {
        Close();
        App.system.confirm.Active(ConfirmTable.AdoptConfirm, () =>
        {
            App.system.catRename.CantCancel().Active(cloudCatData, _location, () =>
            {
                cloudCatData.CatData.Owner = App.system.player.PlayerId;
                App.system.cloudSave.UpdateCloudCatData(cloudCatData);

                cloudCatData.CatSurviveData.IsUseToFind = false;
                App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);

                cloudCatData.CatDiaryData.AdoptLocation = "OutSide";
                App.system.cloudSave.UpdateCloudCatDiaryData(cloudCatData);

                Cat cat = App.system.cat.CreateCatObject(cloudCatData);
                cat.GetLikeSnack();
                cat.GetLikeSoup();
                
                DOVirtual.DelayedCall(0.1f, () => 
                    App.system.confirm.OnlyConfirm().Active(ConfirmTable.HasNewCat, () =>
                    {
                        OnAdoptCat?.Invoke(cloudCatData);
                        OnDoAdopt?.Invoke();
                    }));
            });
        }, () => Open());
    }

    public void DoRelease() //原放
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            cloudCatData.CatSurviveData.IsUseToFind = false;
            App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);

            DOVirtual.DelayedCall(0.1f, () => 
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix, () =>
                {
                    OnDoRelease?.Invoke();
                    Close();
                }));

        }, () => Open());
    }

    public void DoShelter() //送去收容所
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            cloudCatData.CatData.Owner = "Shelter";
            App.system.cloudSave.UpdateCloudCatData(cloudCatData);

            cloudCatData.CatSurviveData.IsUseToFind = false;
            App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);

            DOVirtual.DelayedCall(0.1f, () => 
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix, () =>
                {
                    OnDoShelter?.Invoke();
                    Close();
                }));
        }, () => Open());
    }

    public void DoLigation() //送去結紮
    {
        //Close();
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            if (!App.system.player.ReduceMoney(200))
            {
                DOVirtual.DelayedCall(0.1f,
                    () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.NoMoney));
                return;
            }

            TweenOut();
            
            cloudCatData.CatHealthData.IsLigation = true;
            App.system.cloudSave.UpdateCloudCatHealthData(cloudCatData);

            functionGraphic.gameObject.SetActive(true);
            functionGraphic.AnimationState.SetAnimation(0, "Hospital_Tool/Operation", false);
            catGraphic.AnimationState.SetAnimation(0, "Hospital_Cat/Operation_Cat", false);
            functionGraphic.AnimationState.Complete += DoLigationComplete;
            OnDoLigation?.Invoke();
        }, () =>
        {
            Open();
        });
    }

    private void DoLigationComplete(TrackEntry trackentry)
    {
        trackentry.Complete -= DoLigationComplete;
        catGraphic.AnimationState.SetAnimation(0, "AI_Main/Sleep_01", true);
        Open();
        functionGraphic.gameObject.SetActive(false);
    }

    #region DoTween

    private void TweenInit()
    {
        currencyRect.localScale = Vector2.zero;
        infoButtonRect.localScale = Vector2.zero;
        idRect.localScale = Vector2.zero;
        ligationButtonRect.localScale = Vector2.zero;
        titleRect.localScale = Vector2.zero;

        for (int i = 0; i < bottomButtonRects.Length; i++)
        {
            bottomButtonRects[i].localScale = Vector2.zero;
        }
    }
    
    private void TweenIn()
    {
        currencyRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);
        infoButtonRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1625f);
        idRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1625f * 2);
        ligationButtonRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1625f * 2);
        titleRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.1625f * 2);

        for (int i = 0; i < bottomButtonRects.Length; i++)
        {
            bottomButtonRects[i].DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f * i);
        }
    }

    private void TweenOut()
    {
        currencyRect.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack);
        infoButtonRect.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack).SetDelay(0.1625f);
        idRect.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack).SetDelay(0.1625f * 2);
        ligationButtonRect.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack).SetDelay(0.1625f * 2);
        titleRect.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.OutBack).SetDelay(0.1625f * 2);

        for (int i = 0; i < bottomButtonRects.Length; i++)
        {
            bottomButtonRects[i].DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack).SetDelay(0.25f * i);
        }
    }

    #endregion
}
