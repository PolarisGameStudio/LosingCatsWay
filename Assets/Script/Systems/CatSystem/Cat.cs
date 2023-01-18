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

    public float littleGameBubbleDistance;
    public float bigGameBubbleDistance;

    public CloudCatData cloudCatData;
    public bool isFriendMode = false;

    //GameType
    [HideInInspector] public int littleGameIndex;
    [HideInInspector] public BigGameBehaviour bigGameBehaviour;
    [HideInInspector] public string catNotifyId; //每次刷遊戲覆蓋
    [HideInInspector] public bool isPauseGame;

    public CatRewardCanvas CatRewardCanvas;

    [Title("Effects")] public ParticleSystem catHeartEffect;

    // ID變化

    private PolyNavAgent polyNavAgent;
    private DirectionChecker directionChecker;
    private Animator anim; // 之後統一換這個

    private int CancelGameTimer = 0;
    private int DrawGameTimer = 0;

    #endregion

    private Room specialSpineRoom;

    public void Active()
    {
        polyNavAgent = GetComponent<PolyNavAgent>();
        directionChecker = GetComponent<DirectionChecker>();

        anim = GetComponent<Animator>();

        if (isFriendMode)
            return;

        if (catHeartEffect.isPlaying) catHeartEffect.Stop();

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
        
        specialSpineRoom.spcialSpineIsUse = false;
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
        littleGameBubble.DOLocalMoveY(littleGameBubbleDistance, 0.5f).From(1);

        App.system.catNotify.Add(this);
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
        bigGameBubble.DOLocalMoveY(bigGameBubbleDistance, 0.5f).From(1);

        //TODO Move

        App.system.catNotify.Add(this);
    }

    public void CloseBigGame()
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

        if (isPauseGame)
            return;

        CancelGame();

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

        if (isPauseGame)
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
        
        CheckSick();
    }

    // 每天固定一次
    public void DailyCheckStatus()
    {
        GetLikeSnack();
        GetLikeSoup();

        if (cloudCatData.CatServerData.IsDead) return;
        
        if (cloudCatData.CatHealthData.SickId is "SK001" or "SK002")
        {
            if (Random.value < 0.9f)
            {
                cloudCatData.CatServerData.IsDead = true;
                App.system.cloudSave.UpdateCloudCatServerData(cloudCatData);
                return;
            }

            CheckNaturalDead();
        }
        
        CheckNaturalDead();
        
        if (cloudCatData.CatServerData.IsDead) return;

        //表定順序
        if (string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            cloudCatData.CatHealthData.SickId = App.factory.sickFactory.GetCatSick(cloudCatData);
        if (string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            cloudCatData.CatHealthData.SickId = App.factory.sickFactory.GetVaccineSicks(cloudCatData);
        if (string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            cloudCatData.CatHealthData.SickId = App.factory.sickFactory.GetLigationSicks(cloudCatData);
        //得病換皮
        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            ChangeSkin();

        CheckBug();
    }

    private void CheckNaturalDead()
    {
        float percent = App.factory.catFactory.catDataSetting.NaturalDeadPercent(cloudCatData.CatData.SurviveDays);
        if (Random.value > percent)
            return;
        cloudCatData.CatServerData.IsDead = true;
        App.system.cloudSave.UpdateCloudCatServerData(cloudCatData);
    }

    // 每次上線執行一次
    public void LoginCheckStatus()
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

    private void CheckSick()
    {
        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
        {
            isPauseGame = true;
            return;
        }
        
        cloudCatData.CatHealthData.SickId = App.factory.sickFactory.GetCatSick(cloudCatData);
        
        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
        {
            CancelGame();
            isPauseGame = true;
            
            cloudCatData.CatHealthData.MetDoctorCount =
                App.factory.sickFactory.GetMetCount(cloudCatData.CatHealthData.SickId);
            App.system.cloudSave.UpdateCloudCatHealthData(cloudCatData);
            ChangeSkin();
        }
    }

    // 檢查長蟲
    private void CheckBug()
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
    }

    public void CloseFace()
    {
        catSkin.CloseFace();
    }
}