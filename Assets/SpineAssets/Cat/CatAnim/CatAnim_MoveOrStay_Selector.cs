using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_MoveOrStay_Selector : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var catSizeType = CatExtension.GetCatSizeType(animator.gameObject.GetComponent<Cat>().cloudCatData);
        var exitRoomValue = 0.2f;

        switch (catSizeType)
        {
            case 1:
                exitRoomValue = 0.05f;
                break;
            case 2:
                exitRoomValue = 0.35f;
                break;
        }

        if (Random.value <= exitRoomValue) //離開房間
        {
            int[] total = { 50, 50 };

            switch (catSizeType)
            {
                case 1:
                    total[0] = 80;
                    total[1] = 20;
                    break;
                case 2:
                    total[0] = 20;
                    total[1] = 80;
                    break;
            }

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
            int[] total = { 20, 20, 20, 20, 20 };

            switch (catSizeType)
            {
                case 1:
                    total[0] = 10;
                    total[1] = 5;
                    total[2] = 20;
                    total[3] = 40;
                    total[4] = 25;
                    break;
                case 2:
                    total[0] = 35;
                    total[1] = 35;
                    total[2] = 15;
                    total[3] = 5;
                    total[4] = 10;
                    break;
            }

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