using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using Firebase.Firestore;
using I2.Loc;
using UnityEngine.SceneManagement;

public class DebugTool : MvcBehaviour
{
    DebugTool_Cat cat = new DebugTool_Cat();

    [Button]
    public void CreateAdultCatAtShelter()
    {
        cat.CreateCat("Shelter", true);
    }

    [Button]
    public void CreateAdultCatAtMap()
    {
        int index = Random.Range(0, 2);
        cat.CreateCat($"Location{index}", true);
    }
    
    [Button]
    public void CreateChildCatAtShelter()
    {
        cat.CreateCat("Shelter", false);
    }

    [Button]
    public void CreateChildCatAtMap()
    {
        int index = Random.Range(0, 2);
        cat.CreateCat($"Location{index}", false);
    }

    public void GetMonthLastTime()
    {
        DateTime notTime = DateTime.Now;
        DateTime lastDay = notTime.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).AddHours(-notTime.Hour + 23).AddMinutes(-notTime.Minute + 59).AddSeconds(-notTime.Second + 59);
        print(lastDay);
    }

    [Button]
    public void LoadScene()
    {
        App.system.transition.OnlyOpen(() =>
        {
            PlayerPrefs.SetString("FriendRoomId", "JUSTBUILD");
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        });
    }

    [Button]
    private void DebugAddExp()
    {
        App.system.player.AddExp(200);
        App.controller.lobby.ActiveBuffer();
    }
    
    [Button]
    public async void Test()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Test").Document("Test");

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        int value = Convert.ToInt32(result.ToDictionary()["Value"]);
        Dictionary<string, object> datas = new Dictionary<string, object>
        {
            { "Value", value + 1 }
        };
        await docRef.UpdateAsync(datas);
    }
}
