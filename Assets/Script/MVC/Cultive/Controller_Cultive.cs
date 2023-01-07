using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Doozy.Runtime.Common.Extensions;
using Doozy.Runtime.UIManager.Components;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Controller_Cultive : ControllerBehavior
{
    [SerializeField] private Drop_Cultive catDropSensor;
    [SerializeField] private GameObject catClickObject;
    [SerializeField] private Drop_Cultive littleDropSensor;
    [SerializeField] private PopValue_Cultive satietyPop;
    [SerializeField] private PopValue_Cultive moisturePop;
    [SerializeField] private PopValue_Cultive funPop;

    [Title("Spine")] public SkeletonGraphic catSkeleton;

    [Title("Effects")] [SerializeField] private UIParticle satietyEffect;
    [SerializeField] private UIParticle moistureEffect;
    [SerializeField] private UIParticle funEffects;

    [Title("LitterPop")] [SerializeField] private PopValue_Cultive diamondPop;
    [SerializeField] private PopValue_Cultive moneyPop;
    [SerializeField] private PopValue_Cultive shitPop;

    [Title("Tutorial")] [SerializeField] private Button closeButton;
    [SerializeField] private UIButton infoButton;
    [SerializeField] private UIButton screenshotButton;
    [SerializeField] private Button[] tabButtons;

    private bool isOpen;
    private bool isRecordSkin;

    [Title("Sensor")]
    [ReadOnly] public bool isCanDrag = true;
    [ReadOnly] public bool isDragging; // 不能同時Drag數個物件

    private string skinBeforePreview;
    
    public Callback OnFeedFood;
    public Callback OnFeedWater;
    public Callback OnChangeLitter;
    public CallbackValue OnPlayCat;
    
    public CallbackValue OnAddFun;
    public CallbackValue OnAddSatiety;
    public CallbackValue OnAddMoisture;

    #region Basic

    public void Open()
    {
        closeButton.interactable = !App.system.tutorial.isTutorial;
        infoButton.interactable = !App.system.tutorial.isTutorial;
        screenshotButton.interactable = !App.system.tutorial.isTutorial;
        for (int i = 0; i < tabButtons.Length; i++)
            tabButtons[i].interactable = !App.system.tutorial.isTutorial;
        
        isOpen = true;
        
        App.system.bgm.FadeIn().Play("Cultive");
        App.view.cultive.Open();
        
        CloseDropSensor();
        LocalLoadCultiveLitter();
        
        DOVirtual.DelayedCall(0.3f, () => SelectType(1));
    }

    public void Close()
    {
        if (!isCanDrag) 
            return;
        if (isDragging) 
            return;
        
        App.system.soundEffect.Play("Button");

        LocalSaveCultiveLitter();
        isOpen = false;

        int index = App.model.cultive.OpenFromIndex; //0:Feed 1:Follow

        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.cultive.Close();

            if (App.system.tutorial.isTutorial)
                return;
            
            if (index == 0)
                App.controller.feed.Open();
            if (index == 1)
                App.controller.followCat.Select(App.model.followCat.SelectedCat);
        });
    }

    #endregion

    public void SelectType(int index)
    {
        if (!isCanDrag)
            return;
        if (isDragging)
            return;

        App.system.soundEffect.Play("Button");
        
        App.model.cultive.SelectedType = index;
        ItemType targetType = ItemType.Feed;
        
        switch (index)
        {
            case 2:
                targetType = ItemType.Play;
                break;
            case 3:
                targetType = ItemType.Litter;
                break;
        }

        var items = App.factory.itemFactory.GetHoldItems(targetType);
        List<Item> result = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            result.Add(item);
        }

        App.model.cultive.SelectedItems = result;
    }

    // 傳遞正在拖曳的Item
    public void DragItem(Item item)
    {
        App.model.cultive.DragItem = item;
    }

    // Drag到貓sensor上 + ItemType是Food的話 = 飼養
    public void Feed()
    {
        isCanDrag = false;
        int randomIndex = Random.Range(1, 3);
        bool isChildCat = CatExtension.GetCatAgeLevel(App.model.cultive.SelectedCat.cloudCatData.CatData.SurviveDays) == 0;
        var dragItem = App.model.cultive.DragItem;

        if (isChildCat)
            randomIndex = 2;
        
        if (dragItem.itemFeedType == ItemFeedType.Food)
            catSkeleton.AnimationState.SetAnimation(0, $"AI_Main/Eat_0{randomIndex}_Feed", false);
        
        if (dragItem.itemFeedType == ItemFeedType.Snack)
            catSkeleton.AnimationState.SetAnimation(0, $"AI_Main/Drink_0{randomIndex}_Meat Puree", false);

        if (dragItem.itemFeedType == ItemFeedType.Water)
            catSkeleton.AnimationState.SetAnimation(0, $"AI_Main/Drink_0{randomIndex}_Water", false);

        if (dragItem.itemFeedType == ItemFeedType.Can)
            catSkeleton.AnimationState.SetAnimation(0, $"AI_Main/Eat_0{randomIndex}_Meat", false);

        catSkeleton.AnimationState.AddAnimation(0, "AI_Main/IDLE_Ordinary01", true, 0);
        
        // Eat Drink 完才Set資料
        catSkeleton.AnimationState.Start += SetFeedData;
        catSkeleton.AnimationState.Start += SetEndSensor;
    }

    private void SetFeedData(TrackEntry entry)
    {
        if (!entry.Animation.Name.Equals("AI_Main/IDLE_Ordinary01"))
            return;

        var item = App.model.cultive.DragItem;
        var cat = App.model.cultive.SelectedCat;

        int satietyValue;
        int moistureValue;
        int funValue;

        // 喜歡不喜歡的值
        if (item.itemFeedType == ItemFeedType.Food)
        {
            int likeIndex = cat.cloudCatData.CatSurviveData.LikeFoodIndex;
            int hateIndex = cat.cloudCatData.CatSurviveData.HateFoodIndex;

            if (item.foodType == (FoodType)likeIndex || item.foodType == FoodType.Ultimate)
            {
                satietyValue = item.likeSatiety;
                moistureValue = item.likeMoisture;
                funValue = item.likeFun;
            }
            else if (item.foodType == (FoodType)hateIndex)
            {
                satietyValue = item.hateSatiety;
                moistureValue = item.hateMoisture;
                funValue = item.hateFun;
            }
            else
            {
                satietyValue = item.normalSatiety;
                moistureValue = item.normalMoisture;
                funValue = item.normalFun;
            }
        }
        else if (item.itemFeedType == ItemFeedType.Water)
        {
            if (cat.cloudCatData.CatSurviveData.IsLikeDrink)
            {
                satietyValue = item.likeSatiety;
                moistureValue = item.likeMoisture;
                funValue = item.likeFun;
            }
            else
            {
                satietyValue = item.hateSatiety;
                moistureValue = item.hateMoisture;
                funValue = item.hateFun;
            }
        }
        else // 零食和罐頭是定值
        {
            satietyValue = item.likeSatiety;
            moistureValue = item.likeMoisture;
            funValue = item.likeFun;
        }

        float lastSatiety = cat.cloudCatData.CatSurviveData.Satiety;
        float lastMoisture = cat.cloudCatData.CatSurviveData.Moisture;
        float lastFavourbility = cat.cloudCatData.CatSurviveData.Favourbility;

        cat.cloudCatData.CatSurviveData.Satiety =
            Mathf.Clamp(cat.cloudCatData.CatSurviveData.Satiety + satietyValue, 0, 100);
        cat.cloudCatData.CatSurviveData.Moisture =
            Mathf.Clamp(cat.cloudCatData.CatSurviveData.Moisture + moistureValue, 0, 100);
        cat.cloudCatData.CatSurviveData.Favourbility =
            Mathf.Clamp(cat.cloudCatData.CatSurviveData.Favourbility + funValue, 0, 100);

        float newSatiety = cat.cloudCatData.CatSurviveData.Satiety;
        float newMoisture = cat.cloudCatData.CatSurviveData.Moisture;
        float newFavourbility = cat.cloudCatData.CatSurviveData.Favourbility;

        // OnAddFun?.Invoke(newFavourbility - lastFavourbility);
        OnAddFun?.Invoke(funValue);
        // OnAddSatiety?.Invoke(newSatiety - lastSatiety);
        OnAddSatiety?.Invoke(satietyValue);
        // OnAddMoisture?.Invoke(newMoisture - lastMoisture);
        OnAddMoisture?.Invoke(moistureValue);
        
        if (lastSatiety < 90f && newSatiety >= 90f)
            cat.cloudCatData.CatDiaryData.DiarySatietyScore++;

        if (lastMoisture < 90f && newMoisture >= 90f)
            cat.cloudCatData.CatDiaryData.DiaryMoistureScore++;

        if (lastFavourbility < 90f && newFavourbility >= 90f)
            cat.cloudCatData.CatDiaryData.DiaryFavourbilityScore++;

        if (!App.system.tutorial.isTutorial) item.Count--;

        App.system.cloudSave.UpdateCloudItemData();
        App.system.cloudSave.UpdateCloudCatSurviveData(cat.cloudCatData);
        App.system.cloudSave.UpdateCloudCatDiaryData(cat.cloudCatData);

        SelectType(App.model.cultive.SelectedType);

        if (satietyValue > 0)
            satietyEffect.Play();
        if (moistureValue > 0)
            moistureEffect.Play();
        if (funValue > 0)
            funEffects.Play();
        
        satietyPop.Pop(satietyValue);
        moisturePop.Pop(moistureValue);
        funPop.Pop(funValue);

        //DOVirtual.DelayedCall(1.5f, () => App.model.cultive.SelectedCat = cat);

        catSkeleton.AnimationState.Start -= SetFeedData;
    }

    // Drag到貓sensor + ItemType是Tool的話 = 事件
    public void Play()
    {
        isCanDrag = false;

        // 播放動畫
        var item = App.model.cultive.DragItem;

        catSkeleton.AnimationState.Start += SetPlayData;
        catSkeleton.AnimationState.Start += SetEndSensor;

        if (item.id == "ICP00001")
        {
            catSkeleton.AnimationState.SetAnimation(0, "Raising_Cat/IdleToSee", false);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/See", false, 0);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/SeeToIdle", false, 0);
        }

        if (item.id == "ICP00002")
        {
            catSkeleton.AnimationState.SetAnimation(0, "Raising_Cat/IdleToSing", false);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/Sing", false, 0);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/SingToIdle", false, 0);
        }

        if (item.id == "ICP00003")
        {
            catSkeleton.AnimationState.SetAnimation(0, "Raising_Cat/IdleToSleeping", false);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/Sleep", false, 0);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/SleepingToIdle", false, 0);
        }

        if (item.id == "ICP00004")
        {
            catSkeleton.AnimationState.SetAnimation(0, "Raising_Cat/IdleToSpeak", false);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/Speak", false, 0);
            catSkeleton.AnimationState.AddAnimation(0, "Raising_Cat/SpeakToIdle", false, 0);
        }

        catSkeleton.AnimationState.AddAnimation(0, "AI_Main/IDLE_Ordinary01", true, 0);
    }

    private void SetPlayData(TrackEntry entry)
    {
        if (!entry.Animation.Name.Contains("ToIdle"))
            return;

        catSkeleton.AnimationState.Start -= SetPlayData;

        var cat = App.model.cultive.SelectedCat;
        float lastFavourbility = cat.cloudCatData.CatSurviveData.Favourbility;

        cat.cloudCatData.CatSurviveData.Favourbility =
            Mathf.Clamp(cat.cloudCatData.CatSurviveData.Favourbility + 20, 0, 100);

        float newFavourbility = cat.cloudCatData.CatSurviveData.Favourbility;

        if (lastFavourbility < 96f && newFavourbility >= 96f)
        {
            cat.cloudCatData.CatDiaryData.DiaryFavourbilityScore++;
        }

        App.system.cloudSave.UpdateCloudCatDiaryData(cat.cloudCatData);
        App.system.cloudSave.UpdateCloudCatSurviveData(cat.cloudCatData);

        funEffects.Play();
        funPop.Pop(20);
    }

    private void SetEndSensor(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("AI_Main/IDLE_Ordinary01"))
        {
            entry.Start -= SetEndSensor;
            
            #region Callback

            var item = App.model.cultive.DragItem;

            if (item.itemFeedType == ItemFeedType.Food)
                OnFeedFood?.Invoke();
            if (item.itemFeedType == ItemFeedType.Water)
                OnFeedWater?.Invoke();
            if (item.itemType == ItemType.Play)
                OnPlayCat?.Invoke(App.model.cultive.DragItem.id);

            #endregion

            DOVirtual.DelayedCall(entry.Animation.Duration, () =>
            {
                App.model.cultive.SelectedCat = App.model.cultive.SelectedCat;
                isCanDrag = true;
                OpenClickCat();
            });
        }
    }

    /// 更換貓砂
    public void ChangeLitter()
    {
        if (App.model.cultive.UsingLitterIndex >= 0)
            if (App.model.cultive.NextCleanDateTime > App.system.myTime.MyTimeNow) //時間還沒到
                return;

        if (App.model.cultive.CleanLitterCount > 0) return;

        var item = App.model.cultive.DragItem;

        App.model.cultive.UsingLitterIndex = (int)item.itemLitterType;

        //停止計算
        CancelInvoke(nameof(CountDownTimer));
        //重新給時間
        ResetTimer();
        //開始計算
        InvokeRepeating(nameof(CountDownTimer), 0.1f, 0.1f);

        if (!App.system.tutorial.isTutorial) item.Count--;

        var items = App.model.cultive.SelectedItems;

        for (int i = 0; i < items.Count; i++)
            if (items[i].Count <= 0) items.RemoveAt(i);

        App.model.cultive.SelectedItems = items;

        //Save
        var cloudCatData = App.model.cultive.SelectedCat.cloudCatData;
        cloudCatData.CatDiaryData.DiaryLitterScore++;
        cloudCatData.CatSurviveData.Favourbility =
            Mathf.Clamp(cloudCatData.CatSurviveData.Favourbility + 40, 0, 100);
        
        App.system.cloudSave.UpdateCloudItemData();
        App.system.cloudSave.UpdateCloudCatDiaryData(cloudCatData);
        App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);
        
        funEffects.Play();
        funPop.Pop(40);

        bool isAdult = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0;
        string animName = isAdult ? "Rearing_Cat/Rearing_Smile_IDLE" : "Rearing_Cat/Rearing_Smile_Sit";
        
        var t = catSkeleton.AnimationState.SetAnimation(0, animName, false);
        DOVirtual.DelayedCall(t.Animation.Duration, () => App.model.cultive.SelectedCat = App.model.cultive.SelectedCat);
        
        OnChangeLitter?.Invoke();
    }

    public void CleanLitter()
    {
        if (App.model.cultive.CleanLitterCount <= 0) return;

        App.system.soundEffect.Play("Button");
        
        App.model.cultive.CleanLitterCount--;

        if (Random.value < 0.05f)
        {
            //Diamond
            App.system.player.Diamond += 5;
            diamondPop.transform.SetAsLastSibling();
            diamondPop.Pop(5);
            return;
        }

        if (Random.value < 0.65f)
        {
            //Coin
            App.system.player.Coin += 100;
            moneyPop.transform.SetAsLastSibling();
            moneyPop.Pop(100);
            return;
        }

        //Shit
        shitPop.transform.SetAsLastSibling();
        shitPop.Pop(1);
    }

    public void NoLitterPopUp()
    {
        App.view.cultive.noLitterObject.transform.DOScale(new Vector2(1.1f, 1.1f), 0.15f).From(Vector2.one)
            .SetLoops(4, LoopType.Yoyo);
    }

    public void NoLitterCatTalk()
    {
        App.view.cultive.catDialogObject.transform.DOScale(new Vector2(1.1f, 1.1f), 0.15f).From(Vector2.one)
            .SetLoops(4, LoopType.Yoyo);

        App.view.cultive.catDialogObject.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack).SetDelay(1.2f);
    }

    #region Click

    public void ClickCat()
    {
        if (!isCanDrag)
        {
            var track = catSkeleton.AnimationState.GetCurrent(0);
            track.TrackTime = 25;
            return;
        }

        App.system.soundEffect.Play("Button");
        
        isCanDrag = false;

        CloudCatData cloudCatData = App.model.cultive.SelectedCat.cloudCatData;
        catSkeleton.AnimationState.SetAnimation(0, MyAnimationTable.GetClickAnimationName(cloudCatData), false);
        catSkeleton.AnimationState.AddAnimation(0, "AI_Main/IDLE_Ordinary01", true, 0);

        catSkeleton.AnimationState.Start += SetClickCatEnd;
    }

    private void SetClickCatEnd(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("AI_Main/IDLE_Ordinary01"))
        {
            isCanDrag = true;
            entry.Start -= SetClickCatEnd;
        }
    }

    public void OpenClickCat()
    {
        catClickObject.SetActive(true);
    }

    public void CloseClickCat()
    {
        catClickObject.SetActive(false);
    }

    #endregion

    #region LitterTimer

    private void CountDownTimer()
    {
        DateTime dt = App.model.cultive.NextCleanDateTime;
        if (dt <= App.system.myTime.MyTimeNow)
        {
            App.model.cultive.NextCleanDateTime = dt;
            CancelInvoke(nameof(CountDownTimer));
            return;
        }

        App.model.cultive.NextCleanDateTime = dt;
    }

    private void ResetTimer()
    {
        App.model.cultive.NextCleanDateTime = App.system.myTime.MyTimeNow.AddHours(6).AddMinutes(0).AddSeconds(0);

        int usingIndex = App.model.cultive.UsingLitterIndex;
        int likeIndex = App.model.cultive.SelectedCat.cloudCatData.CatSurviveData.LikeLitterIndex;
        int hateIndex = App.model.cultive.SelectedCat.cloudCatData.CatSurviveData.HateLitterindex;
        
        if (usingIndex == likeIndex)
            App.model.cultive.CleanLitterCount = 7;
        else if (usingIndex == hateIndex)
            App.model.cultive.CleanLitterCount = 3;
        else
            App.model.cultive.CleanLitterCount = 5;
    }

    #endregion

    #region Sensor

    public void OpenDropSensor()
    {
        catDropSensor.SetActive(true);
        littleDropSensor.SetActive(true);
    }

    public void CloseDropSensor()
    {
        catDropSensor.SetActive(false);
        littleDropSensor.SetActive(false);
    }

    #endregion

    #region LocalSave

    public void LocalSaveCultiveLitter()
    {
        if (!isOpen) return;

        CancelInvoke(nameof(CountDownTimer));

        if (App.model.cultive.UsingLitterIndex == -1) return;

        //Key
        Cat cat = App.model.cultive.SelectedCat;
        string key = "Cultive_" + cat.cloudCatData.CatData.CatId;

        //Value
        int litterIndex = App.model.cultive.UsingLitterIndex;
        var ticks = App.model.cultive.NextCleanDateTime.Ticks;
        int cleanCount = App.model.cultive.CleanLitterCount;

        string value = $"{litterIndex}:{ticks}:{cleanCount}";
        PlayerPrefs.SetString(key, value);
    }

    private void LocalLoadCultiveLitter()
    {
        Cat cat = App.model.cultive.SelectedCat;
        string key = "Cultive_" + cat.cloudCatData.CatData.CatId;

        if (!PlayerPrefs.HasKey(key))
        {
            App.model.cultive.UsingLitterIndex = -1;
            App.model.cultive.CleanLitterCount = 0;
            App.model.cultive.NextCleanDateTime = new Timestamp().ToDateTime().ToLocalTime();
            return;
        }

        string value = PlayerPrefs.GetString(key);
        var strings = value.Split(':');
        int litterIndex = Int32.Parse(strings[0]);
        var ticks = Convert.ToInt64(strings[1]);
        var date = new DateTime().AddTicks(ticks);
        int cleanCount = Int32.Parse(strings[2]);

        App.model.cultive.UsingLitterIndex = litterIndex;
        App.model.cultive.NextCleanDateTime = date;
        App.model.cultive.CleanLitterCount = cleanCount;

        if (date > App.system.myTime.MyTimeNow)
            InvokeRepeating(nameof(CountDownTimer), 0.1f, 0.1f);
    }

    #endregion

    #region CatReject
    
    public void Reject()
    {
        isCanDrag = false;
        
        var cat = App.model.cultive.SelectedCat;
        if (CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays) != 0)
        {
            var catSkin = App.view.cultive.catSkin;
            catSkin.SetCold();
        }

        catSkeleton.AnimationState.SetAnimation(0, "Rearing_Cat/Rearing_Reject", false).Complete += WaitReject;
    }

    private void WaitReject(TrackEntry entry)
    {
        isCanDrag = true;
        catSkeleton.AnimationState.Complete -= WaitReject;
        
        var catSkin = App.view.cultive.catSkin;
        catSkin.ChangeSkin(App.model.cultive.SelectedCat.cloudCatData);

        catSkeleton.AnimationState.AddAnimation(0, "AI_Main/IDLE_Ordinary01", true, 0);
    }

    #endregion

    #region Screenshot

    public void OpenScreenshot()
    {
        if (!isCanDrag)
            return;
        if (isDragging)
            return;
        
        App.system.soundEffect.Play("Button");
        
        App.system.screenshot.OnScreenshotComplete += CloseScreenshot;
        App.system.screenshot.OnClose += CloseScreenshot;
        App.view.cultive.TweenOut();

        DOVirtual.DelayedCall(1f, App.system.screenshot.Open);
    }

    private void CloseScreenshot()
    {
        App.system.soundEffect.Play("Button");
        
        App.view.cultive.TweenIn();
        App.system.screenshot.OnScreenshotComplete -= CloseScreenshot;
        App.system.screenshot.OnClose -= CloseScreenshot;
    }

    #endregion

    #region SubInfo

    public void OpenCultiveInfo()
    {
        if (!isCanDrag)
            return;
        if (isDragging)
            return;
        
        App.system.soundEffect.Play("Button");
        App.view.cultive.cultiveInfo.Open();
        SelectTab(0);
    }

    public void CloseCultiveInfo()
    {
        App.system.soundEffect.Play("Button");
        App.view.cultive.cultiveInfo.Close();
        CancelPreviewSkin();
    }

    public void OpenCultiveChooseSkin()
    {
        if (!isCanDrag)
            return;
        if (isDragging)
            return;
        
        App.system.soundEffect.Play("Button");
        App.view.cultive.cultiveInfo.Open();
        SelectTab(1);
    }

    public void SelectTab(int index)
    {
        App.system.soundEffect.Play("Button");
        App.model.cultive.SelectedTab = index;
        
        if (index == 0)
        {
            OpenStatus();
            return;
        }

        if (index == 1)
        {
            OpenChooseSkin();
        }
    }

    private void OpenStatus()
    {
        App.system.soundEffect.Play("Button");
        App.view.cultive.cultiveInfo.status.Open();
        CloseChooseSkin();
    }

    private void CloseStatus()
    {
        App.view.cultive.cultiveInfo.status.Close();
    }

    private void OpenChooseSkin()
    {
        App.system.soundEffect.Play("Button");
        App.model.cultive.SkinItems = App.factory.itemFactory.GetItemByType((int)ItemType.CatSkin);
        App.view.cultive.cultiveInfo.chooseSkin.Open();
        CloseStatus();
    }

    private void CloseChooseSkin()
    {
        CancelPreviewSkin();
        App.view.cultive.cultiveInfo.chooseSkin.Close();
    }

    public void CopyCatId()
    {
        App.model.cultive.SelectedCat.cloudCatData.CatData.CatId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
    }

    public void RenameCat()
    {
        App.system.catRename.Active(App.model.cultive.SelectedCat.cloudCatData, () =>
        {
            App.model.cultive.SelectedCat = App.model.cultive.SelectedCat;
        });
    }

    public void ChooseSkin(int index)
    {
        App.system.soundEffect.Play("Button");
        App.model.cultive.SelectedSkinIndex = index;
        
        if (!isRecordSkin)
        {
            var cat = App.model.cultive.SelectedCat;
            skinBeforePreview = cat.cloudCatData.CatSkinData.UseSkinId;
            isRecordSkin = true;
        }

        PreviewSkin();
    }

    private void PreviewSkin()
    {
        int index = App.model.cultive.SelectedSkinIndex;
        var cat = App.model.cultive.SelectedCat;

        if (index >= 0)
        {
            Item skinItem = App.model.cultive.SkinItems[index];
            cat.cloudCatData.CatSkinData.UseSkinId = skinItem.id;
        }
        else
            cat.cloudCatData.CatSkinData.UseSkinId = String.Empty;
        
        App.model.cultive.SelectedCat = cat;
    }

    private void CancelPreviewSkin()
    {
        if (!isRecordSkin)
            return;
        
        var cat = App.model.information.SelectedCat;
        
        if (skinBeforePreview.IsNullOrEmpty() && cat.cloudCatData.CatSkinData.UseSkinId.IsNullOrEmpty())
            return;

        cat.cloudCatData.CatSkinData.UseSkinId = skinBeforePreview;
        App.model.cultive.SelectedCat = cat;
        skinBeforePreview = String.Empty;
    }

    public void ConfirmChooseSkin()
    {
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        int index = App.model.cultive.SelectedSkinIndex;
        var cat = App.model.cultive.SelectedCat;
        App.system.cloudSave.UpdateCloudCatSkinData(cat.cloudCatData);
        
        isRecordSkin = false;

        SpineSetSkinHappy();

        if (!skinBeforePreview.IsNullOrEmpty())
        {
            Item lastSkinItem = App.factory.itemFactory.GetItem(skinBeforePreview);
            lastSkinItem.Count++;
            skinBeforePreview = String.Empty;
        }
        
        if (index == -1)
        {
            // 脫
            App.system.cloudSave.UpdateCloudItemData();
            OpenChooseSkin();
            App.system.cat.RefreshCatSkin();
            return;
        }

        Item skinItem = App.model.cultive.SkinItems[index];
        skinItem.Count--;
        App.system.cloudSave.UpdateCloudItemData();
        OpenChooseSkin();
        App.system.cat.RefreshCatSkin();
    }

    private void SpineSetSkinHappy()
    {
        TrackEntry t = App.view.cultive.cultiveInfo.catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Rearing_Cat/Rearing_Look_IDLE", false);
        t.Complete += WaitSpineSetSkinHappy;
    }

    private void WaitSpineSetSkinHappy(TrackEntry trackEntry)
    {
        trackEntry.Complete -= WaitSpineSetSkinHappy;
        App.view.cultive.cultiveInfo.catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
    }

    #endregion
    
    //TODO Debug
    public void DebugSick()
    {
        App.model.cultive.SelectedCat.SetSick();
        App.view.cultive.catSkin.ChangeSkin(App.model.cultive.SelectedCat.cloudCatData);
        App.system.cloudSave.UpdateCloudCatHealthData(App.model.cultive.SelectedCat.cloudCatData);
    }

    public void DebugDead()
    {
        App.model.cultive.SelectedCat.cloudCatData.CatServerData.IsDead = true;
        App.view.cultive.catSkin.ChangeSkin(App.model.cultive.SelectedCat.cloudCatData);
        App.system.cloudSave.UpdateCloudCatServerData(App.model.cultive.SelectedCat.cloudCatData);
    }

    public void DebugBug()
    {
        App.model.cultive.SelectedCat.cloudCatData.CatHealthData.IsBug = true;
        App.view.cultive.catSkin.ChangeSkin(App.model.cultive.SelectedCat.cloudCatData);
        App.system.cloudSave.UpdateCloudCatHealthData(App.model.cultive.SelectedCat.cloudCatData);
        
    }
}