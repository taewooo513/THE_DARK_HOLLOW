using UnityEngine;

public class PlayerJumpState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateManager stateManager)
    {
        Debug.Log("Hello from the Jump State");
        this.playerController = stateManager.PlayerController;
    }

    public override void UpdateState(StateManager stateManager)
    {

    }

    public override void OnCollisionEnter(StateManager stateManager, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager stateManager)
    {
        Debug.Log("FixedUpdateState : Jump()");
        Jump();
    }

    private void Jump()
    {
        if (playerController.CanJump)
        {
            playerController.Rigid.AddForce(Vector2.up * playerController.JumpPower, ForceMode2D.Impulse);
            playerController.CanJump = false;
        }
    }
}
