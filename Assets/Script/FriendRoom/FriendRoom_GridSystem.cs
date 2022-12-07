using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

public class FriendRoom_GridSystem : MonoBehaviour
{
    [Title("Require")] 
    public FactoryContainer factory;

    public Transform viewMap;
    private MyGrid[,] viewGridArray;

    public int width;
    public int height;
    public float cellSize;

    private FriendRoom_RoomSystem roomSystem;
    
    public void Init(int widthValue, int heightValue, float cellSizeValue)
    {
        width = widthValue;
        height = heightValue;
        cellSize = cellSizeValue;

        roomSystem = GetComponent<FriendRoom_RoomSystem>();
    }

    public void CreateGrid()
    {
        viewGridArray = new MyGrid[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var floor = factory.roomFactory.GetFloorObject("Spring");

                GameObject viewBuffer = Instantiate(floor, viewMap);
                viewBuffer.transform.position = GetWorldPosition(i, j);

                MyGrid myGrid = new MyGrid(0, viewBuffer);
                viewGridArray[i, j] = myGrid;
            }
        }

        SetCameraToOrigin();
        CreateOutSide();
    }

    public void Build(int x, int y, int roomWidth, int roomHeight, GameObject content)
    {
        GameObject tmp = Instantiate(content, viewMap);

        for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomHeight; j++)
            {
                MyGrid myGrid = GetGrid(x + i, y + j);
                myGrid.Value = 1;

                Destroy(myGrid.Content);
                myGrid.Content = null;

                myGrid.Content = tmp;
            }
        }

        Room room = tmp.GetComponent<Room>();
        room.x = x;
        room.y = y;

        tmp.transform.position = GetWorldPosition(x, y);
        RefreshGirdValues();

        roomSystem.myRooms.Add(tmp.GetComponent<Room>());
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private void SetCameraToOrigin(bool tween = false)
    {
        float x = width / 2 * cellSize + (cellSize / 2);
        float y = height / 2 * cellSize + (cellSize / 2);

        Camera cam = Camera.main;

        if (tween)
        {
            float zoom = cam.orthographicSize;
            DOTween.To(() => zoom, x => zoom = x, 10, 0.5f).OnUpdate(() => { cam.orthographicSize = zoom; });
        }
        else
        {
            cam.transform.position = new Vector3(x, y, -0.5f);
            cam.orthographicSize = 10;
        }

        LeanPinchCamera pinch = cam.GetComponent<LeanPinchCamera>();
        if (pinch != null) pinch.Zoom = 10;
    }

    private void RefreshGirdValues()
    {
        for (int i = 0; i < width; i++)
        for (int j = 0; j < height; j++)
            if (GetGrid(i, j).Value == 2)
                GetGrid(i, j).Value = 0;


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int value = GetGrid(i, j).Value;
                if (value == 0 || value == 2)
                    continue;

                MyGrid upGrid = GetUpGrid(i, j);
                MyGrid downGrid = GetDownGrid(i, j);
                MyGrid leftGrid = GetLeftGrid(i, j);
                MyGrid rightGrid = GetRightGrid(i, j);

                if (upGrid != null && upGrid.Value == 0)
                    upGrid.Value = 2;

                if (downGrid != null && downGrid.Value == 0)
                    downGrid.Value = 2;

                if (leftGrid != null && leftGrid.Value == 0)
                    leftGrid.Value = 2;

                if (rightGrid != null && rightGrid.Value == 0)
                    rightGrid.Value = 2;
            }
        }
    }

    public MyGrid GetGrid(int x, int y)
    {
        return viewGridArray[x, y];
    }

    public MyGrid GetUpGrid(int x, int y)
    {
        if (y == height - 1)
            return null;

        return viewGridArray[x, y + 1];
    }

    public MyGrid GetDownGrid(int x, int y)
    {
        if (y == 0)
            return null;

        return viewGridArray[x, y - 1];
    }

    public MyGrid GetLeftGrid(int x, int y)
    {
        if (x == 0)
            return null;

        return viewGridArray[x - 1, y];
    }

    public MyGrid GetRightGrid(int x, int y)
    {
        if (x == width - 1)
            return null;

        return viewGridArray[x + 1, y];
    }

    #region OutSide

    private void CreateOutSide()
    {
        CreateOutSideCenter();
        CreateOutSideCorner();
        CreateOutSideWall();
    }

    private void CreateOutSideCenter()
    {
        var floorFactory = factory.roomFactory.floorContainers["Spring"];

        GameObject up = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.CenterUp], viewMap);
        up.transform.position = GetWorldPosition(width / 2, height);

        GameObject down = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.CenterDown], viewMap);
        down.transform.position = GetWorldPosition(width / 2, -2);

        GameObject left = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.CenterLeft], viewMap);
        left.transform.position = GetWorldPosition(-2, height / 2);

        GameObject right = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.CenterRight], viewMap);
        right.transform.position = GetWorldPosition(width, height / 2);
    }

    private void CreateOutSideCorner()
    {
        var floorFactory = factory.roomFactory.floorContainers["Spring"];

        // UpRight

        GameObject upRight1 = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.UpRight1], viewMap);
        upRight1.transform.position = GetWorldPosition(width, height);

        GameObject upRight2 = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.UpRight2], viewMap);
        upRight2.transform.position = GetWorldPosition(width - 2, height);

        GameObject upRight3 = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.UpRight3], viewMap);
        upRight3.transform.position = GetWorldPosition(width, height - 2);

        // UpLeft

        GameObject upLeft1 = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.UpLeft1], viewMap);
        upLeft1.transform.position = GetWorldPosition(-2, height);

        GameObject upLeft2 = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.UpLeft2], viewMap);
        upLeft2.transform.position = GetWorldPosition(0, height);

        GameObject upLeft3 = Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.UpLeft3], viewMap);
        upLeft3.transform.position = GetWorldPosition(-2, height - 2);

        // DownRight

        GameObject downRight1 =
            Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.DownRight1], viewMap);
        downRight1.transform.position = GetWorldPosition(width, -2);

        GameObject downRight2 =
            Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.DownRight2], viewMap);
        downRight2.transform.position = GetWorldPosition(width - 2, -2);

        GameObject downRight3 =
            Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.DownRight3], viewMap);
        downRight3.transform.position = GetWorldPosition(width, 0);

        // DownLeft

        GameObject downLeft1 =
            Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.DownLeft1], viewMap);
        downLeft1.transform.position = GetWorldPosition(-2, -2);

        GameObject downLeft2 =
            Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.DownLeft2], viewMap);
        downLeft2.transform.position = GetWorldPosition(0, -2);

        GameObject downLeft3 =
            Instantiate(floorFactory.outSides[FloorContainer.OutsideEnum.DownLeft3], viewMap);
        downLeft3.transform.position = GetWorldPosition(-2, 0);
    }

    private void CreateOutSideWall()
    {
        var floorFactory = factory.roomFactory.floorContainers["Spring"];

        // Up & Down

        int center = 15;
        int x = width / 2;
        int y = height / 2;

        var upWall = floorFactory.outSides[FloorContainer.OutsideEnum.FloorUp];
        var downWall = floorFactory.outSides[FloorContainer.OutsideEnum.FloorDown];
        var leftWall = floorFactory.outSides[FloorContainer.OutsideEnum.FloorLeft];
        var rightWall = floorFactory.outSides[FloorContainer.OutsideEnum.FloorRight];

        for (int i = center - x + 2; i <= center + x - 2; i++)
        {
            if (i == center)
                continue;

            GameObject up = Instantiate(upWall, viewMap);
            up.transform.position = GetWorldPosition(i, height);

            GameObject down = Instantiate(downWall, viewMap);
            down.transform.position = GetWorldPosition(i, center - y - 2);
        }

        for (int i = center - y + 2; i <= center + y - 2; i++)
        {
            if (i == center)
                continue;

            GameObject left = Instantiate(leftWall, viewMap);
            left.transform.position = GetWorldPosition(center - x - 2, i);

            GameObject right = Instantiate(rightWall, viewMap);
            right.transform.position = GetWorldPosition(width, i);
        }
    }

    #endregion
}