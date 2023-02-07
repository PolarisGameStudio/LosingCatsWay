using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class View_HospitalDoctorFunction : ViewBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private SkeletonGraphic functionGraphic;
    [SerializeField] private GameObject tableGraphic;
    
    [Title("Animation")]
    [SerializeField] private string[] functionTrackNames;
    [SerializeField] private string[] catTrackNames;

    public override void Open()
    {
        base.Open();
        tableGraphic.SetActive(true);
        functionGraphic.gameObject.SetActive(true);
        catSkin.SetActive(true);
        PlayDoctorFunction();
    }

    public override void Close()
    {
        base.Close();
        tableGraphic.SetActive(false);
        functionGraphic.gameObject.SetActive(false);
        catSkin.SetActive(false);
    }

    public override void Init()
    {
        base.Init();
        App.model.hospital.OnSelectedCatChange += OnSelectedCatChange;
        App.model.hospital.OnFunctionIndexChange += OnFunctionIndexChange;
        App.model.hospital.OnIsCatHasWormChange += OnIsCatHasWormChange;
    }

    private void OnIsCatHasWormChange(object value)
    {
        bool hasWorm = (bool)value;
        
        if (!hasWorm)
            return;
        
        catSkin.skeletonGraphic.freeze = true;
        functionGraphic.freeze = true;
        
        catSkin.skeletonGraphic.AnimationState.AddAnimation(0, "Hospital_Cat/Deworming_Cat", false, 0);
        functionGraphic.AnimationState.AddAnimation(0, "Hospital_Tool/Deworming", false, 0);
    }

    private void OnFunctionIndexChange(object value)
    {
        int index = (int)value;

        catSkin.skeletonGraphic.freeze = true;
        functionGraphic.freeze = true;
        
        var catState = catSkin.skeletonGraphic.AnimationState;
        var functionState = functionGraphic.AnimationState;

        string catTrackName = catTrackNames[index];
        string functionTrackName = functionTrackNames[index];

        catState.SetAnimation(0, catTrackName, false);
        functionState.SetAnimation(0, functionTrackName, false);
    }

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        catSkin.ChangeSkin(cat.cloudCatData);
    }

    private void PlayDoctorFunction()
    {
        catSkin.skeletonGraphic.freeze = false;
        functionGraphic.freeze = false;

        TrackEntry t = functionGraphic.AnimationState.AddEmptyAnimation(0, 0, 0);
        t.Start += DoctorFunctionEnd;
    }

    public void SkipDoctorFunction()
    {
        var catTrack = catSkin.skeletonGraphic.AnimationState.GetCurrent(0);
        catTrack.TrackTime = catTrack.Animation.Duration;
        var functionTrack = functionGraphic.AnimationState.GetCurrent(0);
        functionTrack.TrackTime = functionTrack.Animation.Duration;
    }

    private void DoctorFunctionEnd(TrackEntry trackEntry)
    {
        trackEntry.Start -= DoctorFunctionEnd;
        App.controller.hospital.CloseDoctorFuntion();
        App.controller.hospital.OpenDoctorResult();
    }
}
