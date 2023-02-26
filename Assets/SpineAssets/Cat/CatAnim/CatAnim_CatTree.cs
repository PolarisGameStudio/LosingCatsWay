using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class CatAnim_CatTree : StateMachineBehaviour
{
    private MeshRenderer meshRenderer;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meshRenderer = animator.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 1;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        meshRenderer.sortingOrder = 0;
    }
}
