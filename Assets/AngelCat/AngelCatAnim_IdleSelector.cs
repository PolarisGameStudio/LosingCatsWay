using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelCatAnim_IdleSelector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int[] randomTotal = { 40, 60 };
        int randomSelect = MathfExtension.RandomRate(randomTotal, 100);

        if (randomSelect == 0)
        {
            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), true);
        }
        else if (randomSelect == 1)
        {
            if (Random.value > 0.5f)
                animator.SetBool(CatAnimTable.IsLeftRoom.ToString(), true);
            else
                animator.SetBool(CatAnimTable.IsLeftRoom.ToString(), false);

            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), false);
        }
        
    }
}
