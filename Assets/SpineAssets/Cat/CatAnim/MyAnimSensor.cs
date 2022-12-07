using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimSensor : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Cat cat = animator.GetComponent<Cat>();

        //cat.OpenBigGame();
    }
}
