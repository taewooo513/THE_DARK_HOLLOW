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

        playerController.Rigid.gravityScale = 1.0f;

        // 공격 사운드
        SoundManager.Instance.PlayEFXSound(Constants.SFX.PLAYER_ATTACK);

        // 공격 쿨타임 설정 
        coolTime = Constants.CoolTime.ATTACK;
        lastAttackTime = 0.0f;

        // 게이지 채우기 
        playerController.PlayerStat.Gauge += 1;
        if(playerController.PlayerStat.Gauge > 5)
            playerController.PlayerStat.Gauge = 5;

        // OverlapBox 생성
        Collider2D[] colliders;
        if (playerController.SpriteRenderer.flipX)
        {
            colliders = Physics2D.OverlapBoxAll(playerController.transform.position + new Vector3(-0.5f, 0.3f, 0), new Vector3(0.5f, 0.5f, 0.5f), 0); // 플레이어 flipX에 따라 다르게 
        }
        else
        {
            colliders = Physics2D.OverlapBoxAll(playerController.transform.position + new Vector3(0.5f, 0.3f, 0), new Vector3(0.5f, 0.5f, 0.5f), 0); // 플레이어 flipX에 따라 다르게 
        }
        foreach (Collider2D collider in colliders)
        {
            // 적 데미지 처리 
            if(collider.TryGetComponent(out Enemy enemy))
            {
                Debug.Log("적이 맞음");
                enemy.SwitchState(EnemyState.Damaged, CharacterManager.Instance.PlayerStat.Attack);

            }
            // 보스 데미지 처리 
            else if (collider.TryGetComponent(out BossController boss))
            {
                Debug.Log("보스가 맞음");
                boss.TakeDamage(CharacterManager.Instance.PlayerStat.Attack, playerController.hitObj);
            }
        }
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 공격 애니메이션 이후에 공격키(x)를 입력하면 
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log($"Time.time = {Time.time}");
            Debug.Log($"lastAttackTime = {lastAttackTime}");
            Debug.Log($"Time.time - lastAttackTime = {Time.time - lastAttackTime}");
            if (Time.time - lastAttackTime > coolTime)
            {
                lastAttackTime = Time.time;

                stateMachine.PlayerController.AnimationController.Attack();
                Debug.Log("애니메이션 실행됨!");
                lastInputTime = Time.time;
            }
        }

        // 0.5초가 지났는데 아무런 입력이 없으면
        if (Time.time - lastInputTime > coolTime)
        {
            // 공격 이전 상태로 전환 
            Debug.Log("이전 상태 전환!");
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
        // 이동 방향 설정
        playerController.MovementDirection = playerController.MovementInput;

        // 이동 속도 설정
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // 중력은 velocity.y값으로 설정
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;
        
        // 이동 처리
        playerController.Rigid.velocity = playerController.MovementDirection;
    }


}
