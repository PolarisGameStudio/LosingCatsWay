using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFactory : SerializedMonoBehaviour
{
    [Title("Rooms")] [SerializeField] private List<Room> rooms;
    public Dictionary<string, Sprite> roomImages;

    [SerializeField] private Sprite[] buildRoomBgs;
    [SerializeField] private GameObject lineRoom;
    [SerializeField] private GameObject originRoom;

    public string originRoomId;

    [Title("Floors")]
    ///用容器讓之後新增地板系列更方便
    public Dictionary<string, FloorContainer> floorContainers;

    public List<Room> GetRooms(int boughtType, int type)
    {
        List<Room> tmp = new List<Room>();

        for (int i = 0; i < rooms.Count; i++)
        {
            Room room = rooms[i];

            if (boughtType != 0)
                if (boughtType != (int) room.roomData.roomBoughtType)
                    continue;

            if (type == (int) room.roomData.roomType)
                tmp.Add(room);
        }

        return tmp;
    }

    public Room GetRoom(Room room)
    {
        Room result = rooms.Find(x => x.roomData.id == room.roomData.id);
        return result;
    }

    //20220406
    public Room GetRoomById(string id)
    {
        Room result = rooms.Find(x => x.roomData.id == id);
        return result;
    }

    public GameObject GetLineRoom()
    {
        return lineRoom;
    }

    public GameObject GetOriginRoom()
    {
        return originRoom;
    }

    public Sprite GetBuildRoomBg(RoomSizeType type)
    {
        int index = (int) type;
        return buildRoomBgs[index];
    }

    #region Floors
    
    public GameObject GetFloorObject(string floorKey)
    {
        float randomValue = Random.value;

        float type1Value = 0.86f;
        float type2Value = 0.98f;
        float type3Value = 1.0f;

        var floors = floorContainers[floorKey].floorType1;

        if (randomValue > type1Value && randomValue <= type2Value)
            floors = floorContainers[floorKey].floorType2;

        if (randomValue > type2Value && randomValue <= type3Value)
            floors = floorContainers[floorKey].floorType3;

        return floors[Random.Range(0, floors.Length)];
    }

    public Sprite GetFloorByKey(string floorKey)
    {
        return floorContainers[floorKey].GetRandomSprite();
    }

    #endregion

    [Serializable]
    public class Style
    {
        public GameObject[] oneXone;
        public GameObject[] twoXtwo;
        public GameObject[] threeXthree;
    }
}