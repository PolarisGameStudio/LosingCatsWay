using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class View_HospitalDoctorCheck : ViewBehaviour
{
    [SerializeField] private CatSkin catSkin;

    [Title("Spine")]
    [SerializeField] private SkeletonGraphic functionGraphic;
    [SerializeField] private GameObject tableGraphic;

    public override void Init()
    {
        base.Init();
        App.model.hospital.OnSelectedCatChange += OnSelectedCatChange;
    }

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        catSkin.ChangeSkin(cat.cloudCatData);
        //todo scale function spine
    }

    public override void Close()
    {
        base.Close();
        tableGraphic.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        tableGraphic.SetActive(true);
        PlayDoctorCheck();
    }

    private void PlayDoctorCheck()
    {
        catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Hospital_Cat/Checking_Cat", false);
        TrackEntry t = functionGraphic.AnimationState.SetAnimation(0, "Hospital_Tool/Checking", false);
        t.Complete += DoctorCheckEnd;
    }

    private void DoctorCheckEnd(TrackEntry trackentry)
    {
        trackentry.Complete -= DoctorCheckEnd;
        App.controller.hospital.DoctorCheckEnd();
    }
}
