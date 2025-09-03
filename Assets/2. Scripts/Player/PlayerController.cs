using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("Init Info")]
    //private Rigidbody2D rigid;
    [field: SerializeField] public Rigidbody2D Rigid { get; set; }
    private BoxCollider2D coll;

    [field: Header("Player Info")]
    //[SerializeField] private float moveSpeed;
    [field: SerializeField] public float MoveSpeed { get; private set; }
    //[SerializeField] private float speedModifier;
    [field: SerializeField] public float SpeedModifier { get; set; }
    //[SerializeField] private float speedModifierInput;
    [field: SerializeField] public float SpeedModifierInput { get; private set; }
    //[SerializeField] private Vector2 movementInput;
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    //[SerializeField] private Vector2 movementDirection;
    [field: SerializeField] public Vector2 MovementDirection { get; set; }
    //[SerializeField] private float jumpPower;
    [field: SerializeField] public float JumpPower { get; private set; }
    //[SerializeField] private bool canJump;
    [field: SerializeField] public bool CanJump { get; set; }

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateManager stateManager;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateManager = GetComponent<StateManager>();
    }

    private void FixedUpdate()
    {
        //Move();
        //Jump();

        // Draw ray
        //Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0)); // 안됨.
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 키를 누르고 있고, 땅에 있으면 
        if (context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // 이동할 수 있게 방향을 부여 
            MovementInput = context.ReadValue<Vector2>();

            // 이동상태 전환 
            //stateManager.SwitchState(stateManager.moveState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Move));
        }
        // 키를 떼면 
        else if (context.phase == InputActionPhase.Canceled)
        {
            MovementInput = Vector2.zero;

            // Idle 상태 전환
            //stateManager.SwitchState(stateManager.idleState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Idle));
        }
    }

    //private void Move()
    //{
    //    // 이동 방향 설정
    //    MovementDirection = MovementInput;

    //    // 이동 속도 설정
    //    MovementDirection *= (MoveSpeed * SpeedModifier);

    //    // 중력은 velocity.y값으로 설정
    //    Vector2 dir = MovementDirection;
    //    dir.y = Rigid.velocity.y;
    //    MovementDirection = dir;
    //    //MovementDirection.y = Rigid.velocity.y;

    //    // 이동 처리
    //    Rigid.velocity = MovementDirection;
    //}

    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프키(z)를 누르기 시작했을 때 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            CanJump = true;

            // Jump 상태로 전환
            //stateManager.SwitchState(stateManager.jumpState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Jump));
        }
    }

    //private void Jump()
    //{
    //    if (CanJump)
    //    {
    //        Rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    //        CanJump = false;
    //    }
    //}

    public void OnRun(InputAction.CallbackContext context)
    {
        // 달리기키(shift)를 누르고 있고, 땅에 있을때
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // 달리기 처리 
            // 원래 속도의 50%로 빠르게 이동할 수 있게 하기 
            //SpeedModifier = SpeedModifierInput;

            // Run 상태로 전환 
            //stateManager.SwitchState(stateManager.runState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Run));
        }

        // 달리기키(shift)를 뗐을 때 
        else if(context.phase == InputActionPhase.Canceled)
        {
            //SpeedModifier = 1.0f;

            // Idle 상태로 전환
            //stateManager.SwitchState(stateManager.idleState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Idle));
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
