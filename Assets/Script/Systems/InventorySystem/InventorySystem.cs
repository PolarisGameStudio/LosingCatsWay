using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Searchable]
public class InventorySystem : SerializedMonoBehaviour
{
    public Dictionary<string, int> CommonData;
    public Dictionary<string, int> RoomData;
    public Dictionary<string, int> FoodData;
    public Dictionary<string, int> ToolData;
    public Dictionary<string, int> LitterData;
    public Dictionary<string, int> SkinData;

    [Title("Player")] 
    public Dictionary<string, int> PlayerIconData;
    public Dictionary<string, int> PlayerAvatarData;
}