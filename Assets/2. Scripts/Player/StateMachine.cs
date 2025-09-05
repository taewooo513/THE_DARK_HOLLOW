using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Move,
    Run,
    Jump,
    Hit,
    Attack,
    SpecialAttack,
    Climb
}

// context = ���� ������ ���� ���� = StateMachine 
public class StateMachine : MonoBehaviour
{
    // ���� �ӽ��� �ϳ��� ���¸� ����. 
    private BaseState currentState;
    private BaseState preState;
    private BaseState stateBeforeJump;

    // ��ü���� ���µ��� ������ Dictionary 
    private Dictionary<PlayerStateType, BaseState> states = new();

    public PlayerController PlayerController { get; set; }

    private void Awake()
    {
        states.Add(PlayerStateType.Idle, new PlayerIdleState());
        states.Add(PlayerStateType.Move, new PlayerMoveState());
        states.Add(PlayerStateType.Run, new PlayerRunState());
        states.Add(PlayerStateType.Jump, new PlayerJumpState());
        states.Add(PlayerStateType.Hit, new PlayerHitState());
        states.Add(PlayerStateType.Attack, new PlayerAttackState());
        states.Add(PlayerStateType.SpecialAttack, new PlayerSpecialAttackState());
        states.Add(PlayerStateType.Climb, new PlayerClimbingState());
    }

    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();    

        // �ʱ� ���� ����
        currentState = states[PlayerStateType.Idle];

        // context�� �Ѱܼ� ���� ���� 
        currentState.EnterState(this);  

    }

    private void Update()
    {
        //Debug.Log($"���� ���� : {currentState.ToString()}");
        switch (currentState)
        {
            case PlayerIdleState:
            case PlayerJumpState:
            case PlayerMoveState:
            case PlayerRunState:
            case PlayerHitState:
            case PlayerAttackState:
            case PlayerSpecialAttackState:
                // �� �����Ӹ��� ����� ����
                currentState.UpdateState(this);
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerMoveState:
            case PlayerJumpState:
            case PlayerRunState:
            case PlayerHitState:
            case PlayerAttackState:
            case PlayerClimbingState:
                currentState.FixedUpdateState(this);
                break;
        }
    }

    // ���� ��ȯ => ���� �ӽ��� ��ü���� ���µ��� ������ �����ϱ� 
    public void SwitchState(BaseState state)
    {
        currentState = state; 
        state.EnterState(this); // ���� �ӽ��� �˷��༭ ���� ���� 
    }

    // ���� �ӽ��� ���� ������Ʈ���� �浹�� �߻��ϸ� �浹 ���·� �����Ѵ� 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public BaseState Getstates(PlayerStateType type)
    {
        return states[type];
    }

    public BaseState GetCurrentState()
    {
        return currentState;
    }

    public BaseState GetPreState()
    {
        return preState;
    }

    public void SetPreState(StateMachine statMachine)
    {
        preState = statMachine.GetCurrentState();
    }

    public void SetStateBeforeJump(StateMachine stateMachine)
    {
        stateBeforeJump = stateMachine.GetCurrentState();
    }

    public BaseState GetStateBeforeJump()
    {
        return stateBeforeJump;
    }
}
