using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //基本資料
    public string PlayerId;
    public string PlayerName;

    //等級經驗
    public int Level;
    public int Exp;

    //$
    public int Diamond;
    public int Coin;
    
    //Cat
    public int DiamondCatSlot;
    
    //GridSize
    public int GridSize;

    public int PlayerGender = -1; //0:Male 1:Female

    //允許數
    public int CanAdoptCatCount;
}
