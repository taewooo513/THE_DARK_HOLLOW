using UnityEngine;

public class PlayerJumpState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Jump State");
        this.playerController = stateMachine.PlayerController;

        playerController.Rigid.gravityScale = 1.0f;

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 보스에게 맞았으면
        if (playerController.IsHit)
        {
            // Hit상태로 진입 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
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
        Jump(stateMachine);
    }

    private void Jump(StateMachine stateMachine)
    {
        //stateMachine.SetStateBeforeJump(stateMachine);
        

        if (playerController.CanJump)
        {
            playerController.Rigid.AddForce(Vector2.up * CharacterManager.instance.PlayerStat.JumpPower, ForceMode2D.Impulse);
            playerController.CanJump = false;
        }
        else
        {
            // 점프 중

            // arrow left/right를 입력한 경우 -> 이동처리 
            if (stateMachine.PlayerController.IsMoving)
            {
                // 원본
                //Debug.Log($"현재 상태는 = {stateMachine.GetCurrentState()}");
                //stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));

                // 수정1
                //// 걷기 중 일때
                //if (stateMachine.GetCurrentState().Equals(Constants.State.MOVE))
                //{
                //    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
                //}

                //// 달리기 중 일때 
                //else if (stateMachine.GetCurrentState().Equals(Constants.State.RUN))
                //{
                //    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
                //}

                // 수정2
                // 이전 상태가 걷기 상태면
                if (stateMachine.GetPreState().Equals(Constants.State.MOVE))
                {
                    //// 이전 상태 저장
                    //stateMachine.SetPreState(stateMachine.GetPreState());

                    // 계속 걷기 상태를 유지 
                    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
                }

                // 이전 상태가 달리기 상태면 
                else if (stateMachine.GetPreState().Equals(Constants.State.RUN))
                {
                    //// 이전 상태 저장
                    //stateMachine.SetPreState(stateMachine.GetPreState());

                    // 계속 달리기 상태를 유지 
                    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
                }

            }

            // 땅에 닿으면 -> Idle 상태로 전환
            if (stateMachine.PlayerController.IsGrounded())
            {
                //Debug.Log("점프후 땅에 착지함.");

                //Debug.Log($"이전 상태({stateMachine.GetPreState()}를 저장한다.");
                //// 추가 
                //stateMachine.SetPreState(stateMachine.GetPreState());
                //Debug.Log($"이전 상태({stateMachine.GetPreState()}");

                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            }
        }
    }
}
