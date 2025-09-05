using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : BaseState
{
    private PlayerController playerController;
    private CompositeCollider2D coll;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Climbing State");
        this.playerController = stateMachine.PlayerController;

        // 이전 상태 저장 
        stateMachine.SetPreState(stateMachine);

        // Climb Animation
        //playerController.AnimationController.Run(Vector2.zero);
        //playerController.AnimationController.Move(playerController.MovementInput);
        //playerController.AnimationController.Climb();
        playerController.AnimationController.Climb(true);

        // 중력 제거
        playerController.Rigid.gravityScale = 0.0f;

        playerController.PlayerStat.isMoved = false;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 콜라이더에서 벗어나면 Idle상태로 변환 
        if (!playerController.IsClimbable)
        {
            Debug.Log("아이들 상태로 전환");

            playerController.Rigid.gravityScale = 1.0f;

            playerController.PlayerStat.isMoved = true;

            coll.isTrigger = false;

            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        
        Move(stateMachine);
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    private void Move(StateMachine stateMachine)
    {
        Debug.Log("사다리 타고 가고 있음");
        // 이동 방향 
        playerController.MovementDirection = playerController.MovementInput;

        // 이동 속도 
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // 이동 처리
        playerController.Rigid.velocity = playerController.MovementDirection;


        // 사다리 타는 중 바닥에 닿고, 아래방향키를 누르면 
        if (playerController.IsGrounded() && Input.GetKey(KeyCode.LeftControl))
        {
            Debug.Log("사다리 타는 중 바닥에 닿음");

            playerController.IsClimbable = false;
        }

        Collider2D ceilingCollider = playerController.GetCeilingCollider();
        if (ceilingCollider != null)
        {
            Debug.Log("천장에 닿음");
            Debug.Log($"{ceilingCollider}");
            coll = ceilingCollider.GetComponent<CompositeCollider2D>();

            if (coll != null)
            {
                coll.isTrigger = true;
            }
        }
    }
}
