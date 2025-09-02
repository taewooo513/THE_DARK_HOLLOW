using UnityEngine;

// 움직이는 역할 
public class PlayerMoveState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateManager stateManager)
    {
        Debug.Log("Hello from the Move State");
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
        Debug.Log("(FixedUpdateState): Player Move!!");
        Move();
    }

    private void Move()
    {
        // 이동 방향 설정
        playerController.MovementDirection = playerController.MovementInput;

        // 이동 속도 설정
        playerController.MovementDirection *= (playerController.MoveSpeed * playerController.SpeedModifier);

        // 중력은 velocity.y값으로 설정
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;
        //playerController.MovementDirection.y = playerController.Rigid.velocity.y;

        // 이동 처리
        playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
