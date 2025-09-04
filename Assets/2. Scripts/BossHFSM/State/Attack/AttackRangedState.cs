using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ��������������������������������������������������������������������������������������������������������������������������
// Attack: ���Ÿ�(Ranged)
// ����������: Windup -> Execute(Fire once) -> Recover
// ��������������������������������������������������������������������������������������������������������������������������
public class AttackRangedState : AttackSuper
{
    public AttackRangedState(BossController c, BossStateMachine f) : base(c, f) { }
    public override string Name => "Attack.Ranged";
    protected override IBossState DefaultSub() => new A_Windup(ctx, fsm, this);

    class A_Windup : BossStateBase
    {
        readonly AttackRangedState sup; 
        float t;
        public A_Windup(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter() 
        { 
            t = 0; 
            sup.locked = true; 
            ctx.StopMove(); 
            ctx.AnimationPlay_Trigger("SetRangedAttack");
            SoundManager.Instance.PlayEFXSound("BatSkill_EFX");
        }
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.rangedWindup) sup.ChangeSub(new A_Execute(ctx, fsm, sup));
        }
    }

    class A_Execute : BossStateBase
    {
        readonly AttackRangedState sup; 
        bool fired;
        public A_Execute(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            fired = false;
        }
        public override void Tick(float dt)
        {
            if (!fired)
            {
                fired = true;
                ctx.FireProjectile(); // ����ü 1��
            }
            // ����ü���̸� Active=0���� �ΰ� ��� ��Ŀ����
            sup.ChangeSub(new A_Recover(ctx, fsm, sup));
        }
    }

    class A_Recover : BossStateBase
    {
        readonly AttackRangedState sup; 
        float t;
        public A_Recover(BossController c, BossStateMachine f, AttackRangedState s) : base(c, f) { sup = s; }
        public override void OnEnter() 
        { 
            t = 0; 
        }
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.rangedRecover)
            {
                sup.locked = false;
                ctx.StartCD_Ranged();
                if (!ctx.CanSeePlayer()) 
                    fsm.Change(ctx.SIdle);
                else 
                    fsm.Change(ctx.SChoose);
            }
        }
    }
}
