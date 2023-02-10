using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerSystem : SerializedMonoBehaviour
{
    private MyApplication app;

    protected MyApplication App
    {
        get
        {
            if (app == null)
            {
                app = FindObjectOfType<MyApplication>();
            }

            return app;
        }
    }
    
    #region InspectorVariable

    public PlayerDataSetting playerDataSetting;
    public Dictionary<string, string> playerStatus;

    #endregion

    #region Callback

    [HideInInspector] public delegate void ValueChange(object value);
    [HideInInspector] public delegate void ValueChangeFromTo(object from, object to);

    public ValueChange OnPlayerIdChange;
    public ValueChange OnPlayerNameChange;
    public ValueChange OnLevelChange;

    public ValueChangeFromTo OnExpChange;
    
    public ValueChange OnCoinChange;
    public ValueChange OnAddCoinChange;
    public ValueChange OnReduceCoinChange;
    
    public ValueChange OnDiamondChange;
    public ValueChange OnAddDiamondChange;
    public ValueChange OnReduceDiamondChange;

    public ValueChange OnCatMemoryChange;
    
    public ValueChange OnDiamondCatSlotChange;
    public ValueChange OnPlayerGenderChange;
    public ValueChange OnFriendIdsChange;
    public ValueChange OnReceiveFriendInvitesChange;

    public ValueChange OnUsingIconChange;
    public ValueChange OnUsingAvatarChange;

    public ValueChange OnCatDeadCountChange;

    #endregion

    #region Variable

    private string playerId;
    private string playerName;
    
    private int level;
    private int exp;
    
    private int diamondCatSlot;
    private int gridSizeLevel;
    
    private int playerGender = -1; //0:Male 1:Female
    
    private string usingIcon;
    private string usingAvatar;
    
    private int catDeadCount;

    #endregion

    public void Init()
    {
        Coin = Coin;
        Diamond = Diamond;
        CatMemory = CatMemory;
    }
    
    #region Properties

    public string PlayerId
    {
        get => playerId;
        set
        {
            playerId = value;
            OnPlayerIdChange?.Invoke(value);
        }
    }

    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            OnPlayerNameChange?.Invoke(value);
        }
    }

    public int Level
    {
        get => level;
        set
        {
            // if (value - level == 1 && level != 0)
            //     App.system.levelUp.Open();
            
            level = value;
            OnLevelChange?.Invoke(value);
        }
    }

    public int Exp
    {
        get => exp;
        set
        {
            float from = exp;
            float to = value;
            OnExpChange?.Invoke(from, to);
            exp = value;
        }
    }

    public int Coin
    {
        get => App.system.inventory.CommonData["Money"];
        set
        {
            App.system.inventory.CommonData["Money"] = value;
            OnCoinChange?.Invoke(value);
        }
    }

    public int Diamond
    {
        get => App.system.inventory.CommonData["Diamond"];
        set
        {
            App.system.inventory.CommonData["Diamond"] = value;
            OnDiamondChange?.Invoke(value);
        }
    }

    public int CatMemory
    {
        get => App.system.inventory.CommonData["CatMemory"];
        set
        {
            App.system.inventory.CommonData["CatMemory"] = value;
            OnCatMemoryChange?.Invoke(value);
        }
    }

    public int CatSlot
    {
        get
        {
            int levelCatSlot = playerDataSetting.GetCatSlotByLevel(Level);
            return levelCatSlot + diamondCatSlot;
        }
    }

    public int DiamondCatSlot
    {
        get => diamondCatSlot;
        set
        {
            diamondCatSlot = value;
            OnDiamondCatSlotChange(value);
        }
    }

    public int GridSizeLevel
    {
        get => gridSizeLevel;
        set => gridSizeLevel = value;
    }

    public int PlayerGender
    {
        get => playerGender;
        set
        {
            playerGender = value;
            OnPlayerGenderChange?.Invoke(value); //TODO Icon
        }
    }

    public string UsingIcon
    {
        get => usingIcon;
        set
        {
            usingIcon = value;
            if (string.IsNullOrEmpty(value))
                return;
            OnUsingIconChange?.Invoke(value);
        }
    }

    public string UsingAvatar
    {
        get => usingAvatar;
        set
        {
            usingAvatar = value;
            if (string.IsNullOrEmpty(value))
                return;
            OnUsingAvatarChange?.Invoke(value);
        }
    }

    public int CatDeadCount
    {
        get => catDeadCount;
        set
        {
            catDeadCount = value;
            OnCatDeadCountChange?.Invoke(value);
        }
    }

    #region Read-Only

    // 可養貓數
    public int CanAdoptCatCount
    {
        get
        {
            int count = App.system.room.FeatureRoomsCount;
            int slot = CatSlot;
            int cats = App.system.cat.GetCats().Count;

            count -= cats;
            slot -= cats;
            
            int result = 0;
            for (int i = 0; i < (count + slot); i++)
            {
                if (count <= 0) break;
                if (slot <= 0) break;
                result++;
                count--;
                slot--;
            }
            
            return result;
        }
    }

    // 升級所需經驗
    public int NextLevelExp => playerDataSetting.GetNextLevelUpExp(Level);

    #endregion

    #endregion

    #region Method

    public void AddExp(int value)
    {
        int result = Exp + value;
        int nextExp = NextLevelExp;
        
        app.controller.lobby.AddExpBuffer(value);

        // 判斷 是否會升等
        if (result >= nextExp)
        {
            int end = result - nextExp;
            Exp = nextExp;
            
            Level++;
            app.controller.lobby.AddLevelBuffer(Level);
            
            Exp = 0;
            Exp = end;
        }
        else
        {
            Exp = result;
        }
    }

    public void AddMoney(int value)
    {
        Coin += value;
        OnAddCoinChange?.Invoke(value);
        app.controller.lobby.AddMoneyBuffer(value);
    }
    
    public bool ReduceMoney(int value)
    {
        if (Coin - value < 0)
            return false;
        
        Coin -= value;
        OnReduceCoinChange?.Invoke(value);
        return true;
    }

    public void AddDiamond(int value)
    {
        Diamond += value;
        OnAddDiamondChange?.Invoke(value);
        app.controller.lobby.AddDiamondBuffer(value);
    }

    public bool ReduceDiamond(int value)
    {
        if (Diamond - value < 0)
            return false;

        Diamond -= value;
        OnReduceDiamondChange?.Invoke(value);
        return true;
    }

    #endregion
}
