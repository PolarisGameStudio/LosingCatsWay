using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_ChooseBuild : ModelBehavior
{
    private List<Room> selectedRooms;
    private GameObject moveBuildTmp;
    private GameObject moveBuildTmpModel;

    private int roomRoomType = -1;
    //private int roomBoughtType = -1;
    private int roomSortType = -1;

    public int RoomType
    {
        get => roomRoomType;
        set
        {
            RoomTypeChange?.Invoke(value);
            roomRoomType = value;
        }
    }

    // public int RoomBoughtType
    // {
    //     get => roomBoughtType;
    //     set
    //     {
    //         BoughtRoomTypeChange?.Invoke(value);
    //         roomBoughtType = value;
    //     }
    // }

    public List<Room> SelectedRooms
    {
        get => selectedRooms;
        set
        {
            SelectedRoomsChange?.Invoke(value);
            selectedRooms = value;
        }
    }

    public GameObject MoveBuildTmp
    {
        get => moveBuildTmp;
        set => moveBuildTmp = value;
    }

    public GameObject MoveBuildTmpModel
    {
        get => moveBuildTmpModel;
        set => moveBuildTmpModel = value;
    }

    public int RoomSortType
    {
        get => roomSortType;
        set
        {
            roomSortType = value;
            OnRoomSortTypeChange(value);
        }
    }

    public ValueChange SelectedRoomsChange;
    public ValueChange RoomTypeChange;
    //public ValueChange BoughtRoomTypeChange;
    public ValueChange OnRoomSortTypeChange;
}