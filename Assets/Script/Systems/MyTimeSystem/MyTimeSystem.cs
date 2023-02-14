using Firebase.Firestore;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class MyTimeSystem : MvcBehaviour
{
    public DateTime AccountCreateDateTime; //Account created date
    public DateTime PerDayLoginDateTime;
    public DateTime LastLoginDateTime;

    /// 每天固定執行一次的事情：貓死亡檢查等
    public Callback OnFirstLogin;
    /// 每次上線都會執行的事情
    public Callback OnAlreadyLogin;

    #region Method
    
    [Button]
    public void Init()
    {
        bool login = IsTodayLogin();
        if (login)
        {
            OnAlreadyLogin?.Invoke();
            OnAlreadyLogin = null;
        }
        else
        {
            PerDayLoginDateTime = MyTimeNow;
            OnFirstLogin?.Invoke();
            OnFirstLogin = null;
        }
    }

    public bool IsTodayLogin()
    {
        DateTime nowTime = MyTimeNow;

        if (nowTime.Year != PerDayLoginDateTime.Year)
            return false;
        if (nowTime.Month != PerDayLoginDateTime.Month)
            return false;
        if (nowTime.Day != PerDayLoginDateTime.Day)
            return false;
        
        return true;
    }

    /// 0:First 1:Already 2:FromFriendLobby
    public int BackLobbyStatus()
    {
        bool login = IsTodayLogin();
        int result;

        if (!login)
            result = 0;
        else
            result = 1;

        if (PlayerPrefs.HasKey("FriendRoomId"))
            result = 2;
        
        return result;
    }
    
    public void SetDateTime()
    {
        LastLoginDateTime = MyTimeNow;
    }

    public DateTime MyTimeNow => Timestamp.GetCurrentTimestamp().ToDateTime().ToLocalTime();

    #endregion
    
    public int GetWeekOfYear(DateTime dt)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        return weekOfYear;
    }
}
