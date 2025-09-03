using UnityEngine;

public class PlayerRunState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateManager stateManager)
    {
        Debug.Log("Hello from the Run State");
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
        this.playerController.SpeedModifier = playerController.SpeedModifierInput;
    }
}
