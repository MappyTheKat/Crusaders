using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(animatorStateInfo.IsName("Idle"))
        {
            animator.ResetTrigger("Airborne");
            animator.ResetTrigger("ZPressed");
            animator.ResetTrigger("XPressed");
            animator.ResetTrigger("CPressed");
            animator.ResetTrigger("SpacePressed");
            animator.ResetTrigger("Hit");
        }
    }
}
