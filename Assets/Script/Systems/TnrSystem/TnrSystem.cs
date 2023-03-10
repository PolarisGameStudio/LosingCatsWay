using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    public void ToggleInfo() //???????????????????????????
    {
        info.ToggleInfo();
    }

    public void CopyId()
    {
        string catId = cloudCatData.CatData.CatId;
        catId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Copy);
    }

    public async void DoAdopt()
    {
        Close();

        if (!await CheckInLocation(cloudCatData.CatData.CatId, _location))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_LateAdopt);
            return;
        }
        
        App.system.confirm.Active(ConfirmTable.Hints_Adopt, () =>
        {
            App.system.catRename.CantCancel().Active(cloudCatData, _location, () =>
            {
                cloudCatData.CatData.Owner = App.system.player.PlayerId;

                cloudCatData.CatSurviveData.IsUseToFind = false;

                cloudCatData.CatSurviveData.Satiety = Random.Range(50f, 69f);
                cloudCatData.CatSurviveData.Moisture = Random.Range(50f, 69f);
                cloudCatData.CatSurviveData.Favourbility = Random.Range(50f, 69f);
                cloudCatData.CatSurviveData.RealSatiety = 100f;
                cloudCatData.CatSurviveData.RealMoisture = 100f;
                cloudCatData.CatSurviveData.RealFavourbility = 100f;

                cloudCatData.CatDiaryData.AdoptLocation = "OutSide";

                Cat cat = App.system.cat.CreateCatObject(cloudCatData);
                cat.GetHateSnack();
                cat.GetHateSoup();
                
                App.system.cloudSave.SaveCloudCatData(cloudCatData);

                DOVirtual.DelayedCall(0.1f, () =>
                    App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_HasNewCat, () =>
                    {
                        OnAdoptCat?.Invoke(cloudCatData);
                        OnDoAdopt?.Invoke();
                    }));
            });
        }, () => Open());
    }

    public void DoRelease() //??????
    {
        App.system.confirm.Active(ConfirmTable.Hints_Release1, () =>
        {
            cloudCatData.CatSurviveData.IsUseToFind = false;
            App.system.cloudSave.SaveCloudCatData(cloudCatData);

            DOVirtual.DelayedCall(0.1f, () =>
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Release2, () =>
                {
                    OnDoRelease?.Invoke();
                    Close();
                }));
        }, () => Open());
    }

    public void DoShelter() //???????????????
    {
        App.system.confirm.Active(ConfirmTable.Hints_Shelter1, () =>
        {
            cloudCatData.CatData.Owner = "Shelter";
            cloudCatData.CatSurviveData.IsUseToFind = false;
            App.system.cloudSave.SaveCloudCatData(cloudCatData);

            DOVirtual.DelayedCall(0.1f, () =>
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Shelter2, () =>
                {
                    OnDoShelter?.Invoke();
                    Close();
                }));
        }, () => Open());
    }

    public void DoLigation() //????????????
    {
        //Close();
        App.system.confirm.Active(ConfirmTable.Hints_Ligation, () =>
        {
            if (!App.system.player.ReduceMoney(400))
            {
                DOVirtual.DelayedCall(0.1f,
                    () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NoMoney));
                return;
            }

            App.controller.pedia.AddLigationCount(cloudCatData.CatData.Variety);
            TweenOut();

            cloudCatData.CatHealthData.IsLigation = true;
            App.system.cloudSave.SaveCloudCatData(cloudCatData);

            functionGraphic.gameObject.SetActive(true);
            functionGraphic.AnimationState.SetAnimation(0, "Hospital_Tool/Operation", false);
            catGraphic.AnimationState.SetAnimation(0, "Hospital_Cat/Operation_Cat", false);
            functionGraphic.AnimationState.Complete += DoLigationComplete;
            OnDoLigation?.Invoke();
        }, () => { Open(); });
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
            bottomButtonRects[i].DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack)
                .SetDelay(0.25f * i);
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
            bottomButtonRects[i].DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack)
                .SetDelay(0.25f * i);
        }
    }

    #endregion
    
    private async Task<bool> CheckInLocation(string id, string location)
    {
        var docRef = FirebaseFirestore.DefaultInstance.Collection("Cats").Document(id);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            try
            {
                string owner = snapshot.GetValue<string>("CatData.Owner");
                return owner == location;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        Debug.LogError("Document not fount");
        return false;
    }
}