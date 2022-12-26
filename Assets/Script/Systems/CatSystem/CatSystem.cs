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

    [PropertyRange(0, 1)] [SerializeField] private float littleGameChance;

    private List<Cat> myCats = new List<Cat>();
    private List<Cat> deadCats = new List<Cat>();

    public CallbackValue OnCatsChange;
    public Callback OnCatDead;
    
    public void Init()
    {
        InvokeRepeating(nameof(CheckCatStatus), 0, 180);

        App.system.myTime.OnFirstLogin += DailyCheckCatStatus;
        App.system.myTime.OnFirstLogin += LoginCheckCatStatus;
        
        App.system.myTime.OnAlreadyLogin += LoginCheckCatStatus;
    }

    #region CatStatus

    private void DailyCheckCatStatus()
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            myCats[i].DailyCheckStatus();
        }
    }

    private void LoginCheckCatStatus()
    {
        for (int i = 0; i < myCats.Count; i++)
            myCats[i].LoginCheckStatus();

        for (int i = myCats.Count - 1; i >= 0; i--)
        {
            if (myCats[i].cloudCatData.CatServerData.IsDead)
                myCats[i].Death();
        }
    }

    private void CheckCatStatus()
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            Cat cat = myCats[i];

            for (int j = 0; j < 3; j++)
            {
                cat.SetMoisture();
                cat.SetSatiety();
                cat.SetFavorability();
            }

            cat.CheckStatus();
        }
    }

    #endregion

    #region Cats

    //TODO CallbackValue
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
        OnCatsChange?.Invoke(myCats);
    }

    /// 本地移除指定貓，伺服器不變(死亡、棄養)
    public void Remove(Cat cat)
    {
        myCats.Remove(cat);
        //Destroy(cat.gameObject);
        cat.gameObject.SetActive(false);
        OnCatsChange?.Invoke(myCats);
    }

    /// 讓貓移動到墓地
    public void SetDead(Cat cat)
    {
        Remove(cat);
        deadCats.Insert(0, cat);
        App.system.cloudSave.CreateCloudLosingCatData(cat.cloudCatData);
        App.system.cloudSave.DeleteCloudCatData(cat.cloudCatData);
        OnCatDead?.Invoke();
    }

    /// 取得所有墓地的貓
    public List<Cat> MyDeadCats()
    {
        return deadCats;
    }

    public void RefreshCatSkin()
    {
        for (int i = 0; i < myCats.Count; i++)
            myCats[i].ChangeSkin();
    }

    #endregion

    #region Game

    /// 暫停所有貓的關遊戲倒計時
    public void ToggleCatsGameTimer(bool pause)
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            myCats[i].ToggleDrawGamePause(pause);
            myCats[i].ToggleCancelGamePause(pause);
        }
    }

    public void ClaseCatsGame()
    {
        for (int i = 0; i < myCats.Count; i++)
        {
            myCats[i].CancelGame();
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
        transform.GetComponent<PolyNavMap>().enabled = true;
    }

    public void ClosePolyNav2D()
    {
        transform.GetComponent<PolyNavMap>().enabled = false;
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