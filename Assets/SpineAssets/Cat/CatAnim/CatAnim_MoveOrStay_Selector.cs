using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_MoveOrStay_Selector : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Cat cat = animator.transform.GetComponent<Cat>();
        CloudCatData cloudCatData = cat.cloudCatData;
        
        int boxTypeIndex = cloudCatData.CatData.PersonalityTypes.FindIndex(x => x == 2);
        int personalityIndex = GetPersonalityIndex(cloudCatData.CatData.PersonalityTypes[0],
            cloudCatData.CatData.PersonalityLevels[0]);

        if (boxTypeIndex != -1)
        {
            int boxLevelIndex = cloudCatData.CatData.PersonalityLevels[boxTypeIndex];
            personalityIndex = GetPersonalityIndex(boxTypeIndex, boxLevelIndex);
        }
        
        // 離開房間機率
        var exitRoomValue = 0.4f;

        switch (personalityIndex)
        {
            case 1:
                exitRoomValue = 0.1f;
                break;
            case 2:
                exitRoomValue = 0.6f;
                break;
            case 3:
                exitRoomValue = 0.05f;
                break;
            case 4:
                exitRoomValue = 0.35f;
                break;
            case 5:
                exitRoomValue = 0.05f;
                break;
            case 6:
                exitRoomValue = 0.35f;
                break;
            case 7:
                exitRoomValue = 0.05f;
                break;
        }
        
        if (Random.value <= exitRoomValue) // 怎麼離開房間的
        {
            int[] total = { 95, 5 };

            switch (personalityIndex)
            {
                case 1:
                    total[0] = 60;
                    total[1] = 40;
                    break;
                case 2:
                    total[0] = 90;
                    total[1] = 10;
                    break;
                case 3:
                    total[0] = 70;
                    total[1] = 30;
                    break;
                case 4:
                    total[0] = 65;
                    total[1] = 35;
                    break;
                case 5:
                    total[0] = 60;
                    total[1] = 40;
                    break;
                case 6:
                    total[0] = 50;
                    total[1] = 50;
                    break;
                case 7:
                    total[0] = 70;
                    total[1] = 30;
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
            int[] total = { 40, 5, 10, 25, 20 }; // 在房間要藂三小

            switch (personalityIndex)
            {
                case 1:
                    total[0] = 15;
                    total[1] = 20;
                    total[2] = 25;
                    total[3] = 35;
                    total[4] = 5;
                    break;
                case 2:
                    total[0] = 35;
                    total[1] = 45;
                    total[2] = 10;
                    total[3] = 5;
                    total[4] = 5;
                    break;
                case 3:
                    total[0] = 10;
                    total[1] = 10;
                    total[2] = 30;
                    total[3] = 40;
                    total[4] = 10;
                    break;
                case 4:
                    total[0] = 20;
                    total[1] = 10;
                    total[2] = 25;
                    total[3] = 20;
                    total[4] = 25;
                    break;
                case 5:
                    total[0] = 20;
                    total[1] = 10;
                    total[2] = 35;
                    total[3] = 10;
                    total[4] = 25;
                    break;
                case 6:
                    total[0] = 35;
                    total[1] = 25;
                    total[2] = 15;
                    total[3] = 10;
                    total[4] = 15;
                    break;
                case 7:
                    total[0] = 15;
                    total[1] = 10;
                    total[2] = 40;
                    total[3] = 25;
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
    
    private int GetPersonalityIndex(int type, int level)
    {
        if ((type == 0 && level == 0) || (type == 0 && level == 1))
            return 1;

        if ((type == 1 && level == 2) || (type == 1 && level == 3))
            return 2;

        if ((type == 1 && level == 0) || (type == 1 && level == 1))
            return 3;

        if ((type == 2 && level == 2) || (type == 2 && level == 3))
            return 4;

        if ((type == 2 && level == 0) || (type == 2 && level == 1))
            return 5;

        if ((type == 3 && level == 2) || (type == 3 && level == 3))
            return 6;

        if ((type == 3 && level == 0) || (type == 2 && level == 1))
            return 7;

        return 0;
    }
}