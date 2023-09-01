using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    public bool isTouchingLedge;
    public bool isOnWallSlide;
    public bool isTouchingWall;
    public bool isTouchingCeiling;

    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    public ContactFilter2D contactFilter;


    [SerializeField] private CapsuleCollider2D wallCollider;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsCeiling;
    [SerializeField] private float distance = 0.01f;
    [SerializeField] private float groundDistance = 0.01f;
    [SerializeField] private float slideDistance = 0.36f;
    [SerializeField] private float wallDistance = 0.3f;
    [SerializeField] private float ceilingDistance = 0.3f;

    [SerializeField] private TouchingDirections direction;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallSlideCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Animator animator;

    [SerializeField] private float xOffset1;
    [SerializeField] private float yOffset1;
    [SerializeField] private float xOffset2;
    [SerializeField] private float yOffset2;

    public bool ledgeDetected;

    [SerializeField]private Vector2 ledgePosBot;

    private Vector2 ledgePos1;
    public Vector2 ledgePos2;
    private Vector2 WallCheckDirection => parent.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField] private bool _isOnWall = false;

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool("isOnWall", value);
        }
    }


    [SerializeField] private bool _isGrounded = true;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool("isGrounded", value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        isTouchingLedge = Physics2D.Raycast(transform.position, WallCheckDirection, distance, whatIsWall);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, WallCheckDirection, distance, whatIsWall);
        isOnWallSlide = Physics2D.Raycast(wallSlideCheck.position, WallCheckDirection, slideDistance, whatIsWall);
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, whatIsGround);
        IsOnWall = wallCollider.Cast(WallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
        isTouchingCeiling = Physics2D.Raycast(ceilingCheck.position, Vector2.up, ceilingDistance, whatIsCeiling);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = transform.position;
        }
        
        
        
    }
    public void CheckLedgeClimb()
    {
        if(ledgeDetected && !player.canClimb && player.canGrabLedge)
        {
            
            player.canGrabLedge = false;

            ledgePos1 = ledgePosBot + new Vector2(xOffset1 * player.transform.localScale.x, yOffset1);
            ledgePos2 = ledgePosBot + new Vector2(xOffset2 * player.transform.localScale.x, yOffset2);

            /*if (player.IsFacingRight)
            {
                ledgePos1 = new Vector2((Mathf.Floor(ledgePosBot.x + distance) - xOffset1), Mathf.Floor(ledgePosBot.y) + yOffset1);
                ledgePos2 = new Vector2((Mathf.Floor(ledgePosBot.x + distance) + xOffset2), Mathf.Floor(ledgePosBot.y) + yOffset2);
            }
            else
            {
                ledgePos1 = new Vector2((Mathf.Ceil(ledgePosBot.x - distance) + xOffset1), Mathf.Floor(ledgePosBot.y) + yOffset1);
                ledgePos2 = new Vector2((Mathf.Ceil(ledgePosBot.x - distance) - xOffset2), Mathf.Floor(ledgePosBot.y) + yOffset2);
            }*/


            player.canClimb = true;
        }

        if (player.canClimb)
        {
            parent.transform.position = ledgePos1;
            player.animator.SetBool("canClimb", player.canClimb);
        }
    }

    /*private void Check()
    {
        isTouchingLedge = Physics2D.Raycast(transform.position, WallCheckDirection, distance, whatIsWall);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, WallCheckDirection, distance, whatIsWall);
        isOnWallSlide = Physics2D.Raycast(wallSlideCheck.position, WallCheckDirection, slideDistance, whatIsWall);
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, whatIsGround);
        IsOnWall = wallCollider.Cast(WallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distance);
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        Gizmos.DrawWireSphere(wallSlideCheck.position, slideDistance);
        Gizmos.DrawWireSphere(ceilingCheck.position, ceilingDistance);

    }
}
