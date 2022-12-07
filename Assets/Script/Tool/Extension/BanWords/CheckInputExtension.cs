using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckInputExtension
{
    public static bool CheckInputNameCanUse(string str)
    {
        if (CheckHasBlankSpaceExists(str)) return false; //���Ů� //False ���i
        if (CheckHasBanWord(str)) return false; //���H�T //False ���i
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

    //����వ��Ѻ�k
    /*
     * �Ҧp�G�T�ε��GFuck �ǰt���GFuuck
     * �ˬd�O�_�s�bF
     * �YTrue�A�ˬd�O�_�s�buck�A�γv�r��
     * �קK�H�W�Ĭ�A�ˬd�۳s�r
     * F����A�O�_�s�buc��ck
     * �ڦb�@�T�p
     */

    private static bool CheckHasBlankSpaceExists(string str)
    {
        str = str.Replace(" ", "");
        if (String.IsNullOrEmpty(str)) return true;
        return false;
    }
}
