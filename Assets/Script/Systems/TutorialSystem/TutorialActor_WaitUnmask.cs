using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_WaitUnmask : TutorialActor
{
    [Title("WaitUnmask")]
    [SerializeField] private Transform targetParent;
    [SerializeField] private int targetIndex;
    [SerializeField] private Unmask unmask;

    public override void Enter()
    {
        StartCoroutine(WaitMask());
        base.Enter();
    }

    private IEnumerator WaitMask()
    {
        yield return new WaitUntil(() => targetParent.childCount > targetIndex);
        yield return null;
        
        RectTransform targetRect = targetParent.GetChild(targetIndex).transform as RectTransform;
        unmask.fitTarget = targetRect;
    }
}
