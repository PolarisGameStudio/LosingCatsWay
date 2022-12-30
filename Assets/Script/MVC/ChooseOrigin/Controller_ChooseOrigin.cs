using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Controller_ChooseOrigin : ControllerBehavior
{
    [SerializeField] private Transform viewMap;

    public void Open()
    {
        RefreshCenterRooms();
        
        App.view.chooseOrigin.Open();

        DOVirtual.DelayedCall(0.25f, () =>
        {
            App.system.room.MyRooms[0].gameObject.SetActive(false);
            Select(App.model.chooseOrigin.UsingRoomIndex);
        });
    }

    public void Close()
    {
        App.view.chooseOrigin.Close();
        App.controller.build.Open();

        App.system.room.MyRooms[0].gameObject.SetActive(true);
        
        if (App.model.chooseOrigin.PreviewRoom == null)
            return;
        ClearPreviewRoom();
    }

    public void Select(int index)
    {
        Room selectedRoom = App.model.chooseOrigin.CenterRooms[index];
        Item roomItem = App.factory.itemFactory.GetItem(selectedRoom.roomData.id);

        if (!roomItem.CanBuyAtStore)
            return;
        
        ClearPreviewRoom();
        
        var tmp = Instantiate(selectedRoom, viewMap);
        tmp.transform.position = App.system.room.MyRooms[0].transform.position;
        
        App.model.chooseOrigin.PreviewRoom = tmp;
        App.model.chooseOrigin.PreviewRoomIndex = index;
    }

    private void ClearPreviewRoom()
    {
        if (App.model.chooseOrigin.PreviewRoom == null)
            return;
        Destroy(App.model.chooseOrigin.PreviewRoom.gameObject);
        App.model.chooseOrigin.PreviewRoom = null;
    }

    public void ChangePreviewToUsing()
    {
        App.model.chooseOrigin.UsingRoomIndex = App.model.chooseOrigin.PreviewRoomIndex;

        Room room = App.system.room.MyRooms[0];
        RoomSizeType sizeTpye = room.roomData.roomSizeType;
        
        int roomWidth = MyTable.GetRoomWidth(sizeTpye);
        int roomHeight = MyTable.GetRoomHeight(sizeTpye);
        
        App.system.grid.Remove(room.x, room.y, roomWidth, roomHeight);
        App.system.room.Remove(room);
        
        int centerX = App.system.grid.width / 2;
        int centerY = App.system.grid.height / 2;
        App.system.grid.BuildOrigin(centerX, centerY, 1, 1, App.model.chooseOrigin.PreviewRoom.gameObject);
        
        App.system.map.GenerateMap();
        
        ClearPreviewRoom();
        Close();
    }

    private void RefreshCenterRooms()
    {
        var tmp = App.factory.roomFactory.GetCenterRooms();
        List<Room> result = new List<Room>();
        
        for (int i = 0; i < tmp.Count; i++)
        {
            Item item = App.factory.itemFactory.GetItem(tmp[i].roomData.id);
            if (!item.CanBuyAtStore)
                continue;
            result.Add(tmp[i]);
        }

        App.model.chooseOrigin.CenterRooms = result;
    }
}
