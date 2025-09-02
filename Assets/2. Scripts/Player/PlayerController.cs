using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     * 움직이자. 
     * 
     * 움직이려면 뭐가 필요해? 
     * - 리지드 바디
     * - 이동 속도 
     * - 애니메이션
     * 
     * 플레이어가 벽이나 오브젝트에 충돌하면 막혀야돼. 
     * 충돌처리를 하려면 뭐가 필요해? 
     * - 콜라이더
     * 
     * 이동시키려면 플레이어의 입력이 필요해. 
     * 플레이어 입력을 뭘로 처리할까? 
     * 
     */

    [Header("Init Info")]
    private Rigidbody2D rigid;
    private BoxCollider2D coll;

    [Header("Player Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedModifier;
    [SerializeField] private float speedModifierInput;
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] private float jumpPower;
    [SerializeField] private bool canJump;

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateManager stateManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateManager = GetComponent<StateManager>();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        // Draw ray
        //Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0)); // 안됨.
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 키를 누르고 있고, 땅에 있으면 
        if (context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // 이동할 수 있게 방향을 부여 
            movementInput = context.ReadValue<Vector2>();

            // 이동상태 전환 
            stateManager.SwitchState(stateManager.moveState);
        }
        // 키를 떼면 
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementInput = Vector2.zero;

            // Idle 상태 전환
            stateManager.SwitchState(stateManager.idleState);
        }
    }

    private void Move()
    {
        // 이동 방향 설정
        movementDirection = movementInput;

        // 이동 속도 설정
        movementDirection *= (moveSpeed * speedModifier);

        // 중력은 velocity.y값으로 설정
        movementDirection.y = rigid.velocity.y;

        // 이동 처리
        rigid.velocity = movementDirection;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프키(z)를 누르기 시작했을 때 
        if (context.phase == InputActionPhase.Started && IsGrounded())
            canJump = true;
    }

    private void Jump()
    {
        if (canJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // 달리기키(shift)를 누르고 있고, 땅에 있을때
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // 달리기 처리 
            // 원래 속도의 50%로 빠르게 이동할 수 있게 하기 
            speedModifier = speedModifierInput;
        }

        // 달리기키(shift)를 뗐을 때 
        else if(context.phase == InputActionPhase.Canceled)
        {
            speedModifier = 1.0f;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        if(hit.collider != null)
            return true;

        return false;
    }

    // test
    public void AddHealth()
    {
        Debug.Log("Add Health!!");
    }

    // test
    public void DetractHealth()
    {
        Debug.Log("Detract Health!!");
    }
}
