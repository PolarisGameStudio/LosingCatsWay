using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "PlayerDataSetting", menuName = "Factory/Create PlayerDataSetting")]
public class PlayerDataSetting : SerializedScriptableObject
{
    public int LittleGameExp;
    public int CatchCatExp;
    public int CatchCatCoin;

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
        if (chance == 3)
            return 100;
        if (chance == 2)
            return 70;
        if (chance == 1)
            return 40;
        return 20;
    }

    ///遊戲剩餘愛心換金幣 全額
    public int GetBigGameCoinsByChance(int level, int chance)
    {
        int total = GetBigGameCoinsByLevel(level);
        
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
    
    private int GetBigGameCoinsByLevel(int level)
    {
        if (level <= 10)
            return 250;
        if (level <= 20)
            return 300;
        if (level <= 30)
            return 400;
        if (level <= 40)
            return 500;
        if (level <= 50)
            return 550;
        if (level <= 60)
            return 650;
        if (level <= 70)
            return 750;
        if (level <= 80)
            return 800;
        if (level <= 90)
            return 900;
        return 1000;
    }

    public int GetLittleGameCoinsByLevel(int level)
    {
        if (level <= 10)
            return Random.Range(50, 60);
        if (level <= 20)
            return Random.Range(60, 80);
        if (level <= 30)
            return Random.Range(80, 100);
        if (level <= 40)
            return Random.Range(100, 110);
        if (level <= 50)
            return Random.Range(110, 130);
        if (level <= 60)
            return Random.Range(130, 150);
        if (level <= 70)
            return Random.Range(150, 160);
        if (level <= 80)
            return Random.Range(160, 180);
        if (level <= 90)
            return Random.Range(180, 200);
        return 200;
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

    [Button]
    private void Test(int level)
    {
        Debug.Log($"下一個等級的經驗：{GetNextLevelUpExp(level)}");
    }
}
