using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossStateBase
{
    public ChaseState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        //ctx.Play("Run"); // 애니 트리거명 맞추기
    }
    public override void Tick(float dt)
    {
        if (!ctx.CanSeePlayer()) { fsm.Change(ctx.SIdle); return; }

        float d = ctx.GetComponent<BossStat>().nearRange; 
        float far = ctx.GetComponent<BossStat>().farRange;

        // 사거리(근접/원거리) 임계에 닿으면 선택으로
        if (ctx.CanSeePlayer() && (ctx.Dist <= d || ctx.Dist >= far))
        { fsm.Change(ctx.SChoose); return; }

        // 추격 이동
        if (ctx.player) ctx.MoveTowards(ctx.player.position, ctx.stat.moveSpeed);
        ctx.FaceToPlayer();
    }
    public override void OnExit() 
    { 
        ctx.StopMove(); 
    }
}
