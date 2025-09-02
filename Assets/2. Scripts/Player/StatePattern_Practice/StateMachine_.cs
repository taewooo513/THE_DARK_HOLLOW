using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// context = 구체적인 상태를 생성, 현재 활성화된 상태를 전달, 구체적인 상태 데이터 저장, 상태 전환
public class StateMachine_ : MonoBehaviour
{
    [Header("Self Init Info")]
    private Rigidbody2D rigid;
    private Animator animator;
    private PlayerInput playerInput;

    [Header("Animator Parameter Variables")]
    private int isWalkingHash;
    private int isRunningHash;

    [Header("Player Input Variables")]
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 appliedMovement;
    private bool isMovementPressed;
    private bool isRunPressed;

    [Header("Constants Variables")]
    private float rotationFactorPerFrame = 15.0f;
    private float runMultiplier = 4.0f;
    private int zero = 0;

    [Header("Gravity Variables")]
    private float gravity = -9.8f;
    private float groundedGravity = 4.0f;

    [Header("Jumping Variables")]
    private bool isJumpPressed = false;
    private float initialJumpVelocity;
    private float maxJumpHeight = 4.0f;
    private float maxJumpTime = 0.75f;
    private bool isJumping = false;
    private int isJumpingHash;
    private int jumpCountHash;
    private bool isJumpAnimating = false;
    private int jumpCount = 0;
    private Dictionary<int, float> initialJumpVelocities = new();
    private Dictionary<int, float> jumpGravities = new();
    private Coroutine currentJumpResetRoutine = null;

    private void Awake()
    {
        // Set reference variables
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerInput = new PlayerInput();
        //playerInput = GetComponent<PlayerInput>();  

        // Set the parameter hash references
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        // Set the player input callbacks
        playerInput.Player.Move.started += OnMove;
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Run.started += OnRun;
        playerInput.Player.Run.canceled += OnRun;
        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>(); // input init 
        isMovementPressed = currentMovementInput.x != zero; // move press(bool) init 
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton(); // is jump button clicked? yes -> true, no -> false 
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton(); // is run button clicked? yes -> true, no -> false 
    }

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    //private void HandleAnimation()
    //{
    //    // Get animation parameter values from animator
    //    bool isWalking = animator.GetBool(isWalkingHash);
    //    bool isRunning = animator.GetBool(isRunningHash);

    //    // Start walking if movement pressed is true and not walking
    //    if(isMovementPressed && !isWalking)
    //    {
    //        // Start walking animation
    //        animator.SetBool(isWalkingHash, true);
    //    }

    //    // Stop walking if movement pressed is false and walking
    //    else if(!isMovementPressed && isWalking)
    //    {
    //        // Stop walking animation
    //        animator.SetBool(isWalkingHash, false);
    //    }
    //}
}
