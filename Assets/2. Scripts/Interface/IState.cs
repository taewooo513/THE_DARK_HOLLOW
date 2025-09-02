using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter();
    void OnExit();
    void Tick(float dt);
    void FixedTick(float fdt);
    void HandleEvent(EventType evt, Component sender, object param = null);
    bool IsLocked { get; }
    string Name { get; }
}
