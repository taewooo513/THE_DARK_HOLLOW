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

// context = 상태 데이터 전달 역할 = StateMachine 
public class StateMachine : MonoBehaviour
{
    // 상태 머신은 하나의 상태만 가짐. 
    private BaseState currentState;
    private BaseState preState;
    private BaseState stateBeforeJump;

    // 구체적인 상태들을 저장할 Dictionary 
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

        // 초기 상태 설정
        currentState = states[PlayerStateType.Idle];

        // context를 넘겨서 상태 진입 
        currentState.EnterState(this);  

    }

    private void Update()
    {
        //Debug.Log($"현재 상태 : {currentState.ToString()}");
        switch (currentState)
        {
            case PlayerIdleState:
            case PlayerJumpState:
            case PlayerMoveState:
            case PlayerRunState:
            case PlayerHitState:
            case PlayerAttackState:
            case PlayerSpecialAttackState:
                // 매 프레임마다 실행될 상태
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

    // 상태 전환 => 상태 머신이 구체적인 상태들을 가지고 있으니까 
    public void SwitchState(BaseState state)
    {
        currentState = state; 
        state.EnterState(this); // 상태 머신을 알려줘서 상태 진입 
    }

    // 상태 머신을 가진 오브젝트에서 충돌이 발생하면 충돌 상태로 진입한다 
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
