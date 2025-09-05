using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 행동후 후딜 처리 상태
public class RecoverState : BossStateBase
{
    public RecoverState(BossController c, BossStateMachine f) : base(c, f) { }
    float t; const float dur = 0.3f;
    public override void OnEnter() 
    {
        t = 0; ctx.StopMove();
    }
    public override void Tick(float dt)
    {
        t += dt;
        if (t >= dur)
        {
            if (!ctx.CanSeePlayer()) fsm.Change(ctx.SIdle);
            else if (ctx.Dist <= ctx.stat.nearRange || ctx.Dist >= ctx.stat.farRange) fsm.Change(ctx.SChoose);
        }
    }
}
