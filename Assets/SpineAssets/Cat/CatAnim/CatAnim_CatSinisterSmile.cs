using UnityEngine;

public class CatAnim_CatSinisterSmile : StateMachineBehaviour
{
    private CatSkin catSkin;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        catSkin = animator.gameObject.GetComponent<CatSkin>();
        catSkin.SetSinisterSmile();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var cat = animator.gameObject.GetComponent<Cat>();
        catSkin.ChangeSkin(cat.cloudCatData);
    }
}
