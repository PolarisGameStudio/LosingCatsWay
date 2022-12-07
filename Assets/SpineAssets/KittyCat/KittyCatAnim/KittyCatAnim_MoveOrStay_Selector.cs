using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyCatAnim_MoveOrStay_Selector : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var exitRoomValue = 0.3f;

        if (Random.value <= exitRoomValue) //離開房間
        {
            int[] total = { 40, 60 };

            int type = MathfExtension.RandomRate(total, 100);

            switch (type)
            {
                case 0: //走
                    animator.SetBool(CatAnimTable.IsWalk.ToString(), true);
                    break;
                case 1: //跑
                    animator.SetBool(CatAnimTable.IsRun.ToString(), true);
                    break;
            }

            animator.SetBool(CatAnimTable.IsLeftRoom.ToString(), true);
        }
        else
        {
            int[] total = { 15, 30, 15, 30, 10 };

            int type = MathfExtension.RandomRate(total, 100);

            switch (type)
            {
                case 0: //走
                    animator.SetBool(CatAnimTable.IsWalk.ToString(), true);
                    break;
                case 1: //跑
                    animator.SetBool(CatAnimTable.IsRun.ToString(), true);
                    break;
                case 2: //坐
                    animator.SetBool(CatAnimTable.IsSit.ToString(), true);
                    break;
                case 3: //睡
                    animator.SetBool(CatAnimTable.IsSleep.ToString(), true);
                    break;
                case 4: //懶
                    animator.SetBool(CatAnimTable.IsGrasp.ToString(), true);
                    break;
            }

            animator.SetBool(CatAnimTable.IsLeftRoom.ToString(), false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(CatAnimTable.IsWalk.ToString(), false);
        animator.SetBool(CatAnimTable.IsRun.ToString(), false);
        animator.SetBool(CatAnimTable.IsSit.ToString(), false);
        animator.SetBool(CatAnimTable.IsSleep.ToString(), false);
        animator.SetBool(CatAnimTable.IsGrasp.ToString(), false);
    }
}
