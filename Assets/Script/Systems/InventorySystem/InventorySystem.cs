using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Searchable]
public class InventorySystem : SerializedMonoBehaviour
{
    public Dictionary<string, int> RoomData;
    public Dictionary<string, int> FoodData;
    public Dictionary<string, int> ToolData;
    public Dictionary<string, int> LitterData;
    public Dictionary<string, int> SkinData;
    public Dictionary<string, bool> itemsCanBuyAtStore = new Dictionary<string, bool>();
    
    public void Add(string id, int value)
    {
        for (int i = 0; i < RoomData.Count; i++) //RoomData
        {
            if (RoomData.ElementAt(i).Key == id)
            {
                RoomData[RoomData.ElementAt(i).Key] += value;
                return;
            }
        }

        for (int i = 0; i < FoodData.Count; i++) //FoodData
        {
            if (FoodData.ElementAt(i).Key == id)
            {
                FoodData[FoodData.ElementAt(i).Key] += value;
                return;
            }
        }

        for (int i = 0; i < ToolData.Count; i++) //ToolData
        {
            if (ToolData.ElementAt(i).Key == id)
            {
                ToolData[ToolData.ElementAt(i).Key] += value;
                return;
            }
        }

        for (int i = 0; i < LitterData.Count; i++) //LitterData
        {
            if (LitterData.ElementAt(i).Key == id)
            {
                LitterData[LitterData.ElementAt(i).Key] += value;
                return;
            }
        }
    }
}