using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : BaseState
{
    public override void EnterState(StateManager stateManager)
    {
        Debug.Log("Hello From The Hit State");
    }

    public override void FixedUpdateState(StateManager stateManager)
    {

    }

    public override void OnCollisionEnter(StateManager stateManager, Collision2D collision)
    {

    }

    public override void UpdateState(StateManager stateManager)
    {

    }
}
