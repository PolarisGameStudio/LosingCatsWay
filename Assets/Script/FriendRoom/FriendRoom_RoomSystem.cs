using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class FriendRoom_RoomSystem : MonoBehaviour
{
    [Title("Require")] 
    public FactoryContainer factory;
    public List<Room> myRooms = new List<Room>();
    
    private FriendRoom_GridSystem gridSystem;
    
    private void Awake()
    {
        gridSystem = GetComponent<FriendRoom_GridSystem>();
    }

    public void CreateRoom(List<CloudSave_RoomData> roomDatas)
    {
        // BuildOrigin();
        
        for (int i = 0; i < roomDatas.Count; i++)
        {
            CloudSave_RoomData roomData = roomDatas[i];
            Build(roomData.Id, roomData.X,roomData.Y);
        }
    }
    
    public Vector3 GetRandomRoomPosition()
    {
        Room room = myRooms[Random.Range(0, myRooms.Count)];
        return GetRoomCenterPosition(room);
    }
    
    public int GetRoomCount()
    {
        return myRooms.Count;
    }
    
    public Room GetRandomRoom()
    {
        return myRooms[Random.Range(0, myRooms.Count)];
    }
    
    private Vector3 GetRoomCenterPosition(Room room)
    {
        Vector3 result = new Vector3();
        result = room.transform.position;
        
        switch (room.roomData.roomSizeType)
        {
            case RoomSizeType.One_One:
                result += Vector3.one * 2.56f;
                break;
            case RoomSizeType.Two_Two:
                result += Vector3.one * 5.12f;
                break;
            case RoomSizeType.Three_Three:
                result += Vector3.one * 7.68f;
                break;
        }

        result.z = 0;

        return result;
    }
    
    private void Build(string roomId, int x, int y)
    {
        Room room = factory.roomFactory.GetRoomById(roomId);

        RoomSizeType sizeType = room.roomData.roomSizeType;
        int roomWidth = MyTable.GetRoomWidth(sizeType);
        int roomHeight = MyTable.GetRoomHeight(sizeType);

        room.transform.GetComponent<LeanSelectableByFinger>().enabled = false;
        
        gridSystem.Build(x, y, roomWidth, roomHeight, room.gameObject);
        
        RefreshRoomsWall();
    }

    private void RefreshRoomsWall()
    {
        for (int i = 0; i < myRooms.Count; i++)
        {
            RefreshRoomWall(myRooms[i]);
        }
    }
    private void RefreshRoomWall(Room room)
    {
        //todo ??????????????????

        int roomWidth = MyTable.GetRoomWidth(room.roomData.roomSizeType);
        int roomHeight = MyTable.GetRoomHeight(room.roomData.roomSizeType);

        //?????????
        
        for (int i = 0; i < roomWidth; i++)
        {
            if (room.y == gridSystem.height - 1)
                break;

            MyGrid upGrid = gridSystem.GetUpGrid(room.x + i, room.y + (roomHeight - 1));
            
            if (upGrid == null)
                continue;

            if (upGrid.Value == 1)
                room.CloseUpWall(i);
            else
                room.OpenUpWall(i);
        }

        //?????????

        for (int i = 0; i < roomWidth; i++)
        {
            if (room.y == 0)
                break;

            MyGrid downGrid = gridSystem.GetDownGrid(room.x + i, room.y);
            
            if (downGrid == null)
                continue;

            if (downGrid.Value == 1)
                room.CloseDownWall(i);
            else
                room.OpenDownWall(i);
        }

        if (room.roomData.roomType != RoomType.Path)
            return;
        
        //?????????
        
        for (int i = 0; i < roomHeight; i++)
        {
            if (room.x == 0)
                break;

            MyGrid leftGrid = gridSystem.GetLeftGrid(room.x, room.y + i);
            
            if (leftGrid == null)
                continue;

            if (leftGrid.Value == 1)
                room.CloseLeftWall(i);
            else
                room.OpenLeftWall(i);
        }

        //?????????

        for (int i = 0; i < roomHeight; i++)
        {
            if (room.x == gridSystem.width - 1)
                break;

            if (room.x + (roomWidth - 1) + 1 >= gridSystem.width - 1)
                break;

            MyGrid rightGrid = gridSystem.GetRightGrid(room.x + (roomWidth - 1), room.y + i);
            
            if (rightGrid == null)
                continue;
            
            if (rightGrid.Value == 1)
                room.CloseRightWall(i);
            else
                room.OpenRightWall(i);
        }
    }
}