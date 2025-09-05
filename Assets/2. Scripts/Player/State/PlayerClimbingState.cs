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

        // ���� ���� ���� 
        stateMachine.SetPreState(stateMachine);

        // Climb Animation
        playerController.AnimationController.Climb();
        //playerController.AnimationController.Run(Vector2.zero);
        //playerController.AnimationController.Move(playerController.MovementInput);

        // �߷� ����
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
        Debug.Log("��ٸ� Ÿ�� ���� ����");
        // �̵� ���� 
        //playerController.MovementDirection = new Vector2(0, playerController.MovementInput.y);
        playerController.MovementDirection = playerController.MovementInput;

        // �̵� �ӵ� 
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // �̵� ó��
        playerController.Rigid.velocity = playerController.MovementDirection;

        //// �߷� ���� ������. 
        ////Debug.Log($"{playerController.MovementDirection}");


        //// �̵� ���� ����
        //playerController.MovementDirection = playerController.MovementInput;

        //// �̵� �ӵ� ����
        //playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        //Debug.Log($"{playerController.MovementDirection}");

        ////// �߷��� velocity.y������ ����
        ////Vector2 dir = playerController.MovementDirection;
        ////dir.y = playerController.Rigid.velocity.y;
        ////playerController.MovementDirection = dir;
        //////playerController.MovementDirection.y = playerController.Rigid.velocity.y;



        //// �̵� ó��
        //playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
