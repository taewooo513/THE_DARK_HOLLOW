using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ─────────────────────────────────────────────────────────────
// Attack: 원거리(Ranged)
// 파이프라인: Windup -> Execute(Fire once) -> Recover
// ─────────────────────────────────────────────────────────────
public class AttackRangedState : AttackSuper
{
    public AttackRangedState(BossController c, BossStateMachine f) : base(c, f) { }
    public override string Name => "Attack.Ranged";
    protected override IBossState DefaultSub() => new A_Windup(ctx, fsm, this);

    class A_Windup : BossStateBase
    {
        readonly AttackRangedState sup; float t;
        public A_Windup(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter() 
        { 
            t = 0; 
            sup.locked = true; 
            ctx.StopMove(); 
            //ctx.Play("Shoot_Windup"); 
        }
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.rangedWindup) sup.ChangeSub(new A_Execute(ctx, fsm, sup));
        }
    }

    class A_Execute : BossStateBase
    {
        readonly AttackRangedState sup; bool fired;
        public A_Execute(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            fired = false;
            ctx.FaceToPlayer();
          //  ctx.Play("Shoot"); // TODO
        }
        public override void Tick(float dt)
        {
            if (!fired)
            {
                fired = true;
                ctx.FireProjectile(); // 투사체 1발
            }
            // 투사체형이면 Active=0으로 두고 즉시 리커버리
            sup.ChangeSub(new A_Recover(ctx, fsm, sup));
        }
    }

    class A_Recover : BossStateBase
    {
        readonly AttackRangedState sup; float t;
        public A_Recover(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter() 
        { 
            t = 0; 
            //ctx.Play("Shoot_Recover"); 
        }
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.rangedRecover)
            {
                sup.locked = false;
                ctx.StartCD_Ranged();
                if (!ctx.CanSeePlayer()) fsm.Change(ctx.SIdle);
                else if (ctx.Dist <= ctx.stat.nearRange || ctx.Dist >= ctx.stat.farRange) fsm.Change(ctx.SChoose);
                else fsm.Change(ctx.SChase);
            }
        }
    }
}
