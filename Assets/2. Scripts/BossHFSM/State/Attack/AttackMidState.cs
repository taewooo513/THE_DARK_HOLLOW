using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ─────────────────────────────────────────────────────────────
// Attack: 중거리 공격(Mid)
// 파이프라인: Windup -> Execute(dash move) -> Recover
// ─────────────────────────────────────────────────────────────
public class AttackMidState : AttackSuper
{
    public AttackMidState(BossController c, BossStateMachine f) : base(c, f) { }
    public override string Name => "Attack.Mid";

    protected override IBossState DefaultSub() => new A_Windup(ctx, fsm, this);
    class A_Windup : BossStateBase
    {
        readonly AttackMidState sup; float t;
        public A_Windup(BossController c, BossStateMachine f, AttackMidState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            t = 0;
            sup.locked = true;
            ctx.StopMove();
            SoundManager.Instance.PlayEFXSound("BossRangedAttack_EFX");
            ctx.AnimatianPlay_Trigger("SetMidAttack");
        }
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.midWindup) sup.ChangeSub(new A_Execute(ctx, fsm, sup));
        }
    }
    class A_Execute : BossStateBase
    {
        readonly AttackMidState sup; 
        public A_Execute(BossController c, BossStateMachine f, AttackMidState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            ctx.FaceToPlayerFireMidAttack();
            ctx.OnMidAttackEffect();
        }
        public override void Tick(float dt)
        {
            sup.ChangeSub(new A_Recover(ctx, fsm, sup));
        }
    }
    class A_Recover : BossStateBase
    {
        readonly AttackMidState sup; 
        float t;
        public A_Recover(BossController c, BossStateMachine f, AttackMidState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            t = 0;
        }
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.midRecover)
            {
                sup.locked = false;
                ctx.StartCD_Mid();
                if (!ctx.CanSeePlayer()) 
                    fsm.Change(ctx.SIdle);
                else
                    fsm.Change(ctx.SChoose);
            }
        }
    }



}
