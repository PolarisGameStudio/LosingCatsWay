using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTask_DynamicMask : FlowTask_Mask
{
    public GameObject blockRaycastObject;
    public RectTransform content;
    public int targetIndex;

    public override void Enter()
    {
        blockRaycastObject.SetActive(true);
        StartCoroutine(Coroutine());
    }

    public override void Exit()
    {
        base.Exit();
    }

    IEnumerator Coroutine()
    {
        yield return new WaitUntil(() => (content.childCount > targetIndex));
        yield return null;

        RectTransform rect = content.GetChild(targetIndex).GetComponent<RectTransform>();
        focusTarget = rect;
        base.Enter();
        blockRaycastObject.SetActive(false);
    }
}
