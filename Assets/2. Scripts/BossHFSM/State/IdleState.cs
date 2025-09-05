using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossStateBase
{
    public IdleState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        ctx.StopMove();
    }
    public override void Tick(float dt)
    {
        if (ctx.CanSeePlayer() && ctx.DecisionReady())
            fsm.Change(ctx.SChoose);
    }
}
