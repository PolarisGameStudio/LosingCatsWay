using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_ChooseOrigin : ModelBehavior
{
    private List<Room> centerRooms;
    private int usingRoomIndex = 0;
    private int previewRoomIndex = -1;
    private Room previewRoom;
    private bool isChooseOrigin;

    public List<Room> CenterRooms
    {
        get => centerRooms;
        set
        {
            centerRooms = value;
            OnCenterRoomsChange(value);
        }
    }

    public int UsingRoomIndex
    {
        get => usingRoomIndex;
        set
        {
            usingRoomIndex = value;
            OnUsingRoomIndexChange(value);
        }
    }

    public int PreviewRoomIndex
    {
        get => previewRoomIndex;
        set
        {
            OnPreviewRoomIndexChange(previewRoomIndex, value);
            previewRoomIndex = value;
        }
    }

    public Room PreviewRoom
    {
        get => previewRoom;
        set => previewRoom = value;
    }

    public bool IsChooseOrigin
    {
        get => isChooseOrigin;
        set => isChooseOrigin = value;
    }

    public ValueChange OnCenterRoomsChange;
    public ValueChange OnUsingRoomIndexChange;
    public ValueFromToChange OnPreviewRoomIndexChange;
}
