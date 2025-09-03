using UnityEngine;

// 움직이는 역할 
public class PlayerMoveState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Move State");
        this.playerController = stateMachine.PlayerController;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        //Debug.Log("(FixedUpdateState): Player Move!!");
        Move();
    }

    private void Move()
    {
        // 이동 방향 설정
        playerController.MovementDirection = playerController.MovementInput;

        // 이동 속도 설정
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // 중력은 velocity.y값으로 설정
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;
        //playerController.MovementDirection.y = playerController.Rigid.velocity.y;

        // 이동 처리
        playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
