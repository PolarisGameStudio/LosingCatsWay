using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "PlayerDataSetting", menuName = "Factory/Create PlayerDataSetting")]
public class PlayerDataSetting : SerializedScriptableObject
{
    [SerializeField] private int BigGameExp;
    public int LittleGameExp;
    public int CatchCatExp;
    public int CatchCatCoin;
    [SerializeField] private Dictionary<int, int> BigGameCoinsByLevel = new Dictionary<int, int>();

    //下次升級經驗
    public int GetNextLevelUpExp(int level)
    {
        var log = Mathf.Log(3);
        var round = Mathf.RoundToInt(Mathf.Pow(level, log) * 17);
        var result = round * 10;
        return result;
    }

    //遊戲剩餘愛心換經驗
    public int GetBigGameExpByChance(int chance)
    {
        float f = (BigGameExp / 3f * chance) / 10f;
        return Convert.ToInt32(Mathf.Round(f) * 10);
    }

    ///遊戲剩餘愛心換金幣 全額
    public int GetBigGameCoinsByChance(int level, int chance)
    {
        MathfExtension.GetNumberRangeByTen(level, out int start, out int end);
        int total = BigGameCoinsByLevel.ContainsKey(end) ? BigGameCoinsByLevel[end] : BigGameCoinsByLevel.Values.Last();
        
        int result;
        switch (chance)
        {
            case 3:
                result = total;
                break;
            case 2:
                result = (int)(total * 0.7f);
                break;
            case 1:
                result = (int)(total * 0.4f);
                break;
            default:
                result = 0;
                break;
        }
        
        return result;
    }

    public int GetLittleGameCoinsByLevel(int level)
    {
        if (level < 10)
            return 30;
        if (level < 20)
            return 54;
        if (level < 30)
            return 78;
        if (level < 40)
            return 102;
        if (level < 50)
            return 120;
        if (level < 60)
            return 138;
        if (level < 70)
            return 156;
        if (level < 80)
            return 168;
        if (level < 90)
            return 180;
        return 186;
    }

    public int GetCatSlotByLevel(int level)
    {
        if (level < 2) return 1;
        if (level < 10) return 2;
        if (level < 25) return 3;
        if (level < 40) return 4;
        if (level < 55) return 5;
        if (level < 70) return 6;
        if (level < 85) return 7;
        return 8;
    }
}
