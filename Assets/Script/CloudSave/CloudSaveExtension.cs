using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

public static class CloudSaveExtension
{
    public static string CurrentUserId
    {
        get
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            return auth.CurrentUser.UserId;
        }
    }

    public static Dictionary<string, object> ToDict(this CloudSaveData data)
    {
        Dictionary<string, object> result = new Dictionary<string, object>
        {
            {"PlayerData", data.PlayerData},
            {"FriendData", data.FriendData},
            {"TimeData", data.TimeData},
            {"SignData", data.SignData},
            {"ItemData", data.ItemData},
            {"MissionData", data.MissionData},
            {"ExistRoomDatas", data.ExistRoomDatas}
        };

        return result;
    }
}