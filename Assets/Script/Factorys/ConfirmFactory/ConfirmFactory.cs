using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmFactory : MvcBehaviour
{
    public string GetNormalContent(string key)
    {
        return App.factory.stringFactory.GetConfirmContent(key);
    }
    
    public string GetAddEndWordContent(string key)
    {
        return App.factory.stringFactory.GetConfirmContent(key);
    }
    
    public string GetNormalTitle(string key)
    {
        return App.factory.stringFactory.GetConfirmTitle(key);
    }
    
    public string GetAddEndWordTitle(string key)
    {
        return App.factory.stringFactory.GetConfirmTitle(key);
    }
}

public enum ConfirmTable
{
    Fix,
    
    // 表定
    Hints_Buy1,
    Hints_Buy2,
    Hints_Bug,
    Hints_MapNoCats,
    Hints_BuyComplete1,
    Hints_BuyComplete2,
    Hints_Adopt,
    Hints_NoMoney,
    Hints_NoDiamond,
    Hints_HasNewCat,
    Hints_NeedFeedRoom1,
    Hints_NeedFeedRoom2,
    Hints_NeedCatSlot1,
    Hints_NeedCatSlot2,
    Hints_CatFindSuccess,
    Hints_CatFindFail,
    Hints_CatCatchSuccess,
    Hints_CatCatchFail,
    Hints_Pause,
    Hints_Release1,
    Hints_Release2,
    Hints_Shelter1,
    Hints_Shelter2,
    Hints_Ligation,
    Hints_Name, // todo 確認玩家取名是不是用這個
    Hints_Rename, // todo 確認玩家改名是不是用這個
    Hints_Claim,
    Hints_PhotoTaken,
    Hints_CantFindCat,
    Hints_Refresh,
    Hints_Space,
    Hints_CantFindFriend,
    Hints_Maintain,
    Hints_VersionNotSame,
    Hints_Abandon1,
    Hints_Abandon2,
    Hints_NoProps,
    Hints_Pay,
    Hints_CancelPay,
    Hints_NullValue,
    Hints_LogOut,
    Hints_WatchTheAD,
    Hints_AdFail,
    Hints_GiveUp,
    Hints_NoMemory,
    Hints_AlreadyUsePotion,
    Hints_CantCheckYourself,
    Hints_AlreadyFriends,
    Hints_AlreadyInvited,
    Hints_InviteReceived,
    Hints_PutHere,
    Hints_MoveOut,
    Hints_UnlockCatSlot,
    Hints_TradeFail,
    Hints_UseCard,
    Hints_NoCard,
    Hints_DeleteAccount,
    Hints_AlreadyDeleteAccount,
    Hints_LateAdopt,
    Hints_Retrieve,
    Hints_Leave,
    Hints_Copy,
    Hints_ChooseSex,
    Hints_UsePotion,
    Hints_NoSameName,
    Hints_GoFriendsHome,
    Hints_GoHome,
}