using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class View_Build : ViewBehaviour
{
    public GameObject buildType;
    public GameObject buildingType;
    
    public Color[] buildTmpColorStatus;

    public Button okButton;
    public Button cancelButton;

    [Title("MoveBuild")]
    public UIView moveBuild;
    
    [Title("Tween")] 
    public Transform chooseFloorButton;
    public Transform chooseBuildButton;

    public override void Open()
    {
        UIView.InstantShow();
        chooseFloorButton.DOLocalMoveY(-485, 0.25f).From(-750).SetEase(Ease.OutBack);
        chooseBuildButton.DOLocalMoveY(-485, 0.25f).SetDelay(0.0625f).From(-750).SetEase(Ease.OutBack);
    }

    public override void Init()
    {
        base.Init();

        App.model.build.IsBuildingChange += OnIsBuildingChange;
        App.model.build.CanBuildChange += OnCanBuildChange;
        App.model.build.IsMovingChange += OnIsMovingChange;
    }

    public void OnIsBuildingChange(object value)
    {
        bool isBuilding = (bool) value;

        buildType.SetActive(!isBuilding);
        buildingType.SetActive(isBuilding);
        App.system.grid.buildTmp.SetActive(isBuilding);
    }

    public void OnCanBuildChange(object value)
    {
        bool flag = (bool) value;

        okButton.interactable = flag;

        if (flag)
        {
            App.system.grid.buildTmpMask.color = buildTmpColorStatus[0];
        }
        else
        {
            App.system.grid.buildTmpMask.color = buildTmpColorStatus[1];
        }
    }

    public void OnIsMovingChange(object value)
    {
        bool flag = (bool) value;

        cancelButton.gameObject.SetActive(!flag);
    }

    public void OpenMoveBuild()
    {
        moveBuild.Show();
    }

    public void CloseMoveBuild()
    {
        moveBuild.InstantHide();
        App.system.soundEffect.Play("Button");
    }
}