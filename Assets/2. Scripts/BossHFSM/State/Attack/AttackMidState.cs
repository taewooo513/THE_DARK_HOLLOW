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
    public override string Name => "Attack.Dash";

    protected override IBossState DefaultSub() => new A_Windup(ctx, fsm, this);
    class A_Windup : BossStateBase
    {
        readonly AttackMidState sup; float t;
        public A_Windup(BossController c, BossStateMachine f, AttackMidState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
        }
        public override void Tick(float dt)
        {
        }
    }
    class A_Execute : BossStateBase
    {
        readonly AttackMidState sup; float t; Vector2 dir;
        public A_Execute(BossController c, BossStateMachine f, AttackMidState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {

        }
        public override void Tick(float dt)
        {

        }
    }
    class A_Recover : BossStateBase
    {
        readonly AttackRangedState sup; float t;
        public A_Recover(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {

        }
        public override void Tick(float dt)
        {
          
        }
    }



}
