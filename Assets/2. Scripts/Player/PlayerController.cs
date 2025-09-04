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
    [field: SerializeField] public PlayerStat PlayerStat { get; set; }
    [SerializeField] public AnimationController AnimationController { get; set; }
    [field: SerializeField] public bool IsHit { get; set; }
    //[field: SerializeField] public bool IsInvincible { get; set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer;
    [field: SerializeField] public GameObject hitObj {  get; set; }

    [Header("Collision Info")]
    [field: SerializeField] public Collider2D collider;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateMachine = GetComponent<StateMachine>();
        PlayerStat = GetComponent<PlayerStat>();
        AnimationController = GetComponent<AnimationController>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            stateMachine.SetStateBeforeJump(stateMachine);
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
            // 아직 달리고 있으면
            if (IsMoving)
            {
                // 계속 달릴 수 있도록 Move 상태로 전환 
                // Walk 애니메이션으로 전환 
                //stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
            }
            // 그게 아니면
            else
            {
                // Idle 상태로 전환 -> SpeedModifier = 1.0f
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            }

            //// Idle 상태로 전환 -> SpeedModifier = 1.0f;
            //stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // 공격키(x)를 누르기 시작했고, 땅에 있으면
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            // 공격상태로 전환한다. 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Attack));
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Boss"))
        {
            IsHit = true;
            this.collider = collider;
        }
    }
}
