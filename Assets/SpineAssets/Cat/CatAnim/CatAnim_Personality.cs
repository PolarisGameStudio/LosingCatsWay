using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_Personality : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Cat cat = animator.transform.GetComponent<Cat>();
        cat.ChangeSkin();
    }
}
