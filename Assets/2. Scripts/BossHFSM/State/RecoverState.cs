using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverState : BossStateBase
{
    public RecoverState(BossController c, BossStateMachine f) : base(c, f) { }
    float t; const float dur = 0.3f;
    public override void OnEnter() { t = 0; ctx.StopMove(); ctx.Play("Recover"); } // TODO
    public override void Tick(float dt)
    {
        t += dt;
        if (t >= dur)
        {
            if (!ctx.CanSeePlayer()) fsm.Change(ctx.SIdle);
            else if (ctx.Dist <= ctx.stat.nearRange || ctx.Dist >= ctx.stat.farRange) fsm.Change(ctx.SChoose);
            else fsm.Change(ctx.SChase);
        }
    }
}
