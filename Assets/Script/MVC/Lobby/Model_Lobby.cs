using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Lobby : ModelBehavior
{
    private int tmpExp;
    private int tmpLevel;
    private int tmpMoney;
    private int tmpDiamond;

    private int expBuffer;
    private int levelBuffer;
    private int moneyBuffer;
    private int diamondBuffer;

    public int TmpExp
    {
        get => tmpExp;
        set
        {
            OnTmpExpChange?.Invoke(tmpExp, value);
            tmpExp = value;
        }
    }

    public int TmpLevel
    {
        get => tmpLevel;
        set
        {
            OnTmpLevelChange?.Invoke(tmpLevel, value);
            tmpLevel = value;
        }
    }

    public int TmpMoney
    {
        get => tmpMoney;
        set
        {
            OnTmpMoneyChange?.Invoke(tmpMoney, value);
            tmpMoney = value;
        }
    }

    public int TmpDiamond
    {
        get => tmpDiamond;
        set
        {
            OnTmpDiamondChange?.Invoke(tmpDiamond, value);
            tmpDiamond = value;
        }
    }

    public int ExpBuffer
    {
        get => expBuffer;
        set => expBuffer = value;
    }

    public int LevelBuffer
    {
        get => levelBuffer;
        set => levelBuffer = value;
    }

    public int MoneyBuffer
    {
        get => moneyBuffer;
        set => moneyBuffer = value;
    }

    public int DiamondBuffer
    {
        get => diamondBuffer;
        set => diamondBuffer = value;
    }

    public ValueFromToChange OnTmpExpChange;
    public ValueFromToChange OnTmpLevelChange;
    public ValueFromToChange OnTmpMoneyChange;
    public ValueFromToChange OnTmpDiamondChange;
}
