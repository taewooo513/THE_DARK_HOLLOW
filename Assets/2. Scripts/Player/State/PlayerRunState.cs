using UnityEngine;

public class PlayerRunState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Run State");
        this.playerController = stateMachine.PlayerController;

        playerController.Rigid.gravityScale = 1.0f;

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);

        ChangeSpeed();

        playerController.GetComponentInChildren<SpriteRenderer>().flipX = playerController.MovementInput.x < 0 ? true : false;

        // Run Animation
        playerController.AnimationController.Move(Vector2.zero);
        playerController.AnimationController.Run(playerController.MovementInput);
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 보스에게 맞았으면
        if (playerController.IsHit)
        {
            // Hit상태로 진입 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
        }

        // 달리기 키를 떼면

    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        //Debug.Log("(FixedUpdateState): Player Run!!");
        Run();
    }

    private void ChangeSpeed()
    {
        CharacterManager.Instance.PlayerStat.SpeedModifier = CharacterManager.Instance.PlayerStat.SpeedModifierInput;
        //this.playerController.SpeedModifier = playerController.SpeedModifierInput;
    }

    private void Run()
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
