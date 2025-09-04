using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : BaseState
{
    private float coolTime;
    private float lastAttackTime;
    private float lastInputTime;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Attack State");
        coolTime = Constants.CoolTime.ATTACK;
        lastAttackTime = 0.0f;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // 공격 쿨타임이 끝나면 
        if (Input.GetKeyDown(KeyCode.X) && Time.time - lastAttackTime > coolTime)
        {
            lastAttackTime = Time.time;

            // 공격 애니메이션 실행
            Debug.Log("공격 애니메이션 실행");
            stateMachine.PlayerController.AnimationController.Attack();
            lastInputTime = Time.time; // 공격 애니메이션 이후 시간을 기록 
            Debug.Log($"마지막 입력 시간 = {lastInputTime}");
        }

        // 공격 애니메이션 이후에 공격키(x)를 입력하면 
        if (Input.GetKeyDown(KeyCode.X))
        {
            // 그 시간을 기록 
            lastInputTime = Time.time;
        }

        Debug.Log($"현재 시간 - 마지막 입력 시간 = {Time.time - lastAttackTime}");
        // 0.5초가 지났는데 아무런 입력이 없으면
        if (Time.time - lastInputTime > coolTime)
        {
            Debug.Log("0.5초가 지났으나 아무런 입력이 없습니다. Idle 상태로 전환합니다.");
            stateMachine.PlayerController.AnimationController.CancelAttack();
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            return;
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {

    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

}
