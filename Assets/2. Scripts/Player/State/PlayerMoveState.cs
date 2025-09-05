using UnityEngine;

// 움직이는 역할 
public class PlayerMoveState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Move State");
        this.playerController = stateMachine.PlayerController;

        playerController.Rigid.gravityScale = 1.0f;

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);

        playerController.GetComponentInChildren<SpriteRenderer>().flipX = playerController.MovementInput.x < 0 ? true : false;

        // Move Animation
        playerController.AnimationController.Run(Vector2.zero);
        playerController.AnimationController.Move(playerController.MovementInput);

        ChangeSpeed();
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 보스에게 맞았으면
        if (playerController.IsHit)
        {
            // Hit상태로 진입 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
        }

        // 이동 중 공격키(x)를 누르면 
        if (Input.GetKeyDown(KeyCode.X))
        {
            // 공격 상태로 전환 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Attack));
        }
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

        Debug.Log($"벨로시티 = {playerController.MovementInput}");
    }

    private void ChangeSpeed()
    {
        CharacterManager.instance.PlayerStat.SpeedModifier = 1.0f;
        //this.playerController.SpeedModifier = 3.0f;
    }
}
