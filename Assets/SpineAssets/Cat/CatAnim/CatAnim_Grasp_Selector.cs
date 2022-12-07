using UnityEngine;

public class CatAnim_Grasp_Selector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int randomIndex = Random.Range(0, 2);
        animator.SetInteger(CatAnimTable.GraspSelectIndex.ToString(), randomIndex);
    }
}
