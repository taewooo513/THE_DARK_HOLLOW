using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello From The Hit State");
    }

    public override void FixedUpdateState(StateManager player)
    {

    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void UpdateState(StateManager player)
    {

    }
}
