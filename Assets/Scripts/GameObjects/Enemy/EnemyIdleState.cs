using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animatorStateInfo.IsName("Idle"))
        {
            animator.ResetTrigger("OnMeleeAttack1");
            animator.ResetTrigger("OnMeleeAttack2");
            animator.ResetTrigger("Airborne");
            animator.ResetTrigger("Hit");
        }
    }
}
