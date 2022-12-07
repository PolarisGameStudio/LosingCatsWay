using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SkeletonGraphic skeletonRenderer;

    public SkeletonDataAsset a;
    public SkeletonDataAsset b;

    public float aS;
    public float bS;
    
    [Button]
    public void A()
    {
        skeletonRenderer.skeletonDataAsset = a;
        skeletonRenderer.Initialize(true);
        skeletonRenderer.Skeleton.SetSkin("Benz");

        skeletonRenderer.gameObject.transform.DOScale(Vector3.one * aS, 0);
        
        skeletonRenderer.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
    }

    [Button]
    public void B()
    {
        skeletonRenderer.skeletonDataAsset = b;
        skeletonRenderer.Initialize(true);
        skeletonRenderer.Skeleton.SetSkin("Ordinary");
        
        skeletonRenderer.gameObject.transform.DOScale(Vector3.one * bS, 0);

        skeletonRenderer.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
    }
}