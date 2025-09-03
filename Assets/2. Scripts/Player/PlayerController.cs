using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("Init Info")]
    [field: SerializeField] public Rigidbody2D Rigid { get; set; }
    private BoxCollider2D coll;

    [field: Header("Player Info")]
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    [field: SerializeField] public Vector2 MovementDirection { get; set; }
    [field: SerializeField] public bool CanJump { get; set; }
    [field: SerializeField] public bool IsMoving { get; set; }

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateMachine stateMachine;
    private PlayerStat playerStat;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateMachine = GetComponent<StateMachine>();
        playerStat = GetComponent<PlayerStat>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 키를 누르고 있으면 (땅에 있든 없든 상관X -> 이동은 땅에서, 공중에서 다 가능)
        if(context.phase == InputActionPhase.Performed)
        {
            // 이동 중이라는 걸 표시
            IsMoving = true;

            // 이동할 수 있게 방향을 부여 
            MovementInput = context.ReadValue<Vector2>();

            // 이동상태 전환 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
        }
        // 키를 떼면 
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 이동 중 X
            IsMoving = false;

            MovementInput = Vector2.zero;

            // Idle 상태 전환
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프키(z)를 누르기 시작했을 때 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            CanJump = true;

            // Jump 상태로 전환
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Jump));
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // 달리기키(shift)를 누르고 있고, 땅에 있을때
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // Run 상태로 전환 -> 원래 속도의 50%로 빠르게 이동할 수 있게 하기 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
        }

        // 달리기키(shift)를 뗐을 때 
        else if(context.phase == InputActionPhase.Canceled)
        {
            // Idle 상태로 전환 -> SpeedModifier = 1.0f;
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public bool IsGrounded()
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
