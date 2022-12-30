using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; //If S.Object

[System.Serializable]
public class RoomData
{
    public string id;
    // [HideInInspector] public string guid;

    [EnumToggleButtons] public RoomSizeType roomSizeType;
    [EnumToggleButtons] public RoomType roomType;
    [EnumToggleButtons] public RoomSortType roomSortType;
    
    [ShowIf("roomType", RoomType.Game)] [EnumToggleButtons]
    public RoomGameType roomGamesType;
    [ShowIf("roomType", RoomType.Other)] [EnumToggleButtons]
    public RoomOtherType roomOtherType;
}
