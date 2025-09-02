using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BossStateBase
{
    public DeadState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        ctx.StopMove();
        //ctx.Play("Dead"); // TODO
        var col = ctx.GetComponent<Collider2D>(); if (col) col.enabled = false;
        ctx.ToDie();
    }
}
