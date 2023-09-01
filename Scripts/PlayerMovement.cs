using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    TouchingDirections touchingDirections;
    Vector2 movementInput;
    [SerializeField] private LedgeCheck directionsCheck;

    [Header("Collectable")]
    public bool doubleJumpCollected = false;
    public bool runCollected = false;
    public bool wallSlideColledcted = false;




    [Header("Movement")]
    public float walkSpeed = 1f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    public float airSpeed = 3f;
   

    [Header("Dash")]
    public float dashPower = 5f;
    public bool canDash = true;
    public bool isDashing;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    [Header("Jump")]
    public float coyoteTime;
    private float coyoteTimer;

    public float bufferTime = 0.2f;
    private float bufferTimer;

    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlidingDuration = 3f;
    [SerializeField] private float wallSlidingTimer;
    [SerializeField] private float wallSlidingSpeed = 2f;

    [SerializeField] private bool doubleJumpp = false;
    [SerializeField] private float doubleJumpPower = 5f;

    [SerializeField] private bool  isWallJumping= false;
    [SerializeField] private float wallJumpingCounter = 0.1f;
    [SerializeField] private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime;
    [SerializeField] private float wallJumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower;


    [Header("Ledge")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbStartPosition;
    private Vector2 climbEndPosition;
    //private bool canGrab = true;
    //private bool leo;

    public bool canGrabLedge = true;
    public bool canClimb = false;
    public bool ledgeDetected;



    [Header("Properties")]
    [SerializeField]private bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set 
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public float CurrentSpeed 
    { 
        get 
        {
            if (CanMove)
            {
                if (IsMoving )
                {

                    if (directionsCheck.IsGrounded)
                    {
                        if (IsRunning)
                        {

                            return runSpeed;
                        }
                        else
                        {

                            return walkSpeed;
                        }

                    }
                    else
                    {  
                        return airSpeed;
                    }

                }

                return 0;
            }
            return 0;

        } 
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { get
        {
            return _isMoving;
        } private set
        {
            _isMoving = value;

            animator.SetBool("isMoving", value);
        }}

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; animator.SetBool("isRunning", value); } }

    public bool CanMove { get
        {
            return animator.GetBool("canMove");
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
        //CheckForLedge();
        directionsCheck.CheckLedgeClimb();
    }

    private void FixedUpdate()
    {
        

        if (!isDashing)
        {

            if (!isWallJumping)
            {
                rb.velocity = new Vector2(movementInput.x * CurrentSpeed, rb.velocity.y);
                SetFacing(rb.velocity);
            }

            

        }

        FallSpeed();

        if (!directionsCheck.IsGrounded)
        {
            bufferTimer -= Time.deltaTime;
        }

        animator.SetFloat("y velocity", rb.velocity.y);

        if (directionsCheck.IsGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        

        if (bufferTimer > 0f && directionsCheck.IsGrounded)
        {
            Jump(jumpImpulse);
            bufferTimer = 0f;
            doubleJumpp = true;
        }

        if (directionsCheck.IsGrounded)
        {
            doubleJumpp = true;
            wallSlidingTimer = wallSlidingDuration;
        }



        WallSlide();
        WallJump();
        
        
        //ledgeDetection.CheckLedgeClimb();


    }


    private void FallSpeed()
    {
        if(CurrentSpeed == runSpeed)
        {
            airSpeed = runSpeed;
        }
        else if (CurrentSpeed == walkSpeed || !IsRunning)
        {
            airSpeed = walkSpeed;
        }
    }

    /*private void CheckForLedge()
    {
        if (ledgeDetected && canGrab)
        {
            canGrab= false;
            
            Vector2 ledgePosition = GetComponentInChildren<NewLedge>().transform.position;
            offset1.x *= transform.localScale.x;
            offset2.x *= transform.localScale.x;
            climbStartPosition = ledgePosition + offset1;
            climbEndPosition = ledgePosition + offset2;

            leo = true;
        }
        if (leo)
        {
            transform.position = climbStartPosition;

        }
        animator.SetBool("canClimb", canClimb);
    }*/

    /*private void LedgeClimbEnd()
    {

    }*/


    private void WallSlide()
    {
        if (!directionsCheck.isOnWallSlide)
        {
            wallSlidingTimer = wallSlidingDuration;
        }
        else if(isWallSliding)
        {
            wallSlidingTimer -= Time.fixedDeltaTime;
        }

        if (!directionsCheck.IsGrounded && directionsCheck.isOnWallSlide && wallSlidingTimer > 0f && rb.velocity.y <0
            && !canClimb && !isWallJumping && wallSlideColledcted)
        {
            isWallSliding = true;
            animator.SetBool("IsWallSliding", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            doubleJumpp = true;
        }
        else
        {
            animator.SetBool("IsWallSliding", false);
            isWallSliding = false;
            
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpingCounter -= Time.fixedDeltaTime;
        }
    }

    private void StopWallJump()
    {
        isWallJumping = false;
        
    }

    
    private void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = directionsCheck.ledgePos2;
        
        directionsCheck.ledgeDetected = false;
        animator.SetBool("canClimb", canClimb);
        canGrabLedge = true;
    }

    

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        IsMoving = movementInput.x != 0; //movementInput != Vector2.zero
        SetFacing(movementInput);
    }

    private void SetFacing(Vector2 movementInput)
    {
        if(!canClimb)
        {
            if (movementInput.x > 0 && !IsFacingRight)
            {
                IsFacingRight = true;
            }
            else if (movementInput.x < 0 && IsFacingRight)
            {
                IsFacingRight = false;
            }
        }
       
        
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started && runCollected)
        {
            IsRunning = true;
        }else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(CanMove)
        {
            if (context.started && wallJumpingCounter > 0f)
            {

                isWallJumping = true;
                Vector2 wallJump = new(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
                animator.SetTrigger("jump");
                isWallSliding = false;
                SetFacing(wallJump);
                rb.velocity = wallJump;

                wallJumpingCounter = 0f;
                Invoke(nameof(StopWallJump), wallJumpingDuration);
            }
            
            else if (!isWallSliding)
            {
                if (context.started && directionsCheck.IsGrounded)
                {
                    Jump(jumpImpulse);
                    doubleJumpp = true;
                }
                else if (context.started && rb.velocity.y < 0f && coyoteTimer > 0f)
                {
                    Jump(jumpImpulse);
                    doubleJumpp = true;
                    coyoteTimer = 0f;

                }

                else if (context.started && !directionsCheck.IsGrounded && doubleJumpp && !canClimb && doubleJumpCollected)
                {
                    Jump(doubleJumpPower);
                    wallSlidingTimer = wallSlidingDuration;
                    doubleJumpp = false;

                }
                else if (!directionsCheck.IsGrounded && context.started)
                {
                    bufferTimer = bufferTime;

                }
            }


        }
        


    }


    public void OnLight(InputAction.CallbackContext context)
    {
        if (context.started && directionsCheck.IsGrounded)
        {
            animator.SetTrigger("Attack");
        }
    }


    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.started && canDash)
        {
            animator.SetTrigger("Dash");
            StartCoroutine(Dashing());

        }
        
    }

    private void Jump(float power)
    {

        animator.SetTrigger("jump");
        if (IsMoving)
        {
            if (IsRunning)
            {
                airSpeed = runSpeed;
            }
            else
            {
                { airSpeed = walkSpeed; }
            }
        }
        rb.velocity = new Vector2(rb.velocity.x, power);
    }

    private IEnumerator Dashing()
    {
        canDash = false;
        isDashing = true;
        
        
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);
        yield return new WaitForSeconds(dashingTime);
        
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    
}
            
        
    


