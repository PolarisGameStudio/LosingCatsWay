using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class VersionChecker
{
    public async Task<bool> Check()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Server").Document("Setting");

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        Dictionary<string, object> data = result.ToDictionary();

        bool isActive = (bool)data["IsActive"];
        bool isVersionCheckActive = (bool)data["IsVersionCheckActive"];

        string clientVersion = Application.version;
        string serverVersion = data["Version"].ToString();

        if (!isActive)
            return false;

        if (!isVersionCheckActive)
            return true;

        if (clientVersion != serverVersion) // todo 是不是要先檢查版本才檢查isVersionCheckActive
            return false;

        return false;
    }
}