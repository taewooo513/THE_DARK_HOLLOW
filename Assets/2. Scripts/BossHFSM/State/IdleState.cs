using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossStateBase
{
    public IdleState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        ctx.StopMove();
       //ctx.AnimationPlay_Trigger("Idle"); // �ִ� Ʈ���Ÿ� ���߱�
    }
    public override void Tick(float dt)
    {
        if (ctx.CanSeePlayer())
            fsm.Change(ctx.SChoose);
    }
}
