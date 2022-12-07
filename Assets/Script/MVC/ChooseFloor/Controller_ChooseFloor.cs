using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Controller_ChooseFloor : ControllerBehavior
{
    public void Open()
    {
        App.system.room.CloseExistRoomsSensor();

        App.view.chooseFloor.Open();
        
        // todo 存檔做這個可以刪掉
        App.model.chooseFloor.UsingFloorIndex = 0;
    }

    public void Close()
    {
        App.view.build.Open();
        App.view.chooseFloor.Close();

        App.system.room.OpenExistRoomsSensor();
    }

    public void Select(int index)
    {
        if (index == App.model.chooseFloor.UsingFloorIndex)
            return;
        
        App.model.chooseFloor.UsingFloorIndex = index;
    }
}

