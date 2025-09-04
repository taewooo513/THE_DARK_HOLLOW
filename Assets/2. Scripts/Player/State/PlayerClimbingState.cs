using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Climbing State");

    }

    public override void UpdateState(StateMachine stateMachine)
    {

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
}
