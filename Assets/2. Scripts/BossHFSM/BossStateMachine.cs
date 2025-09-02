using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BossStateMachine
{
    public IBossState Current { get; private set; }

    public void Change(IBossState next, string reason = null)
    {
        if (next == null || next == Current) return;
        if (Current != null && Current.IsLocked && reason != "Force") return;
        Current?.OnExit();
        Current = next;
        Current?.OnEnter();
    }

    public void Tick(float dt) => Current?.Tick(dt);
    public void FixedTick(float fdt) => Current?.FixedTick(fdt);
    public void Send(EventType e, Component s, object p = null) => Current?.HandleEvent(e, s, p);
}
