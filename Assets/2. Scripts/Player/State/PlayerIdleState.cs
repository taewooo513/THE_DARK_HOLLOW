using UnityEngine;

public class PlayerIdleState : BaseState
{
    private PlayerController playerController; 

    public override void EnterState(StateManager stateManager)
    {
        Debug.Log("Hello from the Idle State");
        this.playerController = stateManager.PlayerController;

        ChangeSpeed();
    }

    public override void UpdateState(StateManager stateManager)
    {
        
    }

    public override void OnCollisionEnter(StateManager stateManager, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager stateManager)
    {

    }

    private void ChangeSpeed()
    {
        this.playerController.SpeedModifier = 1.0f;
    }
}
