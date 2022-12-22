using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Controller_Build : ControllerBehavior
{
    public GameObject buildEffect;
    public BuildTmpSensor buildTmpSensor;

    [Title("MoveBuild")] [SerializeField] private GameObject cantMoveDialog;
    [SerializeField] private GameObject removeMask;
    

    private Tweener jumpTween;

    public void Init()
    {
        BuildOrigin();
        buildTmpSensor.buildTmpPositionChange += BuildTmpPositionChange;
    }

    #region Basic

    public void Open()
    {
        App.view.build.Open();
        App.system.grid.OpenBuildMap();
        App.system.room.OpenExistRoomsSensor();

        App.model.build.IsBuilding = false;
        App.model.build.IsMoving = false;

        App.model.build.IsCanMoveOrRemove = true;
    }

    public void Close()
    {
        App.controller.lobby.Open();
        App.view.build.Close();

        App.system.grid.CloseBuildMap();
        App.system.room.CloseExistRoomsSensor();

        App.model.build.IsCanMoveOrRemove = false;
    }

    public void OpenChooseBuild()
    {
        App.view.build.Close();
        App.controller.chooseBuild.Open();
    }

    public void OpenChooseFloor()
    {
        App.view.build.Close();
        App.controller.chooseFloor.Open();
    }

    #endregion

    #region Building

    public void Build()
    {
        MyGridSystem myGridSystem = App.system.grid;
        RoomSizeType sizeType = App.model.build.SelectedRoom.roomData.roomSizeType;

        var position = App.view.build.buildTmp.transform.position;
        int[] gridIndex = myGridSystem.GetGridIndexByPosision(position.x, position.y);

        int roomWidth = MyTable.GetRoomWidth(sizeType);
        int roomHeight = MyTable.GetRoomHeight(sizeType);

        App.system.grid.Build(gridIndex[0], gridIndex[1], roomWidth, roomHeight,
            App.model.build.SelectedRoom.gameObject);

        #region Effect

        Vector3 effectPos = position;

        switch (sizeType)
        {
            case RoomSizeType.One_One:
                effectPos.x += 2.5f;
                buildEffect.transform.GetChild(0).localScale = new Vector3(2, 2, 2);
                buildEffect.transform.GetChild(1).localScale = new Vector3(2, 2, 2);
                buildEffect.transform.GetChild(2).localScale = new Vector3(2.5f, 2.5f, 2.5f);
                break;
            case RoomSizeType.Two_Two:
                effectPos.x += 5f;
                buildEffect.transform.GetChild(0).localScale = new Vector3(4, 4, 4);
                buildEffect.transform.GetChild(1).localScale = new Vector3(4, 4, 4);
                buildEffect.transform.GetChild(2).localScale = new Vector3(5f, 5f, 5f);
                break;
            case RoomSizeType.Three_Three:
                effectPos.x += 7.5f;
                buildEffect.transform.GetChild(0).localScale = new Vector3(6, 6, 6);
                buildEffect.transform.GetChild(1).localScale = new Vector3(6, 6, 6);
                buildEffect.transform.GetChild(2).localScale = new Vector3(7.5f, 7.5f, 7.5f);
                break;
        }

        GameObject effect = Instantiate(buildEffect, effectPos, Quaternion.identity);
        Destroy(effect, 3);

        #endregion

        App.model.build.SelectedRoom.Count--;

        if (App.model.build.IsMoving)
        {
            Destroy(App.model.chooseBuild.MoveBuildTmp);
            App.model.chooseBuild.MoveBuildTmp = null;

            App.model.build.IsMoving = false;
        }

        App.model.build.IsBuilding = false;
        App.model.build.CanBuild = false;

        App.controller.chooseBuild.ClearTmp();
        App.system.cat.OpenPolyNav2D();

        App.system.room.OpenExistRoomsSensor();
    }

    public void FirestoreBuild(string roomId, int x, int y)
    {
        Room room = App.factory.roomFactory.GetRoomById(roomId);

        RoomSizeType sizeType = room.roomData.roomSizeType;
        int roomWidth = MyTable.GetRoomWidth(sizeType);
        int roomHeight = MyTable.GetRoomHeight(sizeType);

        App.system.grid.Build(x, y, roomWidth, roomHeight, room.gameObject);
    }

    public void Remove()
    {
        Room room = App.model.build.SelectedRoom;
        RoomSizeType sizeTpye = room.roomData.roomSizeType;

        int roomWidth = MyTable.GetRoomWidth(sizeTpye);
        int roomHeight = MyTable.GetRoomHeight(sizeTpye);

        room.Count++;

        App.system.grid.Remove(room.x, room.y, roomWidth, roomHeight);
        App.system.room.Remove(room);

        CloseMoveBuild();
        App.system.cat.CheckCatNeedLeftRoom();
    }

    public void CancelBuilding()
    {
        App.model.build.IsBuilding = false;
        OpenChooseBuild();

        App.controller.chooseBuild.ClearTmp();
    }

    public void BuildTmpPositionChange(Vector3 tmp)
    {
        MyGridSystem myGridSystem = App.system.grid;
        RoomSizeType sizeType = App.model.build.SelectedRoom.roomData.roomSizeType;

        int[] gridIndex = myGridSystem.GetGridIndexByPosision(tmp.x, tmp.y);

        int roomWidth = MyTable.GetRoomWidth(sizeType);
        int roomHeight = MyTable.GetRoomHeight(sizeType);

        bool canBuild = false;

        for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomHeight; j++)
            {
                int x = gridIndex[0] + i;
                int y = gridIndex[1] + j;
                
                if (x >= myGridSystem.width)
                    continue;

                if (y >= myGridSystem.height)
                    continue;
                
                MyGrid myGrid = myGridSystem.GetGrid(x, y);

                if (myGrid.Value == 2)
                    canBuild = true;

                if (myGrid.Value == 1)
                {
                    App.model.build.CanBuild = false;
                    return;
                }
            }
        }

        App.model.build.CanBuild = canBuild;
    }

    private void BuildOrigin()
    {
        MyGridSystem myGridSystem = App.system.grid;

        int centerX = myGridSystem.width / 2;
        int centerY = myGridSystem.height / 2;

        App.system.grid.Build(centerX, centerY, 1, 1, App.factory.roomFactory.GetOriginRoom());
    }

    #endregion

    #region MoveBuild

    public void OpenMoveBuild(Room room)
    {
        if (CheckIsCenter(room))
            return;

        if (!App.model.build.IsCanMoveOrRemove)
            return;

        if (App.model.build.IsBuilding)
            return;

        if (!App.system.room.CheckMovePossibility(room))
            return;

        if (jumpTween != null && jumpTween.IsPlaying())
            return;
        
        var originY = room.transform.position.y;
        var offsetY = originY + 0.5f;
        jumpTween = room.transform.DOMoveY(offsetY, 0.1f).From(originY).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            jumpTween = null;
        });

        // 限制拆養育房
        int catsCount = App.system.cat.GetCats().Count;
        int featuresRoomCount = App.system.room.GetFeaturesRooms().Count;
        removeMask.SetActive(room.roomData.roomType == RoomType.Features && featuresRoomCount <= catsCount);
        cantMoveDialog.SetActive(room.roomData.roomType == RoomType.Features && featuresRoomCount <= catsCount);

        App.model.build.SelectedRoom = room;
        App.view.build.OpenMoveBuild();
        App.view.build.Close();
    }

    public void CloseMoveBuild()
    {
        App.model.build.SelectedRoom = null;
        App.view.build.CloseMoveBuild();
        App.view.build.Open();
    }

    private bool CheckIsCenter(Room room)
    {
        MyGridSystem myGridSystem = App.system.grid;
        int centerX = myGridSystem.width / 2;
        int centerY = myGridSystem.height / 2;

        if (room.x == centerX && room.y == centerY)
            return true;

        return false;
    }

    #endregion
}