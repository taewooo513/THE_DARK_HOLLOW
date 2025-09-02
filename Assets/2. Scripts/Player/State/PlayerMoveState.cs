using UnityEngine;

// 움직이는 역할 
public class PlayerMoveState : BaseState
{
    private StateManager player;

    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello from the Move State");
        this.player = player;
    }

    public override void UpdateState(StateManager player)
    {
        
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {
        Debug.Log("(FixedUpdateState): Player Move!!");
        Move();
        Jump();
    }

    private void Move()
    {
        //// 이동 방향 설정
        //movementDirection = player.movementInput;

        //// 이동 속도 설정
        //movementDirection *= (moveSpeed * speedModifier);

        //// 중력은 velocity.y값으로 설정
        //movementDirection.y = rigid.velocity.y;

        //// 이동 처리
        //rigid.velocity = movementDirection;
    }

    private void Jump()
    {
        //if (canJump)
        //{
        //    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //    canJump = false;
        //}
    }
}
