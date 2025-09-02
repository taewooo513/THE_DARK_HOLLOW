using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello from the Idle State");
    }

    public override void UpdateState(StateManager player)
    {
        
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {

    }
}
