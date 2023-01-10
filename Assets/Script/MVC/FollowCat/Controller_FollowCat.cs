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

    /// 貓身上點擊選中並跟隨
    public void Select(Cat cat)
    {
        if (isFollowing)
            return;

        if (App.model.build.IsCanMoveOrRemove)
            return;

        StartFollow(cat);

        App.model.followCat.SelectedCat = cat;

        App.controller.lobby.Close();
        Open();
    }
    
    public void CloseByOpenLobby()
    {
        CloseFollow();
        
        App.controller.lobby.Open();
        Close();
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
        // var trait = followCat.cloudCatData.CatData.Trait;
        //
        // char traitType = 'C';
        // int traitIndex;
        //
        // int traitTypeIndex = 0;
        //
        // if (trait.Contains('R'))
        // {
        //     traitType = 'R';
        //     traitTypeIndex = 1;
        // }
        //
        // if (trait.Contains('S'))
        // {
        //     traitType = 'S';
        //     traitTypeIndex = 2;
        // }
        //
        // traitIndex = Convert.ToInt32(trait.Split(traitType)[1]);
        //
        // followCat.StopMove();
        //
        // var animator = followCat.GetComponent<Animator>();
        // animator.SetInteger(CatAnimTable.TraitType.ToString(), traitTypeIndex);
        // animator.SetInteger(CatAnimTable.TraitIndex.ToString(), traitIndex);
        //
        // animator.Play(CatAnimTable.ToTrait.ToString());

        followCat.StopMove();

        var skinId = followCat.cloudCatData.CatSkinData.UseSkinId;
        int traitIndex = 0;
        float duration = 0f; // 從若丞Spine看
        if (skinId == "Flyfish")
        {
            traitIndex = 1;
            duration = 8f;
        }
        if (skinId == "Robot")
        {
            traitIndex = 2;
            duration = 17.45f;
        }

        if (skinId == "Magic_Hat")
        {
            traitIndex = 3;
            duration = 13.67f;
        }
        
        var animator = followCat.GetComponent<Animator>();
        animator.SetInteger(CatAnimTable.TraitType.ToString(), 2);
        animator.SetInteger(CatAnimTable.TraitIndex.ToString(), traitIndex);
        
        animator.Play(CatAnimTable.ToTrait.ToString());
        CloseSensor();
        DOVirtual.DelayedCall(duration, OpenSensor);
    }

    public void OpenCultive()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.model.cultive.SelectedCat = App.model.followCat.SelectedCat;
            App.model.cultive.OpenFromIndex = 1;
            CloseFollow();
            App.controller.cultive.Open();
        });
    }

    #region Screenshot
    
    public void OpenScreenshot()
    {
        App.system.screenshot.OnScreenshotComplete += CloseScreenshot;
        Close();
        App.system.screenshot.Open();
    }

    private void CloseScreenshot()
    {
        Open();
        App.system.screenshot.OnScreenshotComplete -= CloseScreenshot;
    }

    #endregion

    public void OpenSensor()
    {
        Open();
        
        if (followCat.bigGameBubble.gameObject.activeSelf)
        {
            followCat.bigGameBubble.DOScale(Vector3.one, 0.5f).From(Vector3.zero);
            followCat.bigGameBubble.DOLocalMoveY(followCat.bigGameBubbleDistance, 0.5f).From(1);
        }
        if (followCat.littleGameBubble.gameObject.activeSelf)
        {
            followCat.littleGameBubble.DOScale(Vector3.one, 0.5f).From(Vector3.zero);
            followCat.littleGameBubble.DOLocalMoveY(followCat.littleGameBubbleDistance, 0.5f).From(1);
        }
    }

    public void CloseSensor()
    {
        Close();
        
        if (followCat.bigGameBubble.gameObject.activeSelf)
        {
            followCat.bigGameBubble.DOScale(Vector3.zero, 0.5f);
            followCat.bigGameBubble.DOLocalMoveY(1, 0.5f);
        }
        if (followCat.littleGameBubble.gameObject.activeSelf)
        {
            followCat.littleGameBubble.DOScale(Vector3.zero, 0.5f);
            followCat.littleGameBubble.DOLocalMoveY(1, 0.5f);
        }
    }
}