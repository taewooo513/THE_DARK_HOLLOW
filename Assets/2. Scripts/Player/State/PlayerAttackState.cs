using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : BaseState
{
    private PlayerController playerController;

    private float coolTime;
    private float lastAttackTime;
    private float lastInputTime;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Attack State");
        this.playerController = stateMachine.PlayerController;

        // ���� ��Ÿ�� ���� 
        coolTime = Constants.CoolTime.ATTACK;
        lastAttackTime = 0.0f;

        // ������ ä��� 
        playerController.PlayerStat.Gauge += 1;
        if(playerController.PlayerStat.Gauge > 5)
            playerController.PlayerStat.Gauge = 5;

        // OverlapBox ����
        Collider2D[] colliders;
        if (playerController.SpriteRenderer.flipX)
        {
            colliders = Physics2D.OverlapBoxAll(playerController.transform.position + new Vector3(-0.5f, 0.3f, 0), new Vector3(0.5f, 0.5f, 0.5f), 0); // �÷��̾� flipX�� ���� �ٸ��� 
        }
        else
        {
            colliders = Physics2D.OverlapBoxAll(playerController.transform.position + new Vector3(0.5f, 0.3f, 0), new Vector3(0.5f, 0.5f, 0.5f), 0); // �÷��̾� flipX�� ���� �ٸ��� 
        }
        foreach (Collider2D collider in colliders)
        {
            // �� ������ ó�� 
            if(collider.TryGetComponent(out Enemy enemy))
            {
                Debug.Log("���� ����");
                enemy.SwitchState(EnemyState.Damaged, CharacterManager.Instance.PlayerStat.Attack);
            }
            // ���� ������ ó�� 
            else if (collider.TryGetComponent(out BossController boss))
            {
                Debug.Log("������ ����");
                boss.TakeDamage(CharacterManager.Instance.PlayerStat.Attack, playerController.hitObj);
            }
        }
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // ���� �ִϸ��̼� ���Ŀ� ����Ű(x)�� �Է��ϸ� 
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"Time.time = {Time.time}");
            Debug.Log($"lastAttackTime = {lastAttackTime}");
            Debug.Log($"Time.time - lastAttackTime = {Time.time - lastAttackTime}");
            if (Time.time - lastAttackTime > coolTime)
            {
                lastAttackTime = Time.time;

                stateMachine.PlayerController.AnimationController.Attack();
                Debug.Log("�ִϸ��̼� �����!");
                lastInputTime = Time.time;
            }
        }

        // 0.5�ʰ� �����µ� �ƹ��� �Է��� ������
        if (Time.time - lastInputTime > coolTime)
        {
            // ���� ���� ���·� ��ȯ 
            Debug.Log("���� ���� ��ȯ!");
            //stateMachine.PlayerController.AnimationController.CancelAttack();
            stateMachine.SwitchState(stateMachine.GetPreState());
            return;
        }

        // 
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
        // �̵� ���� ����
        playerController.MovementDirection = playerController.MovementInput;

        // �̵� �ӵ� ����
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // �߷��� velocity.y������ ����
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;
        
        // �̵� ó��
        playerController.Rigid.velocity = playerController.MovementDirection;
    }


}
