using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

public class View_HospitalDoctorFunction : ViewBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private SkeletonGraphic functionGraphic;
    [SerializeField] private GameObject tableGraphic;
    
    [Title("Animation")]
    [SerializeField] private string[] functionTrackNames;
    [SerializeField] private string[] catTrackNames;

    private List<string> catAnimStrings = new List<string>();
    private List<string> functionAnimStrings = new List<string>();

    private int tmpFunctionIndex;
    
    public override void Open()
    {
        base.Open();
        
        tableGraphic.SetActive(true);
        functionGraphic.gameObject.SetActive(true);
        catSkin.SetActive(true);

        functionGraphic.transform.localScale = catSkin.transform.localScale;
        
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
        App.model.hospital.OnFunctionIndexChange += OnFunctionIndexChange;
        App.model.hospital.OnIsCatHasWormChange += OnIsCatHasWormChange;
        App.model.hospital.OnTmpCatChange += OnTmpCatChange;
    }

    private void OnTmpCatChange(object value)
    {
        Cat cat = (Cat)value;
        catSkin.ChangeSkin(cat.cloudCatData);
        
        if (!string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId))
        {
            catSkin.CloseSickEye();
            catSkin.OpenEye(cat.cloudCatData);
        }
    }

    private void OnIsCatHasWormChange(object value)
    {
        bool hasWorm = (bool)value;

        if (tmpFunctionIndex != 0)
            return;
        
        if (!hasWorm)
            return;
        
        catAnimStrings.Add("Hospital_Cat/Deworming_Cat");
        functionAnimStrings.Add("Hospital_Tool/Deworming");
    }

    private void OnFunctionIndexChange(object value)
    {
        int index = (int)value;

        tmpFunctionIndex = index;

        catAnimStrings.Clear();
        functionAnimStrings.Clear();

        catAnimStrings.Add(catTrackNames[index]);
        functionAnimStrings.Add(functionTrackNames[index]);
    }

    private void PlayDoctorFunction()
    {
        catSkin.skeletonGraphic.AnimationState.ClearTracks();
        functionGraphic.AnimationState.ClearTracks();

        for (int i = 0; i < catAnimStrings.Count; i++)
        {
            Animation catAnim = catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimStrings[i]);
            Animation functionAnim = functionGraphic.SkeletonData.FindAnimation(functionAnimStrings[i]);
            catAnim.Duration = functionAnim.Duration;
            
            if (i == 0)
            {
                catSkin.skeletonGraphic.AnimationState.SetAnimation(0, catAnim, false);
            }
            else
            {
                catSkin.skeletonGraphic.AnimationState.AddAnimation(0, catAnim, false, 0);
            }
        }

        for (int i = 0; i < functionAnimStrings.Count; i++)
            if (i == 0)
            {
                functionGraphic.AnimationState.SetAnimation(0, functionAnimStrings[i], false);
            }
            else
            {
                functionGraphic.AnimationState.AddAnimation(0, functionAnimStrings[i], false, 0);
            }
        
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
        
        catAnimStrings.Clear();
        functionAnimStrings.Clear();

        App.controller.hospital.CloseDoctorFuntion();
        App.controller.hospital.OpenDoctorResult();
    }
}
