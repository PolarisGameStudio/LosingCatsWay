using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyTable
{
    public static int GetRoomWidth(RoomSizeType type)
    {
        switch (type)
        {
            case RoomSizeType.One_One:
                return 1;
            case RoomSizeType.Two_Two:
                return 2;
            case RoomSizeType.Three_Three:
                return 3;
        }

        return 0;
    }

    public static int GetRoomHeight(RoomSizeType type)
    {
        switch (type)
        {
            case RoomSizeType.One_One:
                return 1;
            case RoomSizeType.Two_Two:
                return 2;
            case RoomSizeType.Three_Three:
                return 3;
        }

        return 0;
    }
}

public enum CatAnimTable
{
    IsNeedIdle,
    IsCanExit,
    IsLeftRoom,

    IsWalk,
    IsRun,
    IsSleep,
    IsGrasp,
    IsSit,

    IdleSelectIndex,
    IdleActionSelectIndex,
    WalkSelectIndex,
    RunSelectIndex,
    SleepSelectIndex,
    GraspSelectIndex,
    SitSelectIndex,
    SitActionSelectIndex,
    
    LittleGame,
    LittleGameIndex,
    LittleGameHandEndStatus,
    
    TraitType,
    TraitIndex,
    ToTrait,
    
    SpcialSpineRoomId,
    
    IsPersonality,
    Personality
}

public enum ItemType
{
    All = 0,
    Feed = 1,
    Tool = 2,
    Litter = 3,
    Room = 4,
    Special = 5, //ISL
    Icon = 6,
    CatSkin = 7,

    //CatchGame = 8, // => Tool
    Coin = 9,
    Diamond = 10,
    Play = 11,
    Unlock = 12,
    Avatar = 13,
    Common = 14
}

public enum ItemBoughtType
{
    Coin = 1,
    Diamond = 2,
    Cash = 3,
    Free = 4
}

public enum ItemFeedType
{
    // 乾糧
    Food = 1,

    // 水
    Water = 2,

    // 零食
    Snack = 3,
    
    // 罐頭
    Can = 4,
}

public enum ItemToolType
{
    Normal = 1,
    Catch = 2,
    Package = 3,
}

public enum FoodType //喜歡討厭
{
    Chicken = 0,
    Fish = 1,
    Duck = 2,
    // 絕對喜歡
    Ultimate = 3
}

public enum SnackType
{
    Meat = 0, //肉泥
    Fish = 1, //小魚乾
    Jelly = 2, //凍乾
}

public enum WaterType
{
    Water = 0,
    Milk = 1,
    FishSoup = 2,
    ChickSoup = 3,
}

public enum ItemLitterType //喜歡討厭
{
    Mineral = 0,
    Tofu = 1,
    Wood = 2,
}

public enum RoomSizeType
{
    One_One = 0,
    Two_Two = 1,
    Three_Three = 2
}

public enum RoomType
{
    Features = 0,
    Game = 1,
    Path = 2,
    Special = 3,
    Other = 4,
}

public enum RoomFeaturesType
{
    None = 0,
    Food = 1,
    Litter = 2,
}

public enum RoomGameType
{
    None = 0,
    Litter = 1,
    Meat = 2,
    Teaser = 3,
    Teeth = 4,
    Nail = 5
}

public enum RoomOtherType
{
    Decorate = 0,
    //todo 因爲會加更多細項在「其他」
}

public enum RoomBoughtType
{
    Coin = 1,
    Diamond = 2,
    Cash = 3
}

public enum RoomSortType
{
    Normal = 1,
    Limit = 2,
    Event = 3,
}

public enum MallItemRefreshType
{
    Infinity = -1,
    OnlyOne = 0,
    PerDay = 1,
    PerWeek = 2,
    PerMonth = 3,
    MonthlyCard = 4
}


#region Variety

public enum PurebredCatType
{
    G_American,
    B_American,
    O_American,
    C_American,
    K_American,
    G_American_O,
    B_American_O,
    O_American_O,
    C_American_O,
    K_American_O,

    //O_Munchkin,
    //G_Munchkin_O,
    //G_Munchkin,
    //C_Munchkin,
    //D_Munchkin,
    Siamese_1,
    Siamese_2,
    Siamese_3,
    Siamese_4,
    Siamese_5
}

public enum MixedCatType
{
    White,
    Black,
    Benz,
    Cow,
    Gray,
    G_Benz,
    G_Cow,
    DB_Tabby,
    DB_Mackerel,
    DG_Tabby,
    DG_Mackerel,
    LB_Tabby,
    LB_Mackerel,
    PW_Tabby,
    PW_Mackerel,
    YB_Tabby,
    YB_Mackerel,
    G_Tabby,
    G_Mackerel,
    O_Tabby,
    O_Mackerel,
    C_Tabby,
    C_Mackerel,
    DB_Tabby_O,
    DB_Mackerel_O,
    DG_Tabby_O,
    DG_Mackerel_O,
    LB_Tabby_O,
    LB_Mackerel_O,
    PW_Tabby_O,
    PW_Mackerel_O,
    YB_Tabby_O,
    YB_Mackerel_O,
    G_Tabby_O,
    G_Mackerel_O,
    O_Tabby_O,
    O_Mackerel_O,
    C_Tabby_O,
    C_Mackerel_O,
    K_Calico,
    T_Calico,
    P_Calico,
    PT_Calico,
    K_Tortoiseshell,
    T_Tortoiseshell,
    K_Tortoiseshell_O,
    T_Tortoiseshell_O,
    P_Tortoiseshell,
    PT_Tortoiseshell,
    P_Tortoiseshell_O,
    PT_Tortoiseshell_O,
    P_Black,
    P_Gray,
    P_DB_Tabby,
    P_DG_Tabby,
    P_LB_Tabby,
    P_PW_Tabby,
    P_YB_Tabby,
    P_G_Tabby,
    P_O_Tabby,
    P_C_Tabby,
    P_K_Calico,
    P_T_Calico,
    P_P_Calico,
    P_PT_Calico,
    GT_Siamese,
    CT_Siamese
}

#endregion