using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttackState : BaseState
{
    private PlayerController playerController;
    private bool specialAttack;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Special Attack State");
        this.playerController = stateMachine.PlayerController;

        playerController.Rigid.gravityScale = 1.0f;

        SoundManager.Instance.PlayEFXSound(Constants.SFX.PLAYER_SPECIALATTACK);

        // 게이지가 충분한지 검사 
        if(stateMachine.PlayerController.PlayerStat.Gauge == Constants.SpecialAttack.GUAGE)
        {
            specialAttack = true;

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
                if (collider.TryGetComponent(out Enemy enemy))
                {
                    Debug.Log("적이 맞음");
                    enemy.SwitchState(EnemyState.Damaged, CharacterManager.Instance.PlayerStat.SpecialAttack);
                }
                // 보스 데미지 처리 
                else if (collider.TryGetComponent(out BossController boss))
                {
                    Debug.Log("보스가 맞음");
                    boss.TakeDamage(CharacterManager.Instance.PlayerStat.SpecialAttack, playerController.hitObj);
                }
            }
        }
        else
        {
            specialAttack = false;
        }
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 게이지가 충분하고, 특수 공격키(space)를 눌렀으면 
        if(specialAttack)
        {
            // 특수 공격 애니메이션 실행 
            stateMachine.PlayerController.AnimationController.SpecialAttack();

            // 게이지 줄이기 
            stateMachine.PlayerController.PlayerStat.Gauge = 0;

            // 애니메이션 중복 실행 방지 
            specialAttack = false;
        }
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
