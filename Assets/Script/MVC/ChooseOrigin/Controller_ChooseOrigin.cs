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
        App.model.chooseOrigin.UsingRoomObject = App.system.room.MyRooms[0].gameObject;
        App.view.chooseOrigin.Open();

        DOVirtual.DelayedCall(0.25f, () =>
        {
            Select(App.model.chooseOrigin.UsingRoomIndex);
        });
    }

    public void Close()
    {
        App.view.chooseOrigin.Close();
        App.controller.build.Open();

        App.model.chooseOrigin.UsingRoomObject.SetActive(true);
        if (App.model.chooseOrigin.PreviewRoomObject == null)
            return;
        ClearPreviewRoomObject();
    }

    public void Select(int index)
    {
        if (index == App.model.chooseOrigin.PreviewRoomIndex)
            return;

        Room selectedRoom = App.model.chooseOrigin.CenterRooms[index];
        Item roomItem = App.factory.itemFactory.GetItem(selectedRoom.roomData.id);

        if (!roomItem.CanBuyAtStore)
            return;

        App.model.chooseOrigin.UsingRoomObject.SetActive(false);
        ClearPreviewRoomObject();
        
        var tmp = Instantiate(selectedRoom, viewMap);
        tmp.transform.position = App.model.chooseOrigin.UsingRoomObject.transform.position;
        
        App.model.chooseOrigin.PreviewRoomObject = tmp.gameObject;
        App.model.chooseOrigin.PreviewRoomIndex = index;
    }

    private void ClearPreviewRoomObject()
    {
        if (App.model.chooseOrigin.PreviewRoomObject == null)
            return;
        
        Destroy(App.model.chooseOrigin.PreviewRoomObject);
        App.model.chooseOrigin.PreviewRoomObject = null;
    }

    private void ClearUsingRoomObject()
    {
        if (App.model.chooseOrigin.UsingRoomObject == null)
            return;
        
        Destroy(App.model.chooseOrigin.UsingRoomObject);
        App.model.chooseOrigin.UsingRoomObject = null;
    }

    public void ChangePreviewToUsing()
    {
        App.model.chooseOrigin.UsingRoomIndex = App.model.chooseOrigin.PreviewRoomIndex;
        ClearUsingRoomObject();
        App.model.chooseOrigin.UsingRoomObject = App.model.chooseOrigin.PreviewRoomObject;
        App.system.room.MyRooms[0] = App.model.chooseOrigin.UsingRoomObject.GetComponent<Room>();
        ClearPreviewRoomObject();
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
