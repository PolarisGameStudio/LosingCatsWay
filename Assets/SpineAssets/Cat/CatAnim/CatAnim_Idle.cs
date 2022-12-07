using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_Idle : StateMachineBehaviour
{
    public float exitValue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float value = Random.value;

        if (value <= exitValue)
            animator.SetBool(CatAnimTable.IsCanExit.ToString(), true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(CatAnimTable.IsCanExit.ToString(), false);
    }
}