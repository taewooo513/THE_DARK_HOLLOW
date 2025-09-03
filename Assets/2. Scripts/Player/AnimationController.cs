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
}
