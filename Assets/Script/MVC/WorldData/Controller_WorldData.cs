using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class Controller_WorldData : ControllerBehavior
{
    public void Open()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("WorldData").Document("Total");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
                App.model.worldData.WorldData = snapshot.ConvertTo<Cloud_WorldData>();
        });
        
        App.view.worldData.Open();
    }

    public void Close()
    {
        App.view.worldData.Close();
    }

    private async void CloudGetWorldData()
    {
        Cloud_WorldData worldData = new Cloud_WorldData();
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("WorldData").Document("Total");
        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        worldData = result.ConvertTo<Cloud_WorldData>();
        App.model.worldData.WorldData = worldData;
    }
}
