using UnityEngine;

// 추상 상태 클래스로 사용 
// 모노 필요없음. 
// concrte 클래스에서 구현 
public abstract class BaseState 
{
    public abstract void EnterState(StateMachine stateMachine);

    public abstract void UpdateState(StateMachine stateMachine);

    public abstract void FixedUpdateState(StateMachine stateMachine);

    public abstract void OnCollisionEnter(StateMachine stateMachine, Collision2D collision);

    public abstract void OnTriggerEnter(StateMachine stateMachine, Collision2D collision);
}
