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

    #region Final

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

    #endregion
}