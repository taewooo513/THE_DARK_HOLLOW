using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateBase : IBossState
{
    protected readonly BossController ctx;
    protected readonly BossStateMachine fsm;
    protected bool locked;
    public bool IsLocked => locked;
    public virtual string Name => GetType().Name;

    protected BossStateBase(BossController c, BossStateMachine f) { ctx = c; f = fsm = f; }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Tick(float dt) { }
    public virtual void FixedTick(float fdt) { }
    public virtual void HandleEvent(EventType evt, Component sender, object param = null) { }
}
