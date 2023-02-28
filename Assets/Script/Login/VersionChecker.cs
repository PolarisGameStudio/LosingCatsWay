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

        if (!isVersionCheckActive) // 要不要確認版本
            return true;

        if (clientVersion != serverVersion)
            return false;

        return false;
    }

    /// -1正常 0關伺服 1版本檢查 2版本不同
    public async Task<int> CheckStatus(bool debugMode)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        string docId = debugMode ? "DebugSetting" : "Setting";
        DocumentReference docRef = db.Collection("Server").Document(docId);
        
        if (debugMode)
            Debug.LogWarning("You are in debug mode.");

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        Dictionary<string, object> data = result.ToDictionary();

        bool isActive = (bool)data["IsActive"];
        bool isVersionCheckActive = (bool)data["IsVersionCheckActive"];

        string clientVersion = Application.version;
        string serverVersion = data["Version"].ToString();

        if (!isActive)
            return 0;
        
        Debug.LogWarning("Server is active");

        // if (!isVersionCheckActive) // 要不要確認版本
        //     return -1;
        
        if (isVersionCheckActive && clientVersion != serverVersion)
            return 1;

        Debug.LogWarning($"Version compare: Client: {clientVersion}, Server: {serverVersion}");

        return -1;
    }
}