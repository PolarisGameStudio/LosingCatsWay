using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_IdleSelector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Random.value > 0.5f)
        {
            int randomIndex = Random.Range(0, 5);
            animator.SetInteger(CatAnimTable.IdleSelectIndex.ToString(), randomIndex);

            // IdleAction
            int[] total = { 500, 125, 125, 125, 125 };
            int idleActionRandomIndex = MathfExtension.RandomRate(total, 1000);
            animator.SetInteger(CatAnimTable.IdleActionSelectIndex.ToString(), idleActionRandomIndex);

            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), true);
        }
        else
            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), false);
    }
}