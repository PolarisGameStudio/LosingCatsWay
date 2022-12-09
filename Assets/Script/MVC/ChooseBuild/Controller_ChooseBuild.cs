using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Common;
using UnityEngine;

public class Controller_ChooseBuild : ControllerBehavior
{
    public Transform buildTmp;
    public Transform buildTmpCircle;

    public SpriteRenderer buildTmpMask;

    public LeanPlane leanPlane;
    public Camera mainCamera;

    public void Open()
    {
        App.view.chooseBuild.Open();
        
        DOVirtual.DelayedCall(0.25f, () =>
        {
            // SelectRoomBoughtType(0);
            SelectRoomSortType(0);
            SelectRoomType(0);
            
            RefreshSelectedRooms();
        });
    }

    public void Close()
    {
        App.view.build.Open();
        App.view.chooseBuild.Close();

        App.model.build.IsBuilding = false;
        App.system.soundEffect.Play("Button");
    }

    public void Select(int index)
    {
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        App.view.build.Open();
        App.view.chooseBuild.Close();
        App.system.room.CloseExistRoomsSensor();
        App.model.build.SelectedRoom = App.model.chooseBuild.SelectedRooms[index];

        SetBuildTmp();
        App.model.build.IsBuilding = true;

        App.system.cat.ClosePolyNav2D();
    }

    public void Move()
    {
        Room room = App.model.build.SelectedRoom;

        App.view.build.Open();
        App.view.chooseBuild.Close();

        GameObject tmp = Instantiate(room.gameObject, room.transform.parent);
        tmp.transform.position = room.transform.position;

        App.model.chooseBuild.MoveBuildTmp = tmp;
        App.controller.build.Remove();

        App.model.build.SelectedRoom = App.model.chooseBuild.MoveBuildTmp.GetComponent<Room>();

        App.model.build.IsBuilding = true;
        App.model.build.IsMoving = true;
        App.model.build.CanBuild = true;
        
        SetBuildTmp();
        App.system.cat.CheckCatNeedLeftRoom();
        App.system.cat.ClosePolyNav2D();
        
        App.system.room.CloseExistRoomsSensor();
    }

    #region Building

    // public void SelectRoomBoughtType(int index)
    // {
    //     if (App.model.chooseBuild.RoomBoughtType == index)
    //         return;
    //
    //     App.model.chooseBuild.RoomBoughtType = index;
    //     RefreshSelectedRooms();
    // }

    public void SelectRoomSortType(int type)
    {
        App.system.soundEffect.Play("Button");
        
        if (App.model.chooseBuild.RoomSortType == type)
            return;

        App.model.chooseBuild.RoomSortType = type;
        RefreshSelectedRooms();
    }

    public void SelectRoomType(int index)
    {
        App.system.soundEffect.Play("Button");
        
        if (App.model.chooseBuild.RoomType == index)
            return;

        App.model.chooseBuild.RoomType = index;
        RefreshSelectedRooms();
    }

    private void RefreshSelectedRooms()
    {
        int roomType = App.model.chooseBuild.RoomType;
        int roomSortType = App.model.chooseBuild.RoomSortType;
        App.model.chooseBuild.SelectedRooms = App.factory.roomFactory.GetRoomsBySort(roomSortType, roomType);
    }

    #endregion

    #region BuildTmp

    public void ClearTmp()
    {
        if (App.model.chooseBuild.MoveBuildTmpModel == null)
            return;

        Destroy(App.model.chooseBuild.MoveBuildTmpModel);
        App.model.chooseBuild.MoveBuildTmpModel = null;
    }

    private void SetBuildTmp()
    {
        SetBuildTmpPosition();
        SetBuildTmpContent();
        SetBuildTmpSize();
        SetBuildTmpLimit();
        SetBuildTmpCircleSize();
    }

    private void SetBuildTmpPosition()
    {
        if (App.model.build.IsMoving)
        {
            buildTmp.transform.position = App.model.chooseBuild.MoveBuildTmp.transform.position;
            App.model.chooseBuild.MoveBuildTmp.transform.position = new Vector3(0, 0);
            App.model.chooseBuild.MoveBuildTmp.GetComponent<Room>().OpenAllWall();
            return;
        }

        float gridSize = App.system.grid.cellSize;
        Vector3 centerPosition = mainCamera.transform.position;

        int x = (int) (centerPosition.x / gridSize);
        int y = (int) (centerPosition.y / gridSize);

        buildTmp.transform.position = new Vector3(x * gridSize, y * gridSize, 0);
    }

    private void SetBuildTmpContent()
    {
        RoomSizeType type = App.model.build.SelectedRoom.roomData.roomSizeType;
        Sprite spriteTmp = App.factory.roomFactory.GetBuildRoomBg(type);

        buildTmpMask.sprite = spriteTmp;

        // 導入預覽圖
        GameObject moveBuildTmpModel = Instantiate(App.model.build.SelectedRoom.gameObject, buildTmp);

        moveBuildTmpModel.GetComponent<Room>().enabled = false;
        moveBuildTmpModel.GetComponent<BoxCollider2D>().enabled = false;

        App.model.chooseBuild.MoveBuildTmpModel = moveBuildTmpModel;
    }

    private void SetBuildTmpSize()
    {
        RoomSizeType sizeType = App.model.build.SelectedRoom.roomData.roomSizeType;
        BoxCollider2D boxCollider2D = buildTmp.GetComponent<BoxCollider2D>();

        float gridSize = App.system.grid.cellSize;
        float gridOffset = gridSize * 0.5f;

        switch (sizeType)
        {
            case RoomSizeType.One_One:
                boxCollider2D.offset = new Vector2(gridOffset, gridOffset);
                boxCollider2D.size = new Vector2(gridSize, gridSize);
                break;
            case RoomSizeType.Two_Two:
                boxCollider2D.offset = new Vector2(gridOffset * 2, gridOffset * 2);
                boxCollider2D.size = new Vector2(gridSize * 2, gridSize * 2);
                break;
            case RoomSizeType.Three_Three:
                boxCollider2D.offset = new Vector2(gridOffset * 3, gridOffset * 3);
                boxCollider2D.size = new Vector2(gridSize * 3, gridSize * 3);
                break;
        }
    }

    private void SetBuildTmpCircleSize()
    {
        RoomSizeType sizeType = App.model.build.SelectedRoom.roomData.roomSizeType;

        float size = 1.8f;
        float cellSize = App.system.grid.cellSize;
        float postion = cellSize * 0.5f;

        switch (sizeType)
        {
            case RoomSizeType.Two_Two:
                size *= 2;
                postion *= 2;
                break;
            case RoomSizeType.Three_Three:
                size *= 3;
                postion *= 3;
                break;
        }

        buildTmpCircle.localScale = new Vector3(size, size, 1);
        buildTmpCircle.localPosition = new Vector3(postion, postion, 1);
    }

    private void SetBuildTmpLimit()
    {
        RoomSizeType sizeType = App.model.build.SelectedRoom.roomData.roomSizeType;

        int width = App.system.grid.width - 1;
        int height = App.system.grid.height - 1;
        float gridSize = App.system.grid.cellSize;

        switch (sizeType)
        {
            case RoomSizeType.Two_Two:
                width -= 1;
                height -= 1;
                break;
            case RoomSizeType.Three_Three:
                width -= 2;
                height -= 2;
                break;
        }

        leanPlane.MaxX = gridSize * width;
        leanPlane.MaxY = gridSize * height;
    }

    #endregion
}