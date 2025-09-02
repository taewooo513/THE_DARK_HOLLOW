using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 상위(Composite) 상태: 하위 파이프라인(Windup→Execute→Recover) 관리
public abstract class AttackSuper : BossStateBase
{
    protected IBossState sub;
    protected AttackSuper(BossController c, BossStateMachine f) : base(c, f) { }

    protected abstract IBossState DefaultSub();
    protected void ChangeSub(IBossState next) { sub?.OnExit(); sub = next; sub?.OnEnter(); }
    public override void OnEnter() { ChangeSub(DefaultSub()); }
    public override void OnExit() { sub?.OnExit(); sub = null; }
    public override void Tick(float dt) { sub?.Tick(dt); }
    public override void FixedTick(float fdt) { sub?.FixedTick(fdt); }

    // 공격 도중(락)에는 외부 이벤트로도 전이를 막고 싶으면 locked 유지,
    // 인식 범위 이탈은 컨트롤러에서 강제 Idle 처리(Force)로 해결.
}
