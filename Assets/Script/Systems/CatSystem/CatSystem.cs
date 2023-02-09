using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using PolyNav;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatSystem : MvcBehaviour
{
    [SerializeField] private Cat catObject;
    [SerializeField] private Cat kittyCatObject;
    [SerializeField] private AngelCat angelCatObject;

    private List<Cat> myCats = new List<Cat>();

    private List<CloudLosingCatData> _losingCatDatas = new List<CloudLosingCatData>();
    private AngelCat _angelCat;

    public Callback OnCatDead;

    public void Init()
    {
        InvokeRepeating(nameof(CheckCatStatus), 0, 180);
        RefreshRedPoint();
        
        App.system.myTime.OnFirstLogin += CheckCatsStatusPerDay;
        App.system.myTime.OnAlreadyLogin += CheckCatsStatusPerLogin;
    }

    #region CatStatus

    private async void CheckCatsStatusPerDay()
    {
        for (int i = 0; i < myCats.Count; i++) // 計算上次離開之後的三項
            myCats[i].CheckCatStatusPerLogin();

        for (int i = 0; i < myCats.Count; i++) // 檢查會不會死亡 離家出走 生病 跳蚤
            myCats[i].CheckCatStatusPerDay();

        for (int i = myCats.Count - 1; i >= 0; i--) // 如果要死就進入死亡流程
            if (myCats[i].cloudCatData.CatServerData.IsDead)
            {
                if (i == 0)
                    App.model.entrance.DeadCat = myCats[i];
                await SetDead(myCats[i]);
            }

        App.model.entrance.LosingCatDatas = _losingCatDatas; // 僅本日死亡

        if (_losingCatDatas.Count <= 0)
            return;

        var lastLosingCatDatas = App.model.cloister.LosingCatDatas; // 以前死亡的貓
        lastLosingCatDatas.AddRange(_losingCatDatas);
        App.model.cloister.LosingCatDatas = lastLosingCatDatas;
    }

    private void CheckCatsStatusPerLogin()
    {
        for (int i = 0; i < myCats.Count; i++)
            myCats[i].CheckCatStatusPerLogin(); // 計算上次離開之後的三項
    }

    private void CheckCatStatus()
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            myCats[i].CheckStatus();
            myCats[i].CheckCatSickByStatus();
        }

        RefreshRedPoint();
    }

    public void RefreshRedPoint()
    {
        bool hasRed = false;

        for (int i = 0; i < myCats.Count; i++)
        {
            if (hasRed)
                continue;
            
            var cat = myCats[i];
            var mood = CatExtension.GetCatMood(cat.cloudCatData);
            if (mood == 2 || mood == 3)
                hasRed = true;
        }

        App.view.lobby.catRedPoint.SetActive(hasRed);
    }

    #endregion

    #region Cats

    public List<Cat> GetCats()
    {
        return myCats;
    }

    public Cat CreateCatObject(CloudCatData cloudCatData)
    {
        Cat tmp = catObject;

        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0)
            tmp = kittyCatObject;

        Cat cat = Instantiate(tmp, transform);
        cat.SetCloudCatData(cloudCatData);

        Vector3 randomPostition = App.system.room.GetRandomRoomPosition();
        cat.transform.position = randomPostition;

        cat.Active();

        Add(cat);

        return cat;
    }

    /// 增加貓，伺服器不變(領養、捕獲等)
    private void Add(Cat cat)
    {
        myCats.Add(cat);
    }

    /// 本地移除指定貓，伺服器不變(死亡、棄養)
    public void Remove(Cat cat)
    {
        myCats.Remove(cat);
        cat.gameObject.SetActive(false);
    }

    /// 讓貓移動到墓地
    private async Task SetDead(Cat cat)
    {
        cat.Death();
        Remove(cat);

        App.system.catNotify.Remove(cat);

        App.system.player.CatDeadCount += 1;
        // deadCats.Insert(0, cat);

        var losingCatData = await App.system.cloudSave.CreateCloudLosingCatData(cat.cloudCatData);
        _losingCatDatas.Add(losingCatData);

        App.system.cloudSave.DeleteCloudCatData(cat.cloudCatData);

        //TODO ValueChange

        OnCatDead?.Invoke();
    }

    /// 取得所有墓地的貓
    // public List<Cat> MyDeadCats()
    // {
    //     return deadCats;
    // }
    public void RefreshCatSkin()
    {
        for (int i = 0; i < myCats.Count; i++)
            myCats[i].ChangeSkin();
    }

    #endregion

    #region Game

    /// 暫停所有貓遊戲
    public void PauseCatsGame(bool pause)
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            myCats[i].isPauseGame = pause;
        }
    }

    #endregion

    #region Lobby

    public void CheckCatNeedLeftRoom()
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            Cat cat = myCats[i];

            var position = cat.transform.position;
            int gridX = (int)(position.x / 5.12);
            int gridY = (int)(position.y / 5.12);

            int gridValue = App.system.grid.GetGrid(gridX, gridY).Value;

            if (gridValue != 1)
            {
                cat.transform.position = App.system.room.GetRandomRoomPosition();
                cat.Reset();
            }
        }
    }

    //為了優化 不關會過度消耗
    public void OpenPolyNav2D()
    {
        // transform.GetComponent<PolyNavMap>().enabled = true;
    }

    public void ClosePolyNav2D()
    {
        // transform.GetComponent<PolyNavMap>().enabled = false;
    }

    #endregion

    #region AngelCat

    public void CheckAngelCat()
    {
        List<CloudLosingCatData> losingCatDatas = App.model.cloister.LosingCatDatas;
        CloudLosingCatData angelCat = losingCatDatas.Find(x => x.LosingCatStatus.Contains("AngelCat"));

        if (angelCat == null)
            return;

        if (_angelCat != null)
            return;

        AngelCat cat = Instantiate(angelCatObject, transform);
        cat.SetCloudCatData(angelCat);

        Vector3 randomPostition = App.system.room.GetRandomRoomPosition();
        cat.transform.position = randomPostition;

        _angelCat = cat;
    }

    #endregion

    public int GetFavoriteCatIndex()
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            if (myCats[i].cloudCatData.CatData.IsFavorite)
                return i;
        }

        return -1; // -1 就是沒有
    }
}