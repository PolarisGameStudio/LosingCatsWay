using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnim_SpecialSpine : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Cat cat = animator.gameObject.GetComponent<Cat>();
        cat.specialSpineRoom.spcialSpineIsUse = false;
    }
}
