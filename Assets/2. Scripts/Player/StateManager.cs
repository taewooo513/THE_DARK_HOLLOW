using UnityEngine;

// context = 상태 데이터 전달 역할 = StateMachine = StateManager(나는 매니저로 정의)
// 상태머신의 context(상황, 상태?)
public class StateManager : MonoBehaviour
{
    // 상태 머신은 하나의 상태만 가짐. 
    private BaseState currentState;
    //public BaseState CurrentState { get; set; }

    // 구체적인 상태들을 정의
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerHitState hitState = new PlayerHitState();
    //public PlayerGrowingState growState = new PlayerGrowingState();
    //public PlayerWholeState wholeState = new PlayerWholeState();
    //public PlayerRottenState rottenState = new PlayerRottenState();
    //public PlayerChewedState chewedState = new PlayerChewedState();

    private PlayerController playerController;

    private void Start()
    {
        // 초기 상태 설정
        currentState = idleState;
        //currentState = moveState;
        //currentState = growState;

        // context를 넘겨서 상태 진입 
        currentState.EnterState(this);  

        playerController = GetComponent<PlayerController>();    
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerIdleState:
            case PlayerHitState:
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
            case PlayerRunState:
            case PlayerHitState:
                currentState.FixedUpdateState(this);
                break;
        }
    }

    // 상태 전환 => 상태 매니저가 구체적인 상태들을 가지고 있으니까 
    public void SwitchState(BaseState state)
    {
        currentState = state; 
        state.EnterState(this); // 상태 매니저를 알려줘서 상태 진입 
    }

    // 상태 매니저를 가진 오브젝트에서 충돌이 발생하면 충돌 상태로 진입한다 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }
}
