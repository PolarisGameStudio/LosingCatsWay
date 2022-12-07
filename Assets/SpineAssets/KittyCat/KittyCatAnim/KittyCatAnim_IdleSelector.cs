using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyCatAnim_IdleSelector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Random.value > 0.5f)
        {
            int randomIndex = Random.Range(0, 2);
            animator.SetInteger(CatAnimTable.IdleSelectIndex.ToString(), randomIndex);

            // IdleAction
            int[] total = { 80, 20 };
            int idleActionRandomIndex = MathfExtension.RandomRate(total, 100);
            animator.SetInteger(CatAnimTable.IdleActionSelectIndex.ToString(), idleActionRandomIndex);

            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), true);
        }
        else
            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), false);
    }
}