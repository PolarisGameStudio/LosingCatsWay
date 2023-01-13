using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
using DG.Tweening;

public class MyGridSystem : MvcBehaviour
{
    public Color buildGridColor;

    public int width;
    public int height;
    public float cellSize;

    public Transform viewMap;
    public Transform buildMap;

    public int maxHomeParticle;

    //討論結束再public
    public int maxHomeLight;

    [SerializeField] private LeanPlane leanPlane;

    private MyGrid[,] viewGridArray;
    private SpriteRenderer[,] buildGridArray;

    private List<GameObject> objectPool_Floor = new List<GameObject>();

    public void Init()
    {
        width = GetGridSize();
        height = width;

        CreateFloor();
        SetCameraToOrigin();
        CreateOutSide();
    }

    #region Floor

    private void CreateFloor() //TODO Floor pattern
    {
        // width = App.system.player.GridSize;
        // height = App.system.player.GridSize;

        viewGridArray = new MyGrid[width, height];
        buildGridArray = new SpriteRenderer[width, height];

        leanPlane.MaxX = cellSize * (width + 2);
        leanPlane.MaxY = cellSize * (height + 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var floor = App.factory.roomFactory.GetFloorObject("Spring");

                GameObject viewBuffer = Instantiate(floor, viewMap);
                viewBuffer.transform.position = GetWorldPosition(i, j);

                MyGrid myGrid = new MyGrid(0, viewBuffer);
                viewGridArray[i, j] = myGrid;

                GameObject buildBuffer = Instantiate(App.factory.roomFactory.GetLineRoom(), buildMap);
                buildBuffer.transform.position = GetWorldPosition(i, j);

                buildGridArray[i, j] = buildBuffer.transform.GetComponent<SpriteRenderer>();

                objectPool_Floor.Add(viewBuffer);
            }
        }
    }

    #endregion

    #region utiliy

    public void SetCameraToOrigin(bool tween = false)
    {
        float x = width / 2 * cellSize + cellSize / 2;
        float y = height / 2 * cellSize + cellSize / 2;

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

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public MyGrid GetGrid(int x, int y)
    {
        return viewGridArray[x, y];
    }

    public int[] GetGridIndexByPosision(float x, float y)
    {
        int gridX = Convert.ToInt32(x / cellSize);
        int gridY = Convert.ToInt32(y / cellSize);

        return new int[] { gridX, gridY };
    }

    public SpriteRenderer GetBuildGrid(int x, int y)
    {
        return buildGridArray[x, y];
    }

    public MyGrid GetUpGrid(int x, int y)
    {
        if (y >= height - 1)
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

        RefreshBuildMap();
    }

    private void RefreshBuildMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int myGridValue = GetGrid(i, j).Value;

                if (myGridValue == 2)
                {
                    GetBuildGrid(i, j).color = buildGridColor;
                    continue;
                }

                if (myGridValue == 0 || myGridValue == 1)
                {
                    GetBuildGrid(i, j).color = Color.white;
                }
            }
        }
    }

    #endregion

    #region Build

    public void OpenBuildMap()
    {
        buildMap.gameObject.SetActive(true);
    }

    public void CloseBuildMap()
    {
        buildMap.gameObject.SetActive(false);
    }

    public void BuildOrigin(int x, int y, int roomWidth, int roomHeight, GameObject content)
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

        //todo 優化 要拆耦合
        App.system.room.Insert(room, 0);

        tmp.transform.position = GetWorldPosition(x, y);
        RefreshGirdValues();
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
        tmp.name = room.roomData.id;

        //todo 優化 要拆耦合
        App.system.room.Add(room);

        tmp.transform.position = GetWorldPosition(x, y);
        RefreshGirdValues();
    }

    public void Remove(int x, int y, int roomWidth, int roomHeight)
    {
        for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomHeight; j++)
            {
                MyGrid myGrid = GetGrid(x + i, y + j);
                myGrid.Value = 0;

                var floor = App.factory.roomFactory.GetFloorObject("Spring");

                GameObject viewBuffer = Instantiate(floor, viewMap);
                viewBuffer.transform.position = GetWorldPosition(x + i, y + j);

                myGrid.Content = viewBuffer;
            }
        }

        RefreshGirdValues();
    }

    #endregion

    #region Particle

    IEnumerator RefreshHomeParticle()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(10f);
            SetHomeParticle();
        }
    }

    void SetHomeParticle()
    {
        int homeParticleCount = 0;

        //1.Particle Off
        for (int i = 0; i < objectPool_Floor.Count; i++)
        {
            if (objectPool_Floor[i].Equals(null)) continue;
            objectPool_Floor[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        //2.Particle On
        for (int i = 0; i < objectPool_Floor.Count; i++)
        {
            if (objectPool_Floor[i].Equals(null)) continue;

            if (UnityEngine.Random.value < .3f)
            {
                homeParticleCount++;
                objectPool_Floor[i].transform.GetChild(0).gameObject.SetActive(true);
            }

            if (homeParticleCount >= maxHomeParticle) break;
        }
    }

    #endregion

    public void ChangeFloorSpriteByKey(string floorName)
    {
        //StartCoroutine(ChangeFloor(floorName));

        for (int i = 0; i < objectPool_Floor.Count; i++)
        {
            if (objectPool_Floor[i].Equals(null)) continue;

            Sprite sprite = App.factory.roomFactory.GetFloorByKey(floorName);
            SpriteRenderer renderer = objectPool_Floor[i].GetComponent<SpriteRenderer>();

            renderer.sprite = sprite;
        }

        RefreshBuildMap();
    }

    IEnumerator ChangeFloor(string floorName)
    {
        for (int i = 0; i < objectPool_Floor.Count; i++)
        {
            if (objectPool_Floor[i].Equals(null)) continue;

            Sprite sprite = App.factory.roomFactory.GetFloorByKey(floorName);
            SpriteRenderer renderer = objectPool_Floor[i].GetComponent<SpriteRenderer>();

            renderer.sprite = sprite;

            yield return null;
        }

        RefreshBuildMap();
    }

    #region OutSide

    private void CreateOutSide()
    {
        CreateOutSideCenter();
        CreateOutSideCorner();
        CreateOutSideWall();
        CreateOutSideTree();
    }

    private void CreateOutSideCenter()
    {
        var factory = App.factory.roomFactory.floorContainers["Spring"];

        GameObject up = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.CenterUp), viewMap);
        up.transform.position = GetWorldPosition(width / 2, height);

        GameObject down = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.CenterDown), viewMap);
        down.transform.position = GetWorldPosition(width / 2, -2);

        GameObject left = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.CenterLeft), viewMap);
        left.transform.position = GetWorldPosition(-2, height / 2);

        GameObject right = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.CenterRight), viewMap);
        right.transform.position = GetWorldPosition(width, height / 2);
    }

    private void CreateOutSideCorner()
    {
        var factory = App.factory.roomFactory.floorContainers["Spring"];

        // UpRight

        GameObject upRight1 = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.UpRight1), viewMap);
        upRight1.transform.position = GetWorldPosition(width, height);

        GameObject upRight2 = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.UpRight2), viewMap);
        upRight2.transform.position = GetWorldPosition(width - 2, height);

        GameObject upRight3 = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.UpRight3), viewMap);
        upRight3.transform.position = GetWorldPosition(width, height - 2);

        // UpLeft

        GameObject upLeft1 = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.UpLeft1), viewMap);
        upLeft1.transform.position = GetWorldPosition(-2, height);

        GameObject upLeft2 = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.UpLeft2), viewMap);
        upLeft2.transform.position = GetWorldPosition(0, height);

        GameObject upLeft3 = Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.UpLeft3), viewMap);
        upLeft3.transform.position = GetWorldPosition(-2, height - 2);

        // DownRight

        GameObject downRight1 =
            Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.DownRight1), viewMap);
        downRight1.transform.position = GetWorldPosition(width, -2);

        GameObject downRight2 =
            Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.DownRight2), viewMap);
        downRight2.transform.position = GetWorldPosition(width - 2, -2);

        GameObject downRight3 =
            Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.DownRight3), viewMap);
        downRight3.transform.position = GetWorldPosition(width, 0);

        // DownLeft

        GameObject downLeft1 =
            Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.DownLeft1), viewMap);
        downLeft1.transform.position = GetWorldPosition(-2, -2);

        GameObject downLeft2 =
            Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.DownLeft2), viewMap);
        downLeft2.transform.position = GetWorldPosition(0, -2);

        GameObject downLeft3 =
            Instantiate(factory.GetOutside(FloorContainer.OutsideEnum.DownLeft3), viewMap);
        downLeft3.transform.position = GetWorldPosition(-2, 0);
    }

    private void CreateOutSideWall()
    {
        var factory = App.factory.roomFactory.floorContainers["Spring"];

        // Up & Down

        int center = width / 2;
        int x = width / 2;
        int y = height / 2;

        var upWall = factory.GetOutside(FloorContainer.OutsideEnum.FloorUp);
        var downWall = factory.GetOutside(FloorContainer.OutsideEnum.FloorDown);
        // var leftWall = factory.outSides[FloorContainer.OutsideEnum.FloorLeft];
        // var rightWall = factory.outSides[FloorContainer.OutsideEnum.FloorRight];

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

            GameObject randomLeft = factory.outSides_Left.GetRandom();
            GameObject left = Instantiate(randomLeft, viewMap);
            left.transform.position = GetWorldPosition(center - x - 2, i);

            GameObject randomRight = factory.outSides_Right.GetRandom();
            GameObject right = Instantiate(randomRight, viewMap);
            right.transform.position = GetWorldPosition(width, i);
        }
    }

    private void CreateOutSideTree()
    {
        var factory = App.factory.roomFactory.floorContainers["Spring"];

        int center = width / 2;
        int x = width / 2;
        int y = height / 2;

        for (int i = center - x + 2; i <= center + x - 2; i++)
        {
            if (i == center)
                continue;

            GameObject randomTreeUp = factory.outSides_Trees.GetRandom();
            GameObject up = Instantiate(randomTreeUp, viewMap);
            up.transform.position = GetWorldPosition(i, height + 1);

            GameObject randomTreeDown = factory.outSides_Trees.GetRandom();
            GameObject down = Instantiate(randomTreeDown, viewMap);
            down.transform.position = GetWorldPosition(i, center - y - 2);
            down.transform.position = new Vector3(down.transform.position.x, down.transform.position.y - cellSize / 2,
                down.transform.position.z);
        }

        for (int i = center - y + 2; i <= center + y - 2; i++)
        {
            if (i == center)
                continue;

            GameObject randomTreeLeft = factory.outSides_Trees.GetRandom();
            GameObject left = Instantiate(randomTreeLeft, viewMap);
            left.transform.position = GetWorldPosition(center - x - 2, i);
            left.transform.position = new Vector3(left.transform.position.x - cellSize / 2, left.transform.position.y,
                left.transform.position.z);

            GameObject randomTreeRight = factory.outSides_Trees.GetRandom();
            GameObject right = Instantiate(randomTreeRight, viewMap);
            right.transform.position = GetWorldPosition(width + 1, i);
        }
    }

    #endregion

    private int GetGridSize()
    {
        int level = Math.Clamp(App.system.player.GridSizeLevel, 1, 11);
        int result = 9 + level * 2;

        return result;
    }
}