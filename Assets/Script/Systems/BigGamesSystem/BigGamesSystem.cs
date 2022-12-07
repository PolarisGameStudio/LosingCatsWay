using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using System.Linq;

public class BigGamesSystem : MvcBehaviour
{
    [SerializeField] private UIView view;

    [SerializeField, PropertyRange(0, 1)] private float basicPercent;
    [SerializeField, PropertyRange(0, 1)] private float additionPercent;

    [Title("Games")]
    [SerializeField] private List<BigGameBehaviour> bigGames = new List<BigGameBehaviour>();

    private List<int> roomCounts = new List<int>();

    public Callback OnClose;

    #region Properties

    public List<BigGameBehaviour> GetBigGames()
    {
        return bigGames;
    }

    public BigGameBehaviour GetRandomGame()
    {
        Dictionary<int, float> percents = new Dictionary<int, float>();

        #region 根據遊戲列表依序的類型計算相關房間的數量

        roomCounts.Clear();

        for (int i = 0; i < GetBigGames().Count; i++)
        {
            BigGameBehaviour game = GetBigGames()[i];
            int count = GetCountByRoomGameType(game.gameType);
            roomCounts.Add(count);
        }

        #endregion

        #region 爲每個遊戲創建索引+概率

        for (int i = 0; i < GetBigGames().Count; i++)
        {
            float f = basicPercent;
            int count = roomCounts[i];

            f += additionPercent * count;

            percents.Add(i, f);
        }
        
        #endregion
        
        #region 排序並隨機遊戲

        var sortedPercents = percents.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        
        for (int i = 0; i < sortedPercents.Count; i++)
        {
            if (i >= sortedPercents.Count - 1)
            {
                int lastIndex = sortedPercents.ElementAt(i).Key;
                return GetBigGames()[lastIndex];
            }
            
            // float percent = sortedPercents.ElementAt(i).Value;
            if (Random.value > 0.5f)
                continue;
            int index = sortedPercents.ElementAt(i).Key;
            return GetBigGames()[index];
        }
        
        print("NoLast");
        return GetBigGames()[Random.Range(0, GetBigGames().Count)];

        #endregion
    }

    #endregion

    // 計算相關遊戲房的數量
    private int GetCountByRoomGameType(RoomGameType roomGameType)
    {
        int result = 0;
        var rooms = App.system.room.MyRooms;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomData.roomType != RoomType.Game) continue;
            if (rooms[i].roomData.roomGamesType != roomGameType) continue;
            result++;
        }

        return result;
    }

    [Button]
    public void OpenRandomGame()
    {
        if (GetBigGames().Count <= 0) return;

        App.system.transition.Active(0, () => 
        {
            GetRandomGame().Open();
        });
    }

    [Button]
    public void DebugOpenGame(int index)
    {
        App.system.transition.Active(0, () => 
        {
            GetBigGames()[index].Open();
        });
    }
    
    public void OpenGame(BigGameBehaviour bigGameBehaviour)
    {
        App.system.transition.Active(0, () =>
        {
            bigGameBehaviour.Open();
        });
    }

    public void Open()
    {
        App.system.cat.ToggleCatsGameTimer(true);
        view.InstantShow();
    }

    public void Close()
    {
        App.system.cat.ToggleCatsGameTimer(false);
        view.InstantHide();
        OnClose?.Invoke();
    }
}
