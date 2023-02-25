using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyCatAnim_Sleep_Selector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int randomIndex = Random.Range(0, 2);
        animator.SetInteger(CatAnimTable.SleepSelectIndex.ToString(), randomIndex);
    }
}
