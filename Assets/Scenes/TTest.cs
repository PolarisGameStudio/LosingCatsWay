using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class TTest : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public SkeletonDataAsset kittyCatDataAsset;
    public SkeletonDataAsset commonCatDataAsset;

    [Button]
    public void Active1()
    {
        skeletonGraphic.initialSkinName = "Ordinary";

        skeletonGraphic.skeletonDataAsset = kittyCatDataAsset;
        skeletonGraphic.Initialize(true);
        skeletonGraphic.Skeleton.SetSkin("Ordinary");
        skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
    }
    
    [Button]
    public void Active2()
    {
        skeletonGraphic.initialSkinName = "Normal_Cat/Benz";

        skeletonGraphic.skeletonDataAsset = commonCatDataAsset;
        skeletonGraphic.Initialize(true);
        skeletonGraphic.Skeleton.SetSkin("Normal_Cat/Benz");
        skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
    }
}