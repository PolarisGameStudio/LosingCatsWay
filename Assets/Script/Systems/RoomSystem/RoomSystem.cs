using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSystem : MvcBehaviour
{
    private List<Room> myRooms = new List<Room>();

    public CallbackValue OnRoomsChange;

    public bool CheckMovePossibility(Room targetRoom)
    {
        List<Room> alreadyCheckRooms = new List<Room>();
        Queue<Room> readyCheckRooms = new Queue<Room>();

        readyCheckRooms.Enqueue(myRooms[0]);

        while (readyCheckRooms.Count != 0)
        {
            Room room = readyCheckRooms.Dequeue();
            List<Room> result = room.GetNearByRooms();

            alreadyCheckRooms.Add(room);

            for (int i = 0; i < result.Count; i++)
            {
                if (alreadyCheckRooms.Contains(result[i]))
                    continue;

                if (result[i] == targetRoom)
                    continue;

                if (readyCheckRooms.Contains(result[i]))
                    continue;

                readyCheckRooms.Enqueue(result[i]);
            }
        }

        return alreadyCheckRooms.Count == myRooms.Count - 1;
    }

    public void Add(Room room)
    {
        myRooms.Add(room);

        RefreshRoomsWall();

        OnRoomsChange?.Invoke(myRooms);
    }

    public void Remove(Room room)
    {
        RefreshRoomsWall();

        for (int i = 0; i < myRooms.Count; i++)
        {
            Room tmp = myRooms[i];

            if (tmp.x == room.x && tmp.y == room.y)
            {
                Destroy(tmp.gameObject);
                myRooms.RemoveAt(i);
                break;
            }
        }

        OnRoomsChange?.Invoke(myRooms);
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
        //todo 要加邊界防呆

        MyGridSystem myGridSystem = App.system.grid;

        int roomWidth = MyTable.GetRoomWidth(room.roomData.roomSizeType);
        int roomHeight = MyTable.GetRoomHeight(room.roomData.roomSizeType);

        //上偵查
        
        for (int i = 0; i < roomWidth; i++)
        {
            if (room.y == myGridSystem.height - 1)
                break;

            MyGrid upGrid = myGridSystem.GetUpGrid(room.x + i, room.y + (roomHeight - 1));
            
            if (upGrid == null)
                continue;

            if (upGrid.Value == 1)
                room.CloseUpWall(i);
            else
                room.OpenUpWall(i);
        }

        //下偵查

        for (int i = 0; i < roomWidth; i++)
        {
            if (room.y == 0)
                break;

            MyGrid downGrid = myGridSystem.GetDownGrid(room.x + i, room.y);
            
            if (downGrid == null)
                continue;

            if (downGrid.Value == 1)
                room.CloseDownWall(i);
            else
                room.OpenDownWall(i);
        }

        //左偵查
        
        for (int i = 0; i < roomHeight; i++)
        {
            if (room.x == 0)
                break;

            MyGrid leftGrid = myGridSystem.GetLeftGrid(room.x, room.y + i);
            
            if (leftGrid == null)
                continue;

            if (leftGrid.Value == 1)
                room.CloseLeftWall(i);
            else
                room.OpenLeftWall(i);
        }

        //右偵查

        for (int i = 0; i < roomHeight; i++)
        {
            if (room.x == myGridSystem.width - 1)
                break;

            if (room.x + (roomWidth - 1) + 1 >= myGridSystem.width - 1)
                break;

            MyGrid rightGrid = myGridSystem.GetRightGrid(room.x + (roomWidth - 1), room.y + i);
            
            if (rightGrid == null)
                continue;
            
            if (rightGrid.Value == 1)
                room.CloseRightWall(i);
            else
                room.OpenRightWall(i);
        }
    }

    public int GetRoomCount()
    {
        return myRooms.Count;
    }

    public List<Room> GetFeaturesRooms()
    {
        var result = new List<Room>();

        for (int i = 0; i < myRooms.Count; i++)
        {
            var room = myRooms[i];
            
            if (room.roomData.roomType == RoomType.Features)
                result.Add(room);
        }

        return result;
    }

    public Room GetRandomRoom()
    {
        return myRooms[Random.Range(0, myRooms.Count)];
    }

    public Room GetRandomSpecialSpineRoom()
    {
        var tmp = new List<Room>();

        for (int i = 0; i < myRooms.Count; i++)
        {
            if (myRooms[i].hasSpcialSpine && !myRooms[i].spcialSpineIsUse)
                tmp.Add(myRooms[i]);
        }

        if (tmp.Count == 0)
            return null;
        
        return tmp[Random.Range(0, tmp.Count)];
    }

    public Vector3 GetRandomRoomPosition()
    {
        Room room = myRooms[Random.Range(0, myRooms.Count)];
        return GetRoomCenterPosition(room);
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

    public List<Room> MyRooms => myRooms;

    public int FeatureRoomsCount
    {
        get
        {
            int count = 0;
            for (int i = 0; i < myRooms.Count; i++)
            {
                if (myRooms[i].roomData.roomType != RoomType.Features) continue;
                count++;
            }

            return count;
        }
    }

    #region ExistRooms

    public void OpenExistRoomsSensor()
    {
        for (int i = 0; i < myRooms.Count; i++)
            myRooms[i].sensor.enabled = true;
    }
    
    public void CloseExistRoomsSensor()
    {
        for (int i = 0; i < myRooms.Count; i++)
            myRooms[i].sensor.enabled = false;
    }

    #endregion

    #region EnableRooms (Performance)

    public void OpenRooms()
    {
        for (int i = 0; i < myRooms.Count; i++)
            if (!myRooms[i].gameObject.activeSelf) 
                myRooms[i].gameObject.SetActive(true);
    }

    public void CloseRooms()
    {
        for (int i = 0; i < myRooms.Count; i++)
            if (myRooms[i].gameObject.activeSelf) 
                myRooms[i].gameObject.SetActive(false);
    }

    #endregion
}