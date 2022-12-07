using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item_CatchCat", menuName = "CatchGame/Create Item_CatchCat")]
public class Item_CatchCat : Item
{
    public int personality;
    public int level;
}
