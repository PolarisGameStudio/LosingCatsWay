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
    #region LittleGame

    LittleGameSingStart,
    LittleGameSingEnd,
    LittleGameWatchStart,
    LittleGameWatchEnd,
    LittleGameTouchCatStart,
    LittleGameTouchCatEnd,
    LittleGameScreenshotStart,
    LittleGameScreenshotEnd,
    LittleGameTalkStart,
    LittleGameTalkEnd,
    LittleGameHandStart,
    LittleGameHandEnd,
    LittleGameSleepStart,
    LittleGameSleepEnd,
    LittleGameAssStart,
    LittleGameAssEnd,
    LittleGameSmellStart,
    LittleGameSmellEnd,

    #endregion

    FindGameSuccess,
    FindGameFailed,

    #region Abandon

    AbandonCatConfirm,
    AbandonCatDone,

    #endregion

    #region Adopt

    AdoptionConfirm,
    AdoptionDone,
    AdoptionFailedByCatCountNotEnough,

    #endregion

    BackToHomeConfirm,
    BackConfirm,
    BoughtSuccess,
    CatchGameSuccess,
    CatchGameFailed,
    CaptureScreenshotSuccess,
    ExitComfirm,
    NotEnoughCoin,
    NotEnoughDiamond,
    RefreshConfirm,
    Copied,
    NoFeedAnyCats,

    #region Rename

    NotAllowBlank,
    NotAllowBanWord,
    RenameConfirm,

    #endregion

    #region Friend

    SearchFriendFail,

    #endregion

    #region Common

    NoData,

    #endregion

    #region Login

    ConfirmSignOut,

    #endregion
    
    Fix,

    BuyConfirm,
    MapNoCats,
    IsAddToBag,
    IsAddToShop,
    ShelterNoCats,
    AdoptConfirm,
    NoMoney,
    NoDiamond,
    HasNewCat,
    NeedMoreFeedRoom,
    NeedMoreCatSlot,
    CatchCatGameEnd,
    ConfirmFindCatWithoutAdopt, // 確定在無法收養下進入找貓
    ChooseGenderConfirm,
    
    // 表定
    Hints_Buy1,
    Hints_Buy2, //todo 確認內容
    Hints_Bug,
    Hints_MapNoCats,
    Hints_BuyComplete1,
    Hints_BuyComplete2,
    Hints_Adopt,
    Hints_NoMoney,
    Hints_NoDiamond,
    Hints_HasNewCat,
    Hints_NeedFeedRoom,
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
    Hints_Name,
    Hints_Rename,
    Hints_Claim,
    Hints_PhotoTaken,
    Hints_CantFindCat,
    //todo Excel95 - Hints_CantFindCat
    Hints_Refresh,
    Hints_Space,
    Hints_CantFindFriend,
    Hints_Maintain,
    Hints_Abandon1,
    Hints_Abandon2,
    Hints_NoProps,
    Hints_Pay,
    Hints_NullValue,
    Hints_LogOut,
    Hints_WatchTheAD,
    Hints_AdFail,
    // todo Excel111 - Hints_
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
    // todo Excel127 - Hints_
}