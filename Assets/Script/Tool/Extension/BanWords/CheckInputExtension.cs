using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckInputExtension
{
    public static bool CheckInputNameCanUse(string str)
    {
        if (CheckHasBlankSpaceExists(str)) return false; //有空格 //False 不可
        if (CheckHasBanWord(str)) return false; //有違禁 //False 不可
        return true;
    }

    private static bool CheckHasBanWord(string str)
    {
        BanWordData data = Resources.Load("Data/BanWordData") as BanWordData;

        for (int i = 0; i < data.banWords.Length; i++)
        {
            if (str.ToUpper().Contains(data.banWords[i].ToUpper())) return true;
        }

        return false;
    }

    //之後能做拆解算法
    /*
     * 例如：禁用詞：Fuck 匹配詞：Fuuck
     * 檢查是否存在F
     * 若True，檢查是否存在uck，或逐字拆
     * 避免人名衝突，檢查相連字
     * F之後，是否存在uc或ck
     * 我在共三小
     */

    private static bool CheckHasBlankSpaceExists(string str)
    {
        str = str.Replace(" ", "");
        if (String.IsNullOrEmpty(str)) return true;
        return false;
    }
}
