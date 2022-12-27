using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorContainer", menuName = "Factory/Create FloorContainer")]
public class FloorContainer : SerializedScriptableObject
{
    [Title("Sprite","Floor")]
    public Sprite[] floorSprites;

    [Title("Object", "Floor")] 
    public GameObject[] floorType1; 
    public GameObject[] floorType2; 
    public GameObject[] floorType3; 

    [Title("Object","OutSide")]
    public Dictionary<string, GameObject> outsides;

    public GameObject[] outSides_Left;
    public GameObject[] outSides_Right;

    public GameObject[] outSides_Trees;

    public Sprite GetSprite(int index = 0)
    {
        return floorSprites[index];
    }

    public Sprite GetRandomSprite()
    {
        return floorSprites[Random.Range(0, floorSprites.Length)];
    }
    
    public enum OutsideEnum
    {
        CenterUp,
        CenterDown,
        CenterLeft,
        CenterRight,
        UpRight1,
        UpRight2,
        UpRight3,
        UpLeft1,
        UpLeft2,
        UpLeft3,
        DownRight1,
        DownRight2,
        DownRight3,
        DownLeft1,
        DownLeft2,
        DownLeft3,
        FloorUp,
        FloorDown,
        FloorLeft,
        FloorRight,
    }
}
