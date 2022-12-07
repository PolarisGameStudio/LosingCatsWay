using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

public class Controller_FollowCat : ControllerBehavior
{
    public Camera mainCamera;
    public LeanDragCamera leanDragCamera;
    public LeanPinchCamera leanPinchCamera;

    [ReadOnly] public bool isFollowing;
    
    private Cat followCat;
    
    private float normalZoom = 3.5f;
    private float kittyZoom = 2f;
    private float nextZoom;

    private void Update()
    {
        if (!isFollowing)
            return;

        Vector3 tmp = followCat.transform.position;
        tmp.y += 0.5f;
        tmp.z = -10;

        mainCamera.transform.position = tmp;
    }

    public void Open()
    {
        App.view.followCat.Open();
    }

    public void Close()
    {
        App.view.followCat.Close();
    }

    /// <summary>
    /// 貓身上點擊選中並跟隨
    /// </summary>
    /// <param name="cat"></param>
    public void Select(Cat cat)
    {
        if (isFollowing)
            return;

        if (App.model.build.IsCanMoveOrRemove)
            return;

        StartFollow(cat);

        App.view.followCat.SetCat(cat.cloudCatData);
        App.model.followCat.SelectedCat = cat;

        App.controller.lobby.Close();
        App.view.followCat.Open();
    }
    
    public void CloseByOpenLobby()
    {
        CloseFollow();
        
        App.controller.lobby.Open();
        App.view.followCat.Close();
    }

    public void SelectByOnlyFollw(Cat cat)
    {
        StartFollow(cat);
    }

    private void StartFollow(Cat cat)
    {
        followCat = cat;

        leanDragCamera.enabled = false;
        leanPinchCamera.enabled = false;

        float zoom = mainCamera.orthographicSize;
        nextZoom = (CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays) == 0)
            ? kittyZoom
            : normalZoom;
        DOTween.To(() => zoom, x => zoom = x, nextZoom, 0.5f).OnUpdate(() => { mainCamera.orthographicSize = zoom; });

        isFollowing = true;
    }

    private void CloseFollow()
    {
        leanPinchCamera.Zoom = nextZoom;
        leanDragCamera.enabled = true;
        leanPinchCamera.enabled = true;

        isFollowing = false;
    }

    public void StartTrait()
    {
        var trait = followCat.cloudCatData.CatData.Trait;

        char traitType = 'C';
        int traitIndex;
        
        int traitTypeIndex = 0;

        if (trait.Contains('R'))
        {
            traitType = 'R';
            traitTypeIndex = 1;
        }

        if (trait.Contains('S'))
        {
            traitType = 'S';
            traitTypeIndex = 2;
        }

        traitIndex = Convert.ToInt32(trait.Split(traitType)[1]);

        followCat.StopMove();
        
        var animator = followCat.GetComponent<Animator>();
        animator.SetInteger(CatAnimTable.TraitType.ToString(), traitTypeIndex);
        animator.SetInteger(CatAnimTable.TraitIndex.ToString(), traitIndex);
        
        animator.Play(CatAnimTable.ToTrait.ToString());
    }

    public void OpenCultive()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.model.cultive.SelectedCat = App.model.followCat.SelectedCat;
            App.model.cultive.OpenFromIndex = 1;
            App.controller.cultive.Open();
        });
    }

    #region Screenshot

    /// <summary>
    /// 跟隨貓按鈕開啓截圖頁面
    /// </summary>
    public void OpenScreenshot()
    {
        App.system.screenshot.OnScreenshotComplete += CloseScreenshot;
        App.system.screenshot.OnClose += CloseScreenshot;
        Close();
        App.system.screenshot.Open();
    }

    private void CloseScreenshot()
    {
        Open();
        App.system.screenshot.OnScreenshotComplete -= CloseScreenshot;
        App.system.screenshot.OnClose -= CloseScreenshot;
    }

    #endregion
}