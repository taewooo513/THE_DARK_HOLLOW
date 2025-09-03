using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_01 : Enemy
{
    public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        SwitchState(EnemyState.Idle);
    }
    private void Update()
    {

    }
    public virtual void Attack()
    {

    }

    public virtual void Damaged()
    {

    }
    public virtual void Movement()
    {

    }
    public virtual void Die()
    {
        animator.SetBool("IsDie", true);
    }

    private bool IsAttack()
    {
        return false;
    }
}
