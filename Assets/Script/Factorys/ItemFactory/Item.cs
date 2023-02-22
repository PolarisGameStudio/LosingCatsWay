using System;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Factory/Create Item")]
public class Item : ScriptableObject
{
    private MyApplication myApp = null;
    private MyApplication app
    {
        get
        {
            if (myApp == null)
                myApp = FindObjectOfType<MyApplication>();
            
            return myApp;
        }
    }
    
    public string id;
    [Space(10)] [EnumPaging] public ItemType itemType;
    [EnumPaging] public ItemBoughtType itemBoughtType;

    #region Feed

    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed"), EnumPaging]
    public ItemFeedType itemFeedType;

    [ShowIf("itemFeedType", ItemFeedType.Food), BoxGroup("Feed"), EnumPaging]
    public FoodType foodType;

    [ShowIf("itemFeedType", ItemFeedType.Snack), BoxGroup("Feed"), EnumPaging]
    public SnackType snackType;
    
    [ShowIf("itemFeedType", ItemFeedType.Water), BoxGroup("Feed"), EnumPaging]
    public WaterType waterType;

    [Title("LikeValue")]
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int likeSatiety;
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int likeMoisture;
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int likeFun;

    [Title("NormalValue")]
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int normalSatiety;
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int normalMoisture;
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int normalFun;
    
    [Title("HateValue")]
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int hateSatiety;
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int hateMoisture;
    [ShowIf("itemType", ItemType.Feed), BoxGroup("Feed")] public int hateFun;

    #endregion

    #region Tool

    [ShowIf("itemType", ItemType.Tool)] [EnumPaging]
    public ItemToolType itemToolType = ItemToolType.Normal;

    #endregion

    #region Litter

    [ShowIf("itemType", ItemType.Litter)] [EnumPaging]
    public ItemLitterType itemLitterType;

    #endregion

    [Space(10)] public int price;

    [InlineEditor(InlineEditorModes.GUIAndPreview)] public Sprite icon;
    [InlineEditor(InlineEditorModes.GUIAndPreview)] public Sprite content;

    public bool canUse;
    public bool notShowAtBag;

    [ShowIf("@itemType == ItemType.CatSkin")]
    public int skinLevel;

    public int unlockLevel;

    [ShowIf("@itemBoughtType == ItemBoughtType.Cash")]
    public string purchaseKey;
    
    #region Properties

    public int Count
    {
        get
        {
            int value = 0;

            switch (itemType)
            {
                case ItemType.Feed:
                    value = app.system.inventory.FoodData[id];
                    break;
                case ItemType.Tool:
                    value = app.system.inventory.ToolData[id];
                    break;
                case ItemType.Litter:
                    value = app.system.inventory.LitterData[id];
                    break;
                case ItemType.Room:
                    value = app.system.inventory.RoomData[id];
                    break;
                case ItemType.Play:
                    value = 1;
                    break;
                case ItemType.CatSkin:
                    value = app.system.inventory.SkinData[id];
                    break;
                case ItemType.Special:
                    value = app.system.inventory.ToolData[id];
                    break;
                case ItemType.Icon:
                    value = app.system.inventory.PlayerIconData[id];
                    break;
                case ItemType.Avatar:
                    value = app.system.inventory.PlayerAvatarData[id];
                    break;
                case ItemType.Common:
                    value = app.system.inventory.CommonData[id];
                    break;
            }

            return value;
        }
        set
        {
            switch (itemType)
            {
                case ItemType.Feed:
                    app.system.inventory.FoodData[id] = value;
                    break;
                case ItemType.Tool:
                    app.system.inventory.ToolData[id] = value;
                    break;
                case ItemType.Litter:
                    app.system.inventory.LitterData[id] = value;
                    break;
                case ItemType.Room:
                    app.system.inventory.RoomData[id] = value;
                    break;
                case ItemType.CatSkin:
                    app.system.inventory.SkinData[id] = value;
                    break;
                case ItemType.Special:
                    app.system.inventory.ToolData[id] = value;
                    break;
                case ItemType.Icon:
                    app.system.inventory.PlayerIconData[id] = value;
                    break;
                case ItemType.Avatar:
                    app.system.inventory.PlayerAvatarData[id] = value;
                    break;
                case ItemType.Common:
                    app.system.inventory.CommonData[id] = value;
                    break;
            }
        }
    }

    public string Name => app.factory.stringFactory.GetItemName(id);

    public string Description => app.factory.stringFactory.GetItemDescription(id);

    // 吃的
    public bool ForSatiety => likeSatiety > 0;

    // 喝的
    public bool ForMoisture => likeMoisture > 0;

    // 心情的
    public bool ForFun => likeFun > 0;

    public bool Unlock
    {
        get
        {
            if (app.model.mall.PurchaseRecords.ContainsKey(purchaseKey))
                return true;
            if (unlockLevel > 0 && app.system.player.Level >= unlockLevel)
                return true;
            return false;
        }
    }

    #endregion
}