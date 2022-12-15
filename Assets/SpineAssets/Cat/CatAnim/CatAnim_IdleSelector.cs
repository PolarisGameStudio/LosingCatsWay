using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_IdleSelector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int[] randomTotal = { 40, 40, 20 };
        int randomSelect = MathfExtension.RandomRate(randomTotal, 100);

        if (randomSelect == 0)
        {
            int randomIndex = Random.Range(0, 5);
            animator.SetInteger(CatAnimTable.IdleSelectIndex.ToString(), randomIndex);

            // IdleAction
            int[] total = { 500, 125, 125, 125, 125 };
            int idleActionRandomIndex = MathfExtension.RandomRate(total, 1000);
            animator.SetInteger(CatAnimTable.IdleActionSelectIndex.ToString(), idleActionRandomIndex);

            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), true);
            animator.SetBool(CatAnimTable.IsPersonality.ToString(), false);
        }
        else if (randomSelect == 1)
        {
            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), false);
            animator.SetBool(CatAnimTable.IsPersonality.ToString(), false);
        }
        else if (randomSelect == 2)
        {
            Cat cat = animator.transform.GetComponent<Cat>();
            CloudCatData cloudCatData = cat.cloudCatData;

            cat.CloseFace();

            int boxTypeIndex = cloudCatData.CatData.PersonalityTypes.FindIndex(x => x == 2);
            int personalityIndex = GetPersonalityIndex(cloudCatData.CatData.PersonalityTypes[0],
                cloudCatData.CatData.PersonalityLevels[0]);

            if (boxTypeIndex != -1)
            {
                int boxLevelIndex = cloudCatData.CatData.PersonalityLevels[boxTypeIndex];
                personalityIndex = GetPersonalityIndex(boxTypeIndex, boxLevelIndex);
            }

            animator.SetInteger(CatAnimTable.Personality.ToString(), personalityIndex);
            animator.SetBool(CatAnimTable.IsNeedIdle.ToString(), false);
            animator.SetBool(CatAnimTable.IsPersonality.ToString(), true);
        }
    }

    private int GetPersonalityIndex(int type, int level)
    {
        if ((type == 0 && level == 3) || (type == 0 && level == 2))
            return 1;

        if ((type == 2 && level == 3) || (type == 2 && level == 2))
            return 2;

        if ((type == 0 && level == 1) || (type == 0 && level == 0))
            return 3;

        if ((type == 3 && level == 1) || (type == 3 && level == 0))
            return 4;

        if ((type == 3 && level == 3) || (type == 3 && level == 2))
            return 5;

        if ((type == 1 && level == 1) || (type == 1 && level == 0))
            return 6;

        if ((type == 2 && level == 1) || (type == 2 && level == 0))
            return 7;

        return 0;
    }
}