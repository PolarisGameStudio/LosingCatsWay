using System.Collections;
using System.Collections.Generic;
using PolyNav;
using UnityEngine;

public class AngelCatAnim_Walk : StateMachineBehaviour
{
    private AngelCat angelCat;

    private DirectionChecker directionChecker;
    private PolyNavAgent polyNavAgent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        angelCat = animator.gameObject.GetComponent<AngelCat>();
        directionChecker = animator.GetComponent<DirectionChecker>();
        polyNavAgent = animator.GetComponent<PolyNavAgent>();

        polyNavAgent.maxSpeed = 1;

        bool isLeftRoom = animator.GetBool(CatAnimTable.IsLeftRoom.ToString());

        if (isLeftRoom)
        {
            angelCat.MoveToRandomRoom();
            return;
        }

        angelCat.RandomMoveAtRoom();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!directionChecker.CheckIsMoving())
        {
            animator.SetBool(CatAnimTable.IsCanExit.ToString(), true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(CatAnimTable.IsCanExit.ToString(), false);
    }
}
