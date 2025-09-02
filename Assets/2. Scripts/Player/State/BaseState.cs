using UnityEngine;

// 추상 상태 클래스로 사용 
// 모노 필요없음. 
// concrte 클래스에서 구현 
public abstract class BaseState 
{
    public abstract void EnterState(StateManager player);

    public abstract void UpdateState(StateManager player);

    public abstract void FixedUpdateState(StateManager player);

    public abstract void OnCollisionEnter(StateManager player, Collision2D collision);
}
