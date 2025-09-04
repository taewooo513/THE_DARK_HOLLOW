using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseState : BossStateBase
{
    public ChooseState(BossController c, BossStateMachine f) : base(c, f) { }
    enum Zone 
    { 
        Outside, 
        Blue, 
        Red, 
        Green 
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
            case Zone.Green: // Dash > Mid > Ranged
                if (ctx.CDReadyDash()) { ctx.lastChosen = BossController.AttackChoice.Dash; fsm.Change(ctx.SAtkDash); return; }
                if (ctx.CDReadyMid()) { ctx.lastChosen = BossController.AttackChoice.Mid; fsm.Change(ctx.SAtkMid); return; }
                if (ctx.CDReadyRanged()) { ctx.lastChosen = BossController.AttackChoice.Ranged; fsm.Change(ctx.SAtkRng); return; }
                break;

            case Zone.Red:   // Mid > Ranged
                if (ctx.CDReadyMid()) { ctx.lastChosen = BossController.AttackChoice.Mid; fsm.Change(ctx.SAtkMid); return; }
                if (ctx.CDReadyRanged()) { ctx.lastChosen = BossController.AttackChoice.Ranged; fsm.Change(ctx.SAtkRng); return; }
                break;

            case Zone.Blue:  // Ranged only
                if (ctx.CDReadyRanged()) { ctx.lastChosen = BossController.AttackChoice.Ranged; fsm.Change(ctx.SAtkRng); return; }
                break;

            case Zone.Outside:
                fsm.Change(ctx.SIdle); return;
        }

        // 허용 패턴이 모두 쿨다운이면 대기(경계 유지)
        fsm.Change(ctx.SIdle);
    }
    static Zone GetZone(float d, BossStat s)
    {
        if (d <= s.nearRange) return Zone.Green;
        if (d <= s.midRange) return Zone.Red;
        if (d <= s.farRange) return Zone.Blue;
        return Zone.Outside;
    }
}
