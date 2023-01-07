using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Room : MvcBehaviour
{
    public RoomData roomData;
    [Space(20)] [ReadOnly] public int x;
    [ReadOnly] public int y;

    public GameObject[] upCoverWalls;

    public GameObject[] upDoorWalls;
    public GameObject[] downWalls;

    public GameObject[] leftWalls;
    public GameObject[] rightWalls;

    public BoxCollider2D sensor;

    [Title("SpecialSpine")] public bool hasSpcialSpine;

    [ShowIf("hasSpcialSpine")] public int spcialSpineType;
    [ShowIf("hasSpcialSpine")] public bool spcialSpineIsUse = false;
    [ShowIf("hasSpcialSpine")] public Transform spcialSpinePosition;

    [ShowIf("@hasSpcialSpine && spcialSpineType != 0")]
    public SkeletonAnimation[] specialSpines;

    [ShowIf("@hasSpcialSpine")]
    public SpriteRenderer[] specialSpineSpriteObjects;
    
    [ShowIf("@hasSpcialSpine")]
    public MeshRenderer[] specialSpineMeshObjects;
    #region GetSet

    public string Name
    {
        get { return App.factory.stringFactory.GetRoomName(roomData.id); }
    }

    public int Count
    {
        get { return App.system.inventory.RoomData[roomData.id]; }
        set { App.system.inventory.RoomData[roomData.id] = value; }
    }

    public Sprite Image
    {
        get { return App.factory.roomFactory.roomImages[roomData.id]; }
    }

    public int Height
    {
        get
        {
            int result = 1;

            switch (roomData.roomSizeType)
            {
                case RoomSizeType.Two_Two:
                    result = 2;
                    break;
                case RoomSizeType.Three_Three:
                    result = 3;
                    break;
            }

            return result;
        }
    }

    public int Width
    {
        get
        {
            int result = 1;

            switch (roomData.roomSizeType)
            {
                case RoomSizeType.Two_Two:
                    result = 2;
                    break;
                case RoomSizeType.Three_Three:
                    result = 3;
                    break;
            }

            return result;
        }
    }

    #endregion

    private int endCount;

    public void OpenMoveBuild()
    {
        App.controller.build.OpenMoveBuild(this);
    }

    public void PlaySpecialSpine()
    {
        endCount = 0;

        switch (spcialSpineType)
        {
            case 1:
                for (int i = 0; i < specialSpines.Length; i++)
                {
                    var skeletonAnimationName = specialSpines[i].AnimationName;
                    specialSpines[i].state.SetAnimation(0, skeletonAnimationName, false);
                    specialSpines[i].timeScale = 1;
                    specialSpines[i].state.Complete += OnSpecialSpineEnd;
                }
                break;
        }

        for (int i = 0; i < specialSpineSpriteObjects.Length; i++)
            specialSpineSpriteObjects[i].sortingOrder = -2;
        
        for (int i = 0; i < specialSpineMeshObjects.Length; i++)
            specialSpineMeshObjects[i].sortingOrder = -2;
    }

    private void OnSpecialSpineEnd(TrackEntry trackEntry)
    {
        endCount++;

        if (endCount == specialSpines.Length)
        {
            for (int i = 0; i < specialSpines.Length; i++)
            {
                specialSpines[i].state.Complete -= OnSpecialSpineEnd;

                var skeletonAnimationName = specialSpines[i].AnimationName;
                specialSpines[i].state.SetAnimation(0, skeletonAnimationName, false);
                specialSpines[i].timeScale = 0;
            }
            
            for (int i = 0; i < specialSpineSpriteObjects.Length; i++)
                specialSpineSpriteObjects[i].sortingOrder = 0;
            
            for (int i = 0; i < specialSpineMeshObjects.Length; i++)
                specialSpineMeshObjects[i].sortingOrder = 0;
        }
    }

    #region Basic

    public void OpenAllWall()
    {
        for (int i = 0; i < Height; i++)
        {
            OpenUpWall(i);
            OpenDownWall(i);
        }

        for (int i = 0; i < Width; i++)
        {
            OpenLeftWall(i);
            OpenRightWall(i);
        }
    }

    public void OpenUpWall(int index)
    {
        upCoverWalls[index].SetActive(true);

        if (roomData.roomType != RoomType.Path)
            upDoorWalls[index].SetActive(false);
    }

    public void CloseUpWall(int index)
    {
        upCoverWalls[index].SetActive(false);

        if (roomData.roomType != RoomType.Path)
            upDoorWalls[index].SetActive(true);
    }

    public void OpenDownWall(int index)
    {
        downWalls[index].SetActive(true);
    }

    public void CloseDownWall(int index)
    {
        downWalls[index].SetActive(false);
    }

    public void OpenLeftWall(int index)
    {
        if (index >= leftWalls.Length)
            return;
        if (leftWalls[index] == null)
            return;
        leftWalls[index].SetActive(true);
    }

    public void CloseLeftWall(int index)
    {
        if (index >= leftWalls.Length)
            return;
        if (leftWalls[index] == null)
            return;
        leftWalls[index].SetActive(false);
    }

    public void OpenRightWall(int index)
    {
        if (index >= rightWalls.Length)
            return;
        if (rightWalls[index] == null)
            return;
        rightWalls[index].SetActive(true);
    }

    public void CloseRightWall(int index)
    {
        if (index >= rightWalls.Length)
            return;
        if (rightWalls[index] == null)
            return;
        rightWalls[index].SetActive(false);
    }


    public List<Room> GetNearByRooms()
    {
        List<Room> result = new List<Room>();
        MyGridSystem myGridSystem = App.system.grid;

        int width = MyTable.GetRoomWidth(roomData.roomSizeType);
        int height = MyTable.GetRoomHeight(roomData.roomSizeType);

        //上&下
        for (int i = 0; i < width; i++)
        {
            MyGrid upGrid = myGridSystem.GetUpGrid(x + i, y + height - 1);
            MyGrid downGrid = myGridSystem.GetDownGrid(x + i, y);

            if (upGrid == null)
                continue;

            if (downGrid == null)
                continue;

            if (upGrid.Value == 1)
            {
                Room upRoom = upGrid.Content.GetComponent<Room>();
                if (!result.Contains(upRoom))
                    result.Add(upRoom);
            }

            if (downGrid.Value == 1)
            {
                Room downRoom = downGrid.Content.GetComponent<Room>();
                if (!result.Contains(downRoom))
                    result.Add(downRoom);
            }
        }

        //左&右
        for (int i = 0; i < height; i++)
        {
            MyGrid leftGrid = myGridSystem.GetLeftGrid(x, y + i);
            MyGrid rightGrid = myGridSystem.GetRightGrid(x + width - 1, y + i);

            if (leftGrid == null)
                continue;

            if (rightGrid == null)
                continue;

            if (leftGrid.Value == 1)
            {
                Room leftRoom = leftGrid.Content.GetComponent<Room>();
                if (!result.Contains(leftRoom))
                    result.Add(leftRoom);
            }

            if (rightGrid.Value == 1)
            {
                Room rightRoom = rightGrid.Content.GetComponent<Room>();
                if (!result.Contains(rightRoom))
                    result.Add(rightRoom);
            }
        }

        return result;
    }

    #endregion
}