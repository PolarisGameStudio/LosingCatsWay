using System.Collections;
using System.Collections.Generic;
using PolyNav;
using UnityEngine;

public class CatAnim_Run : StateMachineBehaviour
{
    private Cat cat;

    private PolyNavAgent polyNavAgent;
    private DirectionChecker directionChecker;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cat = animator.gameObject.GetComponent<Cat>();
        directionChecker = animator.GetComponent<DirectionChecker>();
        polyNavAgent = animator.GetComponent<PolyNavAgent>();
        
        polyNavAgent.maxSpeed = 2;

        bool isLeftRoom = animator.GetBool(CatAnimTable.IsLeftRoom.ToString());

        if (isLeftRoom)
        {
            var catAgeLevel = CatExtension.GetCatAgeLevel(cat.cloudCatData.CatData.SurviveDays);

            if (catAgeLevel != 0 && Random.value >= 0.7f)
            {
                cat.MoveToSpecialSpineRoom();
                return;
            }

            cat.MoveToRandomRoom();
            return;
        }

        cat.RandomMoveAtRoom();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!directionChecker.CheckIsMoving())
            animator.SetBool(CatAnimTable.IsCanExit.ToString(), true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(CatAnimTable.IsCanExit.ToString(), false);
    }
}