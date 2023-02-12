using System;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.Common.Extensions;
using Firebase.Firestore;
using PolyNav;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cat : MvcBehaviour
{
    #region Variable

    [SerializeField] private CatSkin catSkin;
    public Transform littleGameBubble;
    public Transform bigGameBubble;
    public Transform hand;

    public CloudCatData cloudCatData;
    public bool isFriendMode = false;

    public string dontLikePlayId;
    
    //GameType
    [HideInInspector] public int littleGameIndex;
    [HideInInspector] public BigGameBehaviour bigGameBehaviour;
    [HideInInspector] public string catNotifyId; //每次刷遊戲覆蓋
    [HideInInspector] public bool isPauseGame;

    [Title("Effects")]
    public ParticleSystem catHeartEffect;

    [Title("UI")]
    public CatCanvas catCanvas;

    // ID變化

    private PolyNavAgent polyNavAgent;
    private DirectionChecker directionChecker;
    private Animator anim; // 之後統一換這個

    private int CancelGameTimer = 0;
    private int DrawGameTimer = 0;

    #endregion

    public Room specialSpineRoom;

    public void Active()
    {
        polyNavAgent = GetComponent<PolyNavAgent>();
        directionChecker = GetComponent<DirectionChecker>();

        anim = GetComponent<Animator>();

        if (isFriendMode)
            return;

        if (catHeartEffect.isPlaying)
            catHeartEffect.Stop();

        DrawDontLikeId();
        
        //隨機開始時間
        int randTimer = Random.Range(10, 15);
        DrawGameTimer = randTimer;
        InvokeRepeating(nameof(CountTimerDrawGame), 1, 1);
    }

    #region Init

    public void SetCloudCatData(CloudCatData cloudCatData)
    {
        this.cloudCatData = cloudCatData;
        catSkin.ChangeSkin(cloudCatData);
    }

    #endregion

    public void Reset()
    {
        PolyNavAgent agent = GetComponent<PolyNavAgent>();

        for (int i = 0; i < anim.parameters.Length; i++)
        {
            if (anim.parameters[i].type == AnimatorControllerParameterType.Bool)
                anim.SetBool(anim.parameters[i].name, false);

            if (anim.parameters[i].type == AnimatorControllerParameterType.Int)
                anim.SetInteger(anim.parameters[i].name, 0);
        }

        anim.Play("Idle_Selector");
        agent.Stop();
    }

    #region Move

    public void MoveToRandomRoom()
    {
        Room room = null;

        if (!isFriendMode)
        {
            if (App.system.room.GetRoomCount() == 1)
            {
                RandomMoveAtRoom();
                return;
            }

            room = App.system.room.GetRandomRoom();
        }
        else
        {
            var friendRoomSystem = FindObjectOfType<FriendRoom_RoomSystem>();

            if (friendRoomSystem.GetRoomCount() == 1)
            {
                RandomMoveAtRoom();
                return;
            }

            room = friendRoomSystem.GetRandomRoom();
        }

        for (int i = 0; i < 50; i++)
        {
            Vector2 target = room.transform.position;

            target.x += Random.Range(1f, 5f) * Random.Range(0, 2);
            target.y += Random.Range(1f, 5f) * Random.Range(0, 2);

            polyNavAgent.SetDestination(target);
            if (polyNavAgent.hasPath)
                break;
        }
    }

    public void MoveToSpecialSpineRoom()
    {
        // print("開始");
        Room room = null;

        if (!isFriendMode)
        {
            room = App.system.room.GetRandomSpecialSpineRoom();
            if (room != null)
                print(room.roomData.id);
            else
                print("找不到");
        }
        else
        {
            // var friendRoomSystem = FindObjectOfType<FriendRoom_RoomSystem>();
            //
            // if (friendRoomSystem.GetRoomCount() == 1)
            // {
            //     RandomMoveAtRoom();
            //     return;
            // }
            //
            // room = friendRoomSystem.GetRandomRoom();
        }

        if (room == null)
        {
            RandomMoveAtRoom();
            return;
        }

        Vector2 target = room.transform.position;

        target.x += Random.Range(1f, 5f) * Random.Range(0, 2);
        target.y += Random.Range(1f, 5f) * Random.Range(0, 2);

        polyNavAgent.SetDestination(target);
        
        if (polyNavAgent.hasPath)
        {
            specialSpineRoom = room;
            specialSpineRoom.spcialSpineIsUse = true;
            InvokeRepeating("WaitMoveEnd", 0, 0.1f);
        }
        else
        {
            RandomMoveAtRoom();
        }
    }

    private void WaitMoveEnd()
    {
        // print("正在走");

        if (directionChecker.CheckIsMoving())
            return;

        // print("走到");

        if (specialSpineRoom == null) // 房間爆掉
        {
            specialSpineRoom.spcialSpineIsUse = false;
            CancelInvoke("WaitMoveEnd");
            return;
        }
        
        transform.position = specialSpineRoom.spcialSpinePosition.position;
        
        directionChecker.TurnLeft();
        directionChecker.Stop();
        
        int roomIndex = Convert.ToInt32(specialSpineRoom.roomData.id.Split("IRM")[1]);
        anim.SetInteger(CatAnimTable.SpcialSpineRoomId.ToString(), roomIndex);
        anim.Play("SpecialSpine");
        
        specialSpineRoom.PlaySpecialSpine();
        CancelInvoke("WaitMoveEnd");
    }
    
    public void RandomMoveAtRoom()
    {
        FriendRoom_GridSystem friendGridSystem = null;

        if (isFriendMode)
            friendGridSystem = FindObjectOfType<FriendRoom_GridSystem>();

        for (int i = 0; i < 50; i++)
        {
            Vector2 target = transform.position;
            int x = (int)(target.x / 5.12f);
            int y = (int)(target.y / 5.12f);

            Room room = null;

            if (!isFriendMode)
                room = App.system.grid.GetGrid(x, y).Content.GetComponent<Room>();
            else
                room = friendGridSystem.GetGrid(x, y).Content.GetComponent<Room>();

            if (room == null)
                continue;

            target.x = (x + Random.Range(0, room.Width)) * 5.12f + Random.Range(1.024f, 4.096f);
            target.y = (y + Random.Range(0, room.Height)) * 5.12f + Random.Range(1.024f, 4.096f);

            polyNavAgent.SetDestination(target);
            if (polyNavAgent.hasPath)
                break;
        }
    }

    public void StopMove()
    {
        polyNavAgent.Stop();
    }

    #endregion

    public void FollowCat()
    {
        if (isFriendMode)
        {
            FindObjectOfType<FriendRoom_FollowCat>().Select(this);
            return;
        }

        App.controller.followCat.Select(this);
        App.system.catNotify.Remove(this);
    }

    #region DrawGame

    private void StartCountDrawGame()
    {
        DrawGameTimer = 0;
        InvokeRepeating(nameof(CountTimerDrawGame), 1, 1);
    }

    private void CountTimerDrawGame()
    {
        if (isPauseGame)
            return;
        
        if (DrawGameTimer < 30)
        {
            DrawGameTimer++;
            return;
        }

        DrawGame();
    }

    [Button]
    public void DrawGame()
    {
        if (isPauseGame)
        {
            Debug.LogWarning("生病不能玩");
            return;
        }
        
        CancelInvoke(nameof(CountTimerDrawGame));
        if (App.system.bigGames.GetBigGames().Count > 0)
        {
            if (Random.value > 0.7f)
            {
                OpenBigGame();
                StartCountCancelGame();
                return;
            }
        }

        App.system.littleGame.SetLittleGame(this);
        OpenLittleGame();
        StartCountCancelGame();
    }

    #endregion

    #region CancelGame

    /// 開始計時關遊戲
    private void StartCountCancelGame()
    {
        CancelGameTimer = 0;
        InvokeRepeating(nameof(CountTimerCancelGame), 1, 1);
    }

    private void CountTimerCancelGame()
    {
        if (isPauseGame)
            return;
        
        if (CancelGameTimer < 60)
        {
            CancelGameTimer++;
            return;
        }

        CancelGame();
    }

    /// 關掉氣泡，準備下一個遊戲
    [Button]
    public void CancelGame()
    {
        if (isPauseGame)
            return;
        
        CancelInvoke(nameof(CountTimerCancelGame));
        CloseBigGame();
        CloseLittleGame();
        StartCountDrawGame();
    }

    #endregion

    #region Games

    public void OpenLittleGame()
    {
        CloseBigGame();

        App.system.littleGame.SetLittleGame(this);
        littleGameBubble.gameObject.SetActive(true);

        littleGameBubble.DOScale(Vector3.one, 0.5f).From(Vector3.zero);
        littleGameBubble.DOLocalMoveY(3, 0.5f).From(1);

        App.system.catNotify.Add(this);
        App.system.soundEffect.Play("ED00002");
    }

    public void CloseLittleGame()
    {
        littleGameBubble.DOScale(Vector3.zero, 0.5f);
        littleGameBubble.DOLocalMoveY(1, 0.5f).OnComplete(() => { littleGameBubble.gameObject.SetActive(false); });

        App.system.catNotify.Remove(this);
    }

    public void OpenBigGame()
    {
        CloseLittleGame();

        bigGameBehaviour = App.system.bigGames.GetRandomGame();
        catNotifyId = bigGameBehaviour.notifyId;

        bigGameBubble.gameObject.SetActive(true);

        bigGameBubble.DOScale(Vector3.one, 0.5f).From(Vector3.zero);
        bigGameBubble.DOLocalMoveY(3, 0.5f).From(1);

        //TODO Move

        App.system.catNotify.Add(this);
        App.system.soundEffect.Play("ED00001");
    }

    private void CloseBigGame()
    {
        bigGameBubble.DOScale(Vector3.zero, 0.5f);
        bigGameBubble.DOLocalMoveY(1, 0.5f).OnComplete(() => { bigGameBubble.gameObject.SetActive(false); });

        App.system.catNotify.Remove(this);
    }

    public void StartLittleGame()
    {
        if (isFriendMode)
            return;

        if (App.model.build.IsCanMoveOrRemove)
            return;
        
        CancelGame();

        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0)
        {
            catSkin.SetUsingSkinNull();
        }
        
        App.system.littleGame.Active(this);
        App.system.catNotify.Remove(this);

        catNotifyId = string.Empty;
    }

    public void StartBigGame()
    {
        if (isFriendMode)
            return;

        if (App.model.build.IsCanMoveOrRemove)
            return;

        CancelGame();

        App.controller.followCat.CloseByOpenLobby();
        App.controller.lobby.Open();
        App.system.bigGames.OpenGame(bigGameBehaviour);

        App.system.catNotify.Remove(this);
        catNotifyId = string.Empty;
    }

    #endregion

    #region Status

    private void SetMoisture()
    {
        // 顯 水份數值
        float value = App.factory.catFactory.catDataSetting.basicMoisture;
        cloudCatData.CatSurviveData.Moisture = Mathf.Clamp(cloudCatData.CatSurviveData.Moisture - value, 0, 100);

        // 深 水份數值
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        var realValue = CatExtension.GetCatRealValue(ageLevel, cloudCatData.CatSurviveData.Moisture);
        cloudCatData.CatSurviveData.ChangeRealMoisture(realValue);
        
        App.system.cat.RefreshRedPoint();
    }

    private void SetSatiety()
    {
        // 顯 飽足數值
        float value = App.factory.catFactory.catDataSetting.SatietyByLevel(CatExtension.CatEatLevel(cloudCatData));
        cloudCatData.CatSurviveData.Satiety = Mathf.Clamp(cloudCatData.CatSurviveData.Satiety - value, 0, 100);

        // 深 飽足數值
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        var realValue = CatExtension.GetCatRealValue(ageLevel, cloudCatData.CatSurviveData.Satiety);
        cloudCatData.CatSurviveData.ChangeRealSatiety(realValue);
        
        App.system.cat.RefreshRedPoint();
    }

    private void SetFavorability()
    {
        // 顯 好感數值
        float value = App.factory.catFactory.catDataSetting.FunByLevel(CatExtension.CatFunLevel(cloudCatData));

        if (string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId) || !cloudCatData.CatHealthData.IsBug) //沒病 沒蟲
            cloudCatData.CatSurviveData.Favourbility =
                Mathf.Clamp(cloudCatData.CatSurviveData.Favourbility - value, 0, 100);
        else //有病
            cloudCatData.CatSurviveData.Favourbility =
                Mathf.Clamp(cloudCatData.CatSurviveData.Favourbility - value, 0, 60);

        // 深 好感數值
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        var realValue = CatExtension.GetCatRealValue(ageLevel, cloudCatData.CatSurviveData.Favourbility);
        cloudCatData.CatSurviveData.ChangeRealFavourbility(realValue);
        
        App.system.cat.RefreshRedPoint();
    }

    #endregion

    #region Check

    // 固定經過時間一次
    public void CheckStatus()
    {
        for (int i = 0; i < 3; i++)
        {
            SetMoisture();
            SetSatiety();
            SetFavorability();
        }
    }

    private bool CheckNaturalDead()
    {
        float percent = App.factory.catFactory.catDataSetting.NaturalDeadPercent(cloudCatData.CatData.SurviveDays);
        
        if (Random.value > percent)
            return false;
        
        cloudCatData.CatServerData.IsDead = true;
        App.system.cloudSave.UpdateCloudCatServerData(cloudCatData);
        return true;
    }

    private bool CheckSickDead()
    {
        int sickLevel = App.factory.sickFactory.GetSickLevel(cloudCatData.CatHealthData.SickId);
        int metCount = cloudCatData.CatHealthData.MetDoctorCount;
        float percent = 0;

        if (cloudCatData.CatHealthData.SickId is "SK001" or "SK002")
            percent = 0.9f;
        else
            App.factory.catFactory.catDataSetting.SickDeadPercent(sickLevel, metCount);
        
        if (Random.value > percent)
            return false;
        
        cloudCatData.CatServerData.IsDead = true;
        App.system.cloudSave.UpdateCloudCatServerData(cloudCatData);
        return true;
    }
    
    private void CheckBug() // 檢查長蟲
    {
        DateTime noBugExpiredDate = cloudCatData.CatHealthData.NoBugExpireTimestamp.ToDateTime().ToLocalTime();
        if (noBugExpiredDate > App.system.myTime.MyTimeNow)
            return;

        //檢查長蟲
        if (Random.value <= 0.15f)
        {
            cloudCatData.CatHealthData.IsBug = true;
            App.system.cloudSave.UpdateCloudCatHealthData(cloudCatData);
            ChangeSkin();
        }
    }

    public void Death()
    {
        App.model.entrance.OpenType = 1;
        App.system.catNotify.Remove(this);

        // 造型給我還回來
        if (!cloudCatData.CatSkinData.UseSkinId.IsNullOrEmpty())
            App.factory.itemFactory.GetItem(cloudCatData.CatSkinData.UseSkinId).Count += 1;

        cloudCatData.CatData.DeathTime = Timestamp.GetCurrentTimestamp();
        cloudCatData.CatDiaryData.DiaryDatas = App.factory.diaryFactory.GetDiaryDatas(cloudCatData);
    }

    #endregion

    #region Snack/Soup

    public void GetLikeSnack()
    {
        cloudCatData.CatSurviveData.LikeSnackIndex = Random.Range(0, 3);
        SaveLikeSnackIndex();
    }

    private void SaveLikeSnackIndex()
    {
        PlayerPrefs.SetInt($"{cloudCatData.CatData.CatId}_LikeSnackIndex", cloudCatData.CatSurviveData.LikeSnackIndex);
    }

    private void LoadLikeSnackIndex()
    {
        cloudCatData.CatSurviveData.LikeSnackIndex =
            PlayerPrefs.GetInt($"{cloudCatData.CatData.CatId}_LikeSnackIndex", -1);
        if (cloudCatData.CatSurviveData.LikeSnackIndex == -1)
            GetLikeSnack();
    }

    public void GetLikeSoup()
    {
        cloudCatData.CatSurviveData.LikeSoupIndex = Random.Range(1, 4);
        SaveLikeSoupIndex();
    }

    private void SaveLikeSoupIndex()
    {
        PlayerPrefs.SetInt($"{cloudCatData.CatData.CatId}_LikeSoupIndex", cloudCatData.CatSurviveData.LikeSoupIndex);
    }

    private void LoadLikeSoupIndex()
    {
        cloudCatData.CatSurviveData.LikeSoupIndex =
            PlayerPrefs.GetInt($"{cloudCatData.CatData.CatId}_LikeSoupIndex", -1);
        if (cloudCatData.CatSurviveData.LikeSoupIndex == -1)
            GetLikeSoup();
    }

    #endregion

    #region Debug
    
    [Button]
    private void DebugPrint()
    {
        print($"LikeSnack: {cloudCatData.CatSurviveData.LikeSnackIndex}");
        print($"LikeSoup:{cloudCatData.CatSurviveData.LikeSoupIndex}");
    }

    #endregion

    public void ChangeSkin()
    {
        catSkin.ChangeSkin(cloudCatData);

        if (isFriendMode)
            return;
        
        Card_CatNotify cardCatNotify = App.system.catNotify.GetNotify(this);
        if (cardCatNotify != null)
            cardCatNotify.SetData(this);
    }

    public void CloseFace()
    {
        catSkin.CloseFace();
    }

    public void CheckCatStatusPerDay()
    {
        if (cloudCatData.CatServerData.IsDead) // 該死了
            return;

        if (CheckNaturalDead()) // 要自然死了
            return;

        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId)) // 病了
        {
            if (CheckSickDead())
                return;
            
            CheckBug();
            return;
        }

        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        string sickId = string.Empty;

        if (ageLevel == 0) //Kitty
        {
            if (cloudCatData.CatSurviveData.RealSatiety <= 0 || cloudCatData.CatSurviveData.RealMoisture <= 0 || cloudCatData.CatSurviveData.RealFavourbility <= 0)
                UpdateSick("SK011");
        }
        else // Adult
        {
            if (cloudCatData.CatSurviveData.RealMoisture <= 0)
                sickId = App.factory.sickFactory.GetMoistureSick(cloudCatData);
            else if (cloudCatData.CatSurviveData.RealFavourbility <= 0)
            {
                if (Random.value > 0.5f)
                    App.system.catLosing.Active(this);
                else
                    sickId = App.factory.sickFactory.GetFavourbilitySick(cloudCatData);
            }
            else if (cloudCatData.CatSurviveData.RealSatiety <= 0)
                sickId = App.factory.sickFactory.GetSatietySick(cloudCatData);
        }

        if (!string.IsNullOrEmpty(sickId)) // 三項生病不跑後面
        {
            UpdateSick(sickId);
            CheckBug();
            return;
        }

        if (!cloudCatData.CatHealthData.IsVaccine)
            sickId = App.factory.sickFactory.GetVaccineSick();
        
        if (!string.IsNullOrEmpty(sickId)) // 疫苗生病不跑後面
        {
            UpdateSick(sickId);
            CheckBug();
            return;
        }
        
        if (!cloudCatData.CatHealthData.IsLigation)
            sickId = App.factory.sickFactory.GetLigationSick(cloudCatData);

        if (!string.IsNullOrEmpty(sickId)) // 結紮生病不跑後面
        {
            UpdateSick(sickId);
            CheckBug();
            return;
        }
        
        //沒病的話檢查跳蚤
        CheckBug();
    }

    public void CheckCatSickByStatus()
    {
        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId)) // 病了
            return;
        
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        string sickId = string.Empty;

        if (ageLevel == 0) //Kitty
        {
            if (cloudCatData.CatSurviveData.RealSatiety <= 0 || cloudCatData.CatSurviveData.RealMoisture <= 0 || cloudCatData.CatSurviveData.RealFavourbility <= 0)
                UpdateSick("SK011");
        }
        else // Adult
        {
            if (cloudCatData.CatSurviveData.RealMoisture <= 0)
                sickId = App.factory.sickFactory.GetMoistureSick(cloudCatData);
            else if (cloudCatData.CatSurviveData.RealFavourbility <= 0)
                sickId = App.factory.sickFactory.GetFavourbilitySick(cloudCatData);
            else if (cloudCatData.CatSurviveData.RealSatiety <= 0)
                sickId = App.factory.sickFactory.GetSatietySick(cloudCatData);
        }

        if (!string.IsNullOrEmpty(sickId))
            UpdateSick(sickId);
    }

    public void CheckCatStatusPerLogin()
    {
        int passMinutes = (int)(App.system.myTime.MyTimeNow - App.system.myTime.LastLoginDateTime).TotalMinutes;
        for (int i = 0; i < passMinutes; i++)
        {
            SetMoisture();
            SetSatiety();
            SetFavorability();
        }

        App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);

        // 載入今日最愛零食
        LoadLikeSnackIndex();
        LoadLikeSoupIndex();
    }

    private void DrawDontLikeId()
    {
        int index = 0;
        
        if (Random.value > .5f)
            index = Random.Range(1, 5);
        
        dontLikePlayId = "ICP" + index.ToString("00000");
    }

    private void UpdateSick(string sickId)
    {
        if (!string.IsNullOrEmpty(sickId) && !string.IsNullOrEmpty(cloudCatData.CatSkinData.UseSkinId))
        {
            var skinItem = App.factory.itemFactory.GetItem(cloudCatData.CatSkinData.UseSkinId);
            skinItem.Count++;
            cloudCatData.CatSkinData.UseSkinId = string.Empty;
        }
        
        cloudCatData.CatHealthData.SickId = sickId;
        ChangeSkin();
        App.system.cloudSave.UpdateCloudCatHealthData(cloudCatData);
    }
}