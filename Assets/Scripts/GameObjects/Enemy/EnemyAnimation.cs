using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator EnemyAnimator;

    private void Awake()
    {
        if (!EnemyAnimator)
            EnemyAnimator = gameObject.GetComponentInChildren<Animator>();
        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMeleeAttack1()
    {
        EnemyAnimator.SetTrigger("OnMeleeAttack1");
    }

    public void OnMeleeAttack2()
    {
        EnemyAnimator.SetTrigger("OnMeleeAttack2");
    }

    public void OnRangeAttack1()
    {
        EnemyAnimator.SetTrigger("OnRangeAttack1");
    }

    public void OnRangeAttack2()
    {
        EnemyAnimator.SetTrigger("OnRangeAttack2");
    }

    public void OnHit()
    {
        EnemyAnimator.SetTrigger("Hit");
    }

    public void OnDeath()
    {
        EnemyAnimator.SetTrigger("Death");
    }

    public void OnAirborne()
    {
        EnemyAnimator.SetTrigger("Airborne");
    }

    public void SetGround(bool isGround)
    {
        EnemyAnimator.SetBool("Ground", isGround);
    }

    public void SetWalk(bool isWalk)
    {
        EnemyAnimator.SetBool("Walk", isWalk);
    }

    public bool IsMoveAvailable()
    {
        var stateInfo = EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Idle") || stateInfo.IsName("Walk") || stateInfo.IsName("Run");
    }

    public bool IsDirectionChangeAvailable()
    {
        var stateInfo = EnemyAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Idle") || stateInfo.IsName("Walk") || stateInfo.IsName("Run");
    }

    public void Dead()
    {
        transform.parent.SendMessage("Dead");
    }

    public float GetAnimationDuration(string ClipName)
    {
        AnimationClip[] animationClips = EnemyAnimator.runtimeAnimatorController.animationClips;

        for (int idx = animationClips.Length; idx-- > 0;)
        {
            if(animationClips[idx].name == ClipName)
            {
                return animationClips[idx].length;
            }
        }
        return 0f;
    }
}
