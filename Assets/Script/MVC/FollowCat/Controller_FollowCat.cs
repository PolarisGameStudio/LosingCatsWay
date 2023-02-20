using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller_FollowCat : ControllerBehavior
{
    private Camera _mainCamera;
    private LeanDragCamera _leanDragCamera;
    private LeanPinchCamera _leanPinchCamera;

    [ReadOnly] public bool isFollowing;
    
    private Cat followCat;
    
    private float normalZoom = 3.5f;
    private float kittyZoom = 2f;
    private float nextZoom;

    private void Start()
    {
        _mainCamera = Camera.main;
        _leanDragCamera = FindObjectOfType<LeanDragCamera>();
        _leanPinchCamera = FindObjectOfType<LeanPinchCamera>();
    }

    private void Update()
    {
        if (!isFollowing)
            return;

        Vector3 tmp = followCat.transform.position;
        tmp.y += 0.5f;
        tmp.z = -10;

        _mainCamera.transform.position = tmp;
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

        App.system.soundEffect.PlayCatMeow();

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

        _leanDragCamera.enabled = false;
        _leanPinchCamera.enabled = false;

        float zoom = _mainCamera.orthographicSize;
        nextZoom = cat.cloudCatData.CatData.SurviveDays <= 3
            ? kittyZoom
            : normalZoom;
        DOTween.To(() => zoom, x => zoom = x, nextZoom, 0.5f).OnUpdate(() => { _mainCamera.orthographicSize = zoom; });

        isFollowing = true;
    }

    private void CloseFollow()
    {
        _leanPinchCamera.Zoom = nextZoom;
        _leanDragCamera.enabled = true;
        _leanPinchCamera.enabled = true;

        isFollowing = false;
    }

    public void StartTrait()
    {
        followCat.StopMove();
        App.system.cat.PauseCatsGame(true);

        var skinId = followCat.cloudCatData.CatSkinData.UseSkinId;
        int traitIndex = 0;
        float duration = 0f; // 從若丞Spine看
        if (skinId == "Flyfish_Cat")
        {
            traitIndex = 1;
            duration = 8f;
        }
        if (skinId == "Robot_Cat")
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
        App.view.followCat.OpenTrait();

        DOVirtual.DelayedCall(duration - 0.5f, () =>
        {
            App.view.followCat.CloseTrait();
            App.system.screenshot.Close();
        });
        
        DOVirtual.DelayedCall(duration, () =>
        {
            OpenSensor();
            App.system.cat.PauseCatsGame(false);
        });
    }

    public void OpenCultive()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.25f, () =>
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
        App.system.screenshot.OnScreenshotCancel += CloseScreenshot;
        Close();
        App.system.screenshot.Open();
    }

    private void CloseScreenshot()
    {
        Open();
        App.system.screenshot.OnScreenshotComplete -= CloseScreenshot;
        App.system.screenshot.OnScreenshotCancel -= CloseScreenshot;
    }

    public void OpenTraitScreenshot()
    {
        App.system.screenshot.OnScreenshotComplete += CloseTraitScreenshot;
        App.system.screenshot.OnScreenshotCancel += CloseTraitScreenshot;
        App.view.followCat.CloseTrait();
        App.system.screenshot.Open();
    }

    private void CloseTraitScreenshot()
    {
        App.view.followCat.OpenTrait();
        App.system.screenshot.OnScreenshotComplete -= CloseTraitScreenshot;
        App.system.screenshot.OnScreenshotCancel -= CloseTraitScreenshot;
    }

    #endregion

    public void OpenSensor()
    {
        Open();
        
        if (followCat.bigGameBubble.gameObject.activeSelf)
        {
            followCat.bigGameBubble.DOScale(Vector3.one, 0.5f).From(Vector3.zero);
            followCat.bigGameBubble.DOLocalMoveY(3, 0.5f).From(1);
        }
        if (followCat.littleGameBubble.gameObject.activeSelf)
        {
            followCat.littleGameBubble.DOScale(Vector3.one, 0.5f).From(Vector3.zero);
            followCat.littleGameBubble.DOLocalMoveY(3, 0.5f).From(1);
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