using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Climbing State");
        this.playerController = stateMachine.PlayerController;

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);

        // Climb Animation
        playerController.AnimationController.Climb();
        //playerController.AnimationController.Run(Vector2.zero);
        //playerController.AnimationController.Move(playerController.MovementInput);

        // 중력 제거
        playerController.Rigid.gravityScale = 0.0f;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        
        Move();
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    private void Move()
    {
        Debug.Log("사다리 타고 가고 있음");
        // 이동 방향 
        //playerController.MovementDirection = new Vector2(0, playerController.MovementInput.y);
        playerController.MovementDirection = playerController.MovementInput;

        // 이동 속도 
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // 이동 처리
        playerController.Rigid.velocity = playerController.MovementDirection;

        //// 중력 없는 움직임. 
        ////Debug.Log($"{playerController.MovementDirection}");


        //// 이동 방향 설정
        //playerController.MovementDirection = playerController.MovementInput;

        //// 이동 속도 설정
        //playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        //Debug.Log($"{playerController.MovementDirection}");

        ////// 중력은 velocity.y값으로 설정
        ////Vector2 dir = playerController.MovementDirection;
        ////dir.y = playerController.Rigid.velocity.y;
        ////playerController.MovementDirection = dir;
        //////playerController.MovementDirection.y = playerController.Rigid.velocity.y;



        //// 이동 처리
        //playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
