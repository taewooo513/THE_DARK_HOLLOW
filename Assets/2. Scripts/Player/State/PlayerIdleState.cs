using UnityEngine;

public class PlayerIdleState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Idle State");
        this.playerController = stateMachine.PlayerController;

        playerController.Rigid.gravityScale = 1.0f;

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);

        // Idle Animation
        playerController.AnimationController.Move(Vector2.zero);
        playerController.AnimationController.Run(Vector2.zero);
        //playerController.AnimationController.Idle();
        playerController.AnimationController.Climb(false);

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

        // 추가
        // 방향키가 계속 눌려져 있으면
        if (playerController.IsMoving)
        {
            // 이전 상태가 Jump이고, 점프 이전 상태가 Run이면 
            if (stateMachine.GetPreState().ToString().Equals(Constants.State.JUMP) &&
                stateMachine.GetStateBeforeJump().ToString().Equals(Constants.State.RUN))
            {
                // 달리기 상태로 전환
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
            }

            // 이전 상태가 Jump이고, 점프 이전 상태가 Move이면 
            else if (stateMachine.GetPreState().ToString().Equals(Constants.State.JUMP) &&
                stateMachine.GetStateBeforeJump().ToString().Equals(Constants.State.MOVE))
            {
                // 걷기 상태로 전환
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
            }
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

    }

    private void ChangeSpeed()
    {
        CharacterManager.instance.PlayerStat.SpeedModifier = 1.0f;
    }
}
