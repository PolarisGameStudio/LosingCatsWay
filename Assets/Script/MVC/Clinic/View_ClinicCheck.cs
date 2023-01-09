using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class View_ClinicCheck : ViewBehaviour
{
    [Title("Spine")]
    [SerializeField] private string funcAnimName;
    [SerializeField] private string catAnimName;
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private CatSkin resultCatSkin;
    [SerializeField] private SkeletonGraphic tableGraphic;
    [SerializeField] private SkeletonGraphic functionGraphic;

    private string sickId;
    private Cat cat;
    
    public override void Init()
    {
        base.Init();
        App.model.clinic.OnSelectedCatChange += OnSelectedCatChange;
    }

    private void OnSelectedCatChange(object value)
    {
        cat = (Cat)value;

        catSkin.ChangeSkin(cat.cloudCatData);
        resultCatSkin.ChangeSkin(cat.cloudCatData);

        functionGraphic.transform.localScale = catSkin.transform.localScale;

        sickId = cat.cloudCatData.CatHealthData.SickId;
    }

    public override void Open()
    {
        base.Open();
        
        catSkin.SetActive(true);
        catSkin.ChangeSkin(cat.cloudCatData);
        catSkin.CloseSickEye();
        catSkin.OpenEye(cat.cloudCatData);
        
        tableGraphic.gameObject.SetActive(true);
        functionGraphic.gameObject.SetActive(true);

        Spine.Animation checkCatAnim = catSkin.skeletonGraphic.SkeletonData.FindAnimation(catAnimName);
        Spine.Animation checkAnim = functionGraphic.SkeletonData.FindAnimation(funcAnimName);

        catSkin.skeletonGraphic.AnimationState.SetAnimation(0, checkCatAnim, false);
        TrackEntry trackEntry = functionGraphic.AnimationState.SetAnimation(0, checkAnim, false);
        trackEntry.Complete += CheckComplete;
    }

    private void CheckComplete(TrackEntry trackEntry)
    {
        trackEntry.Complete -= CheckComplete;
        Close();

        if (sickId is "SK001" or "SK002")
        {
            App.controller.clinic.OpenCheckResult();
            return;
        }
        
        App.controller.clinic.OpenInvoice();
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
        tableGraphic.gameObject.SetActive(false);
        functionGraphic.gameObject.SetActive(false);
    }
}
