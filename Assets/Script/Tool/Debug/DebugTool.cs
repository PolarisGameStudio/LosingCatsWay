using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
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

    [Button]
    public void Test()
    {
        List<int> r = new List<int>();
        
        r.Add(0);
        r.Add(1);
        r.Add(2);
        r.Add(3);
        
        print(r.FindIndex(x => x == 4));
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
}
