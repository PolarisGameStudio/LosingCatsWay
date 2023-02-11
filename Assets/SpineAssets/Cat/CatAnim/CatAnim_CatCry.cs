using UnityEngine;

public class CatAnim_CatCry : StateMachineBehaviour
{
    private CatSkin catSkin;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        catSkin = animator.gameObject.GetComponent<CatSkin>();
        catSkin.SetCry();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        catSkin.OpenFace();
    }
}
