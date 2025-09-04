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
    [field: SerializeField] public bool IsClimbable { get; set; }

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateMachine stateMachine;
    [field: SerializeField] public PlayerStat PlayerStat { get; set; }
    [SerializeField] public AnimationController AnimationController { get; set; }
    [field: SerializeField] public bool IsHit { get; set; }
    //[field: SerializeField] public bool IsInvincible { get; set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer;
    [field: SerializeField] public GameObject hitObj {  get; set; }
    

    // lockInput==true -> return 

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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SceneLoadManager.Instance.LoadScene(SceneKey.bossScene);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!PlayerStat.isMoved) return;

        // Ű�� ������ ������ (���� �ֵ� ���� ���X -> �̵��� ������, ���߿��� �� ����)
        if (context.phase == InputActionPhase.Performed)
        {
            // �̵� ���̶�� �� ǥ��
            IsMoving = true;

            // �̵��� �� �ְ� ������ �ο� 
            MovementInput = context.ReadValue<Vector2>();

            // �̵����� ��ȯ 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
        }
        // Ű�� ���� 
        else if (context.phase == InputActionPhase.Canceled)
        {
            // �̵� �� X
            IsMoving = false;

            MovementInput = Vector2.zero;

            // Idle ���� ��ȯ
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // ����Ű(z)�� ������ �������� �� 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            CanJump = true;

            // Jump ���·� ��ȯ
            stateMachine.SetStateBeforeJump(stateMachine);
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Jump));
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // �޸���Ű(shift)�� ������ �ְ�, ���� ������
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // Run ���·� ��ȯ -> ���� �ӵ��� 50%�� ������ �̵��� �� �ְ� �ϱ� 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
        }

        // �޸���Ű(shift)�� ���� �� 
        else if(context.phase == InputActionPhase.Canceled)
        {
            // ���� �޸��� ������
            if (IsMoving)
            {
                // ��� �޸� �� �ֵ��� Move ���·� ��ȯ 
                // Walk �ִϸ��̼����� ��ȯ 
                //stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
            }
            // �װ� �ƴϸ�
            else
            {
                // Idle ���·� ��ȯ -> SpeedModifier = 1.0f
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            }

            //// Idle ���·� ��ȯ -> SpeedModifier = 1.0f;
            //stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // ����Ű(x)�� ������ �����߰�, ���� ������
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            // ���ݻ��·� ��ȯ�Ѵ�. 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Attack));
        }
    }

    public void OnSpecialAttack(InputAction.CallbackContext context)
    {
        // Ư������Ű(space)�� ������ �����߰�, ���� ������
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            // Ư�����ݻ��·� ��ȯ�Ѵ�. 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.SpecialAttack));
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
        else if (collider.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("��ٸ� ����");
            IsClimbable = true;   
        }
    }

    
}
