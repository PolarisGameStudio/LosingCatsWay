using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_SitAction_Selector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int[] total = { 50, 15, 15, 20 };
        int randomIndex = MathfExtension.RandomRate(total, 100);
        animator.SetInteger(CatAnimTable.SitActionSelectIndex.ToString(), randomIndex);
    }
}
