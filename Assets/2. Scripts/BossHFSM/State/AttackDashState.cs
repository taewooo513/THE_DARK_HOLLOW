using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ─────────────────────────────────────────────────────────────
// Attack: 돌진(Dash)
// 파이프라인: Windup -> Execute(dash move) -> Recover
// ─────────────────────────────────────────────────────────────
public class AttackDashState : AttackSuper
{
    public AttackDashState(BossController c, BossStateMachine f) : base(c, f) { }
    public override string Name => "Attack.Dash";

    protected override IBossState DefaultSub() => new A_Windup(ctx, fsm, this);

    class A_Windup : BossStateBase
    {
        readonly AttackDashState sup; float t;
        public A_Windup(BossController c, BossStateMachine f, AttackDashState s) : base(c, f) { sup = s; }
        public override void OnEnter() { t = 0; sup.locked = true; ctx.StopMove(); ctx.Play("Dash_Windup"); } // TODO
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.dashWindup) sup.ChangeSub(new A_Execute(ctx, fsm, sup));
        }
    }

    class A_Execute : BossStateBase
    {
        readonly AttackDashState sup; float t; Vector2 dir;
        public A_Execute(BossController c, BossStateMachine f, AttackDashState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            t = 0;
            ctx.FaceToPlayer();
            if (ctx.player) dir = (ctx.player.position - ctx.transform.position).normalized;
            else dir = new Vector2(ctx.transform.localScale.x, 0f);
            ctx.Play("Dash"); // TODO
        }
        public override void Tick(float dt)
        {
            t += dt;
            // 이동(가속/감속 원하면 보간)
            ctx.rb.velocity = dir * ctx.stat.dashSpeed;
            if (t >= ctx.stat.dashActive)
            {
                ctx.StopMove();
                sup.ChangeSub(new A_Recover(ctx, fsm, sup));
            }
        }
    }

    class A_Recover : BossStateBase
    {
        readonly AttackDashState sup; float t;
        public A_Recover(BossController c, BossStateMachine f, AttackDashState s) : base(c, f) { sup = s; }
        public override void OnEnter() { t = 0; ctx.Play("Dash_Recover"); } // TODO
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.dashRecover)
            {
                sup.locked = false;
                ctx.StartCD_Dash();
                // 다음으로
                if (!ctx.CanSeePlayer()) fsm.Change(ctx.SIdle);
                else if (ctx.Dist <= ctx.stat.nearRange || ctx.Dist >= ctx.stat.farRange) fsm.Change(ctx.SChoose);
                else fsm.Change(ctx.SChase);
            }
        }
    }
}
