using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator PlayerAnimator;

    private void Awake()
    {
        if (!PlayerAnimator)
            PlayerAnimator = gameObject.GetComponentInChildren<Animator>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Idle로 돌아올때 소비 안된 트리거가 남아있는걸 어떻게 치우고 싶은데.
        // PlayerAnimator.ResetTrigger
    }

    public void OnZKeyPressed()
    {
        PlayerAnimator.SetTrigger("ZPressed");
    }

    public void OnXKeyPressed()
    {
        PlayerAnimator.SetTrigger("XPressed");
    }

    public void OnCKeyPressed()
    {
        PlayerAnimator.SetTrigger("CPressed");
    }

    public void OnSpaceKeyPressed()
    {
        PlayerAnimator.SetTrigger("SpacePressed");
    }

    public void OnHit()
    {
        PlayerAnimator.SetTrigger("Hit");
    }

    public void OnDeath()
    {
        PlayerAnimator.SetTrigger("Death");
    }

    public void OnAirborne()
    {
        PlayerAnimator.SetTrigger("Airborne");
    }

    public void SetWalk(bool isWalk)
    {
        PlayerAnimator.SetBool("Walk", isWalk);
    }

    public void SetGround(bool isGround)
    {
        PlayerAnimator.SetBool("Ground", isGround);
    }

    public void Dead()
    {
        transform.parent.SendMessage("Dead");
    }

    public bool IsMoveAvailable()
    {
        var StateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        return StateInfo.IsName("Idle") || StateInfo.IsName("Walk") || StateInfo.IsName("Run") || StateInfo.IsName("Jump") || StateInfo.IsName("JumpAttack");
    }

    public bool IsDirectionChangeAvailable()
    {
        var StateInfo = PlayerAnimator.GetCurrentAnimatorStateInfo(0);
        return StateInfo.IsName("Idle")|| StateInfo.IsName("Walk") || StateInfo.IsName("Run");
    }
}
