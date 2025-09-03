using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseState : BossStateBase
{
    public ChooseState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        if (!ctx.CanSeePlayer()) 
        { fsm.Change(ctx.SIdle); return; }

        bool nearOK = ctx.Dist <= ctx.stat.nearRange && ctx.CDReadyDash();
        bool farOK = ctx.Dist >= ctx.stat.farRange && ctx.CDReadyRanged();

        // 우선순위: 근접 우선 → 원거리
        if (nearOK) { fsm.Change(ctx.SAtkDash); return; }
        if (farOK) { fsm.Change(ctx.SAtkRng); return; }

        // 둘 다 아니면 다시 추격
        fsm.Change(ctx.SIdle);
    }
}
