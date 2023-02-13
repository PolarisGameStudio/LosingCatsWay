using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FriendRoom_CatSystem : MonoBehaviour
{
    [Title("Require")] 
    public FactoryContainer factory;
    public Cat catObject;
    public Cat kittyObject;
    public Transform catsTransform;
    
    private FriendRoom_RoomSystem roomSystem;

    private void Awake()
    {
        roomSystem = GetComponent<FriendRoom_RoomSystem>();
    }

    public void CreateCat(List<CloudCatData> values)
    {
        for (int i = 0; i < values.Count; i++)
            CreateCatObject(values[i]);
    }
    
    public void CreateCatObject(CloudCatData cloudCatData)
    {
        Cat tmp = catObject;
        if (cloudCatData.CatData.CatAge <= 3)
            tmp = kittyObject;
        
        Cat cat = Instantiate(tmp, catsTransform);
        cat.SetCloudCatData(cloudCatData);
        cat.isFriendMode = true;
        
        Vector3 randomPostition = roomSystem.GetRandomRoomPosition();
        cat.transform.position = randomPostition;
        
        cat.Active();
    }
}
