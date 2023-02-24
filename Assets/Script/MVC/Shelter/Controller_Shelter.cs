using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Controller_Shelter : ControllerBehavior
{
    [SerializeField] private Card_ChipInfo info;
    [SerializeField] private Scrollbar scrollbar;

    [Title("Quest")] [SerializeField] private SHR001 freeRefresh;
    [SerializeField] private SHR002 adsRefresh;

    public CallbackValue OnAdoptCat;

    #region Basic

    public void Init()
    {
        App.system.myTime.OnFirstLogin += ResetRefreshPerDay;
        App.system.myTime.OnAlreadyLogin += UpdateRefresh;

        freeRefresh.Init();
        adsRefresh.Init();

        GetCloudCatDatas();
    }

    public void Open()
    {
        App.system.bgm.FadeIn().Play("Shelter");
        App.view.shelter.Open();
        DOVirtual.DelayedCall(0.2f, () =>
        {
            App.view.shelter.npc.Click();
        });
        
        if (!App.system.tutorial.shelterTutorialEnd)
            App.system.tutorial.ActionShelterTutorial();
    }

    public void Close()
    {
        App.view.shelter.Close();
    }

    public void CloseByOpenMap()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            Close();
            App.controller.map.Open();
        });
    }

    public async void OpenAbandon()
    {
        if (await CheckShelterLimit())
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_ShelterFull);
            return;
        }
        
        App.system.abandon.Active("Shelter");
    }

    public void OpenSubShelter()
    {
        App.view.shelter.subShelter.Open();
    }

    public void CloseSubShelter()
    {
        App.view.shelter.subShelter.Close();
        info.CloseInfo();
    }

    #endregion

    #region Cats

    public async void GetCloudCatDatas()
    {
        List<CloudCatData> cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatasByOwner("Shelter", 12);

        App.model.shelter.CloudCatDatas = cloudCatDatas;

        for (int i = 0; i < App.view.shelter.cages.Length; i++)
        {
            App.view.shelter.cages[i].button.interactable = true;
            App.view.shelter.cages[i].catSkin.SetActive(true);
        }

        scrollbar.value = 0;
    }

    #endregion

    #region Refresh

    public void RefreshShelterCats()
    {
        if (!freeRefresh.IsReach)
        {
            App.system.confirm.Active(ConfirmTable.Hints_Refresh, () =>
            {
                GetCloudCatDatas();
                freeRefresh.Progress++;
                UpdateRefresh();
            });
            return;
        }

        if (!adsRefresh.IsReach)
        {
            //TODO Ads Refresh Confirm
            App.system.ads.Active(AdsType.ShelterRefresh, () =>
            {
                //TODO Ads
                GetCloudCatDatas();
                adsRefresh.Progress++;
                UpdateRefresh();

                if (App.model.shelter.AdsRefresh <= 0)
                    return;

                App.model.shelter.Cooldown = App.system.myTime.MyTimeNow.AddMinutes(1);
                InvokeRepeating(nameof(CooldownCounter), 1f, 1f);
            });
        }
    }

    private void CooldownCounter()
    {
        if (App.model.shelter.Cooldown <= App.system.myTime.MyTimeNow)
            CancelInvoke(nameof(CooldownCounter));
        App.model.shelter.Cooldown = App.model.shelter.Cooldown;
    }

    private void UpdateRefresh()
    {
        App.model.shelter.FreeRefresh = freeRefresh.TargetCount - freeRefresh.Progress;

        if (App.model.shelter.FreeRefresh > 0)
            return;

        App.model.shelter.AdsRefresh = adsRefresh.TargetCount - adsRefresh.Progress;
    }

    private void ResetRefreshPerDay()
    {
        freeRefresh.Progress = 0;
        adsRefresh.Progress = 0;
        UpdateRefresh();
    }

    #endregion

    #region Search

    public async void Search()
    {
        string searchCatId = App.view.shelter.inputField.text;

        if (string.IsNullOrEmpty(searchCatId)) // 空
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NullValue);
            return;
        }
        
        if (!await CheckInShelter(searchCatId)) // 不在收容所 
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CantFindCat);
            return;
        }

        var cloudCatData = await GetCloudCatData(searchCatId);

        if (cloudCatData != null)
        {
            if (!cloudCatData.CatHealthData.IsChip)
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CantFindCat);
                return;
            }

            App.model.shelter.SelectedAdoptCloudCatData = cloudCatData;
            OpenSubShelter();
        }
        else
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_CantFindCat);
        }
    }

    #endregion

    #region Adopt

    public void SelectAdopt(int index)
    {
        App.system.soundEffect.PlayCatMeow();
        App.model.shelter.SelectedAdoptCloudCatData = App.model.shelter.CloudCatDatas[index];
        App.model.shelter.SelectedCageIndex = index;
        print(App.model.shelter.SelectedAdoptCloudCatData.CatData.CatId);
        OpenSubShelter();
    }

    public async void Adopt()
    {
        CloseSubShelter();

        if (App.system.player.CanAdoptCatCount <= 0)
        {
            int count = App.system.room.FeatureRoomsCount;

            if (App.system.player.CatSlot >= count)
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NeedFeedRoom1);
            else
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NeedCatSlot1);

            return;
        }
        
        var cloudCatData = App.model.shelter.SelectedAdoptCloudCatData;

        if (!await CheckInShelter(cloudCatData.CatData.CatId))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_LateAdopt);
            CloseCage(cloudCatData.CatData.CatId);
            return;
        }

        App.system.confirm.Active(ConfirmTable.Hints_Adopt, okEvent: () =>
        {
            Cat cat = App.system.cat.CreateCatObject(cloudCatData);

            cat.GetHateSnack();
            cat.GetHateSoup();

            cloudCatData.CatData.Owner = App.system.player.PlayerId;

            cloudCatData.CatSurviveData.Satiety = Random.Range(50f, 69f);
            cloudCatData.CatSurviveData.Moisture = Random.Range(50f, 69f);
            cloudCatData.CatSurviveData.Favourbility = Random.Range(50f, 69f);
            cloudCatData.CatSurviveData.RealSatiety = 100f;
            cloudCatData.CatSurviveData.RealMoisture = 100f;
            cloudCatData.CatSurviveData.RealFavourbility = 100f;

            cloudCatData.CatDiaryData.AdoptLocation = "Shelter";
            cloudCatData.CatDiaryData.AdoptTimestamp = Timestamp.GetCurrentTimestamp();

            App.system.cloudSave.SaveCloudCatData(cloudCatData);

            CloseCage(cloudCatData.CatData.CatId);

            OnAdoptCat?.Invoke(cloudCatData);

            App.system.catRename.CantCancel().Active(cloudCatData, "Shelter",
                () =>
                {
                    DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_HasNewCat));
                });
        });
    }

    private void CloseCage(string catId)
    {
        App.view.shelter.cages[App.model.shelter.SelectedCageIndex].SetActive(false);

        //ValueChange
        List<CloudCatData> cats = App.model.shelter.CloudCatDatas;
        for (int i = cats.Count - 1; i >= 0; i--)
        {
            if (cats[i].CatData.CatId != catId) continue;
            cats.RemoveAt(i);
        }

        App.model.shelter.CloudCatDatas = cats;
    }

    private async Task<bool> CheckInShelter(string id)
    {
        var docRef = FirebaseFirestore.DefaultInstance.Collection("Cats").Document(id);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            try
            {
                string owner = snapshot.GetValue<string>("CatData.Owner");
                return owner == "Shelter";
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

    #endregion

    #region Chip

    public void ToggleInfo()
    {
        info.ToggleInfo();
    }

    #endregion

    #region SubShelter

    public void CopySubShelterCatId()
    {
        string CatId = App.model.shelter.SelectedAdoptCloudCatData.CatData.CatId;
        CatId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Copy);
    }

    #endregion

    private async Task<bool> CheckShelterLimit()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        
        int totalCats;
        int shelterCats;
        
        DocumentReference docRef = db.Collection("WorldData").Document("Total");
        var snapshot = await docRef.GetSnapshotAsync();
        totalCats = snapshot.GetValue<int>("CatCount");
        shelterCats = snapshot.GetValue<int>("ShelterCount");

        return shelterCats >= totalCats;
    }

    private async Task<CloudCatData> GetCloudCatData(string id)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(id);
        var snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
            return snapshot.ConvertTo<CloudCatData>();

        return null;
    }
}