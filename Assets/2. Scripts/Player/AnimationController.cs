using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Move Animation
    public void Move(Vector2 dir)
    {
        animator.SetBool(Constants.AnimationParameter.MOVE, dir.magnitude > .5f);
    }

    // Run Animation
    public void Run(Vector2 dir)
    {
        animator.SetBool(Constants.AnimationParameter.RUN, dir.magnitude > .5f);
    }

    // Hit Animation
    //public void Hit()
    //{
    //    // 한번만 실행 
    //    animator.SetBool(Constants.AnimationParameter.HIT, true);
    //}

    // Attack Animation
    public void Attack()
    {
        //animator.SetBool(Constants.AnimationParameter.ATTACK, true);
        animator.SetTrigger(Constants.AnimationParameter.ATTACK);
    }

    //// Cancel Attack Animation
    //public void CancelAttack()
    //{
    //    animator.SetBool(Constants.AnimationParameter.ATTACK, false);
    //}

    // Special Attack Animation
    public void SpecialAttack()
    {
        animator.SetTrigger(Constants.AnimationParameter.SPECIALATTACK);
    }

    public void Climb()
    {
        animator.SetTrigger(Constants.AnimationParameter.CLIMB);
    }

    public void Climb(bool flag)
    {
        animator.SetBool(Constants.AnimationParameter.CLIMB, flag);
    }

    public void Idle()
    {
        animator.SetTrigger(Constants.AnimationParameter.IDLE);
    }
}
