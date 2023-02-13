using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Lobby : ModelBehavior
{
    private int tmpExp = -1;
    private int tmpLevel = -1;
    private int tmpMoney = -1;
    private int tmpDiamond = -1;

    private int expBuffer = -1;
    private int levelBuffer = -1;
    private int moneyBuffer = -1;
    private int diamondBuffer = -1;

    private int nextExpBuffer = -1;

    public int TmpExp
    {
        get => tmpExp;
        set => tmpExp = value;
    }

    public int TmpLevel
    {
        get => tmpLevel;
        set
        {
            if (value - tmpLevel == 1 && tmpLevel > 0)
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

    public int NextExpBuffer
    {
        get => nextExpBuffer;
        set => nextExpBuffer = value;
    }

    public ValueFromToChange OnTmpLevelChange;
    public ValueFromToChange OnTmpMoneyChange;
    public ValueFromToChange OnTmpDiamondChange;
}
