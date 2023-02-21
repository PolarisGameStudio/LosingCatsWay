using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BuyCatSubView : ViewBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public Animator animator;
    public GameObject sparklePaticle;
    public GameObject tipText;
    public GameObject clickButton;
    private CloudCatData _cloudCatData;
    
    public void Open(CloudCatData cloudCatData)
    {
        skeletonGraphic.enabled = true;
        clickButton.SetActive(true);
        
        sparklePaticle.SetActive(true);
        tipText.SetActive(true);
        animator.enabled = true;
        animator.Play("BuyCat_Aniamrtion");
        
        _cloudCatData = cloudCatData;
        skeletonGraphic.AnimationState.SetAnimation(0, "Store_Cat/Box_IDLE", true);
        base.Open();
    }

    public void Click()
    {
        animator.enabled = false;
        clickButton.SetActive(false);
        sparklePaticle.SetActive(false);
        tipText.SetActive(false);
        
        skeletonGraphic.AnimationState.SetAnimation(0, "Store_Cat/Buy_Cat", false);
        DOVirtual.DelayedCall(7f, () =>
        {
            Close();
            App.system.catRename.CantCancel().Active(_cloudCatData, "Location1");
        });
    }

    public override void Close()
    {
        skeletonGraphic.enabled = false;
        base.Close();
    }
}
