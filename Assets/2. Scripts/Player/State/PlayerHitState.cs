using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : BaseState
{
    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Hit State");
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {

    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void UpdateState(StateMachine stateMachine)
    {

    }
}
