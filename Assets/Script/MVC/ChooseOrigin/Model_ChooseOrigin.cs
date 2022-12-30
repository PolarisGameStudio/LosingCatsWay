using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_ChooseOrigin : ModelBehavior
{
    private List<Room> centerRooms;
    private int usingRoomIndex = 0;
    private int previewRoomIndex = -1;
    private GameObject usingRoomObject;
    private GameObject previewRoomObject;

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

    public GameObject UsingRoomObject
    {
        get => usingRoomObject;
        set => usingRoomObject = value;
    }

    public GameObject PreviewRoomObject
    {
        get => previewRoomObject;
        set => previewRoomObject = value;
    }

    public ValueChange OnCenterRoomsChange;
    public ValueChange OnUsingRoomIndexChange;
    public ValueFromToChange OnPreviewRoomIndexChange;
}
