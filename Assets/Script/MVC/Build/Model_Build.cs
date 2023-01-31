using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Build : ModelBehavior
{
    public bool IsCanMoveOrRemove;
    
    private bool isBuilding;
    private bool canBuild;
    private bool isMoving;
    private bool isOpenMoveBuild;
    private Room selectedRoom;

    public bool IsBuilding
    {
        get => isBuilding;
        set
        {
            isBuilding = value;
            IsBuildingChange(value);
        }
    }

    public bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            IsMovingChange(value);
        }
    }

    public bool CanBuild
    {
        get => canBuild;
        set
        {
            canBuild = value;
            CanBuildChange(value);
        }
    }

    public Room SelectedRoom
    {
        get => selectedRoom;
        set => selectedRoom = value;
    }

    public bool IsOpenMoveBuild
    {
        get => isOpenMoveBuild;
        set => isOpenMoveBuild = value;
    }

    public ValueChange IsBuildingChange;
    public ValueChange CanBuildChange;
    public ValueChange IsMovingChange;
}