using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseState : BossStateBase
{
    public ChooseState(BossController c, BossStateMachine f) : base(c, f) { }
    enum Zone 
    { 
        Outside,
        NearRange,
        MidRange,
        FarRange, 
    }
    public override void OnEnter()
    {
        if (!ctx.CanSeePlayer()) 
        { 
            fsm.Change(ctx.SIdle); 
            return; 
        }
        if (!ctx.DecisionReady()) 
        { 
            fsm.Change(ctx.SIdle); 
            return; 
        }
        float d = ctx.Dist;
        var s = ctx.stat;
        Zone z = GetZone(d, s);
        switch (z)
        {
            case Zone.NearRange: // Dash > Mid > Ranged
                if (ctx.CDReadyDash()) { ctx.lastChosen = BossController.AttackChoice.Dash; fsm.Change(ctx.SAtkDash); return; }
                if (ctx.CDReadyMid()) { ctx.lastChosen = BossController.AttackChoice.Mid; fsm.Change(ctx.SAtkMid); return; }
                if (ctx.CDReadyRanged()) { ctx.lastChosen = BossController.AttackChoice.Ranged; fsm.Change(ctx.SAtkRng); return; }
                break;

            case Zone.MidRange:   // Mid > Ranged
                if (ctx.CDReadyMid()) { ctx.lastChosen = BossController.AttackChoice.Mid; fsm.Change(ctx.SAtkMid); return; }
                if (ctx.CDReadyRanged()) { ctx.lastChosen = BossController.AttackChoice.Ranged; fsm.Change(ctx.SAtkRng); return; }
                break;

            case Zone.FarRange:  // Ranged only
                if (ctx.CDReadyRanged()) { ctx.lastChosen = BossController.AttackChoice.Ranged; fsm.Change(ctx.SAtkRng); return; }
                break;

            case Zone.Outside:
                fsm.Change(ctx.SIdle); return;
        }

        // 허용 패턴이 모두 쿨다운이면 대기(경계 유지)
        ctx.StartDecisionDelay();
        fsm.Change(ctx.SIdle);
    }
    static Zone GetZone(float d, BossStat s)
    {
        if (d <= s.nearRange) return Zone.NearRange;
        if (d <= s.midRange) return Zone.MidRange;
        if (d <= s.farRange) return Zone.FarRange;
        return Zone.Outside;
    }
}
