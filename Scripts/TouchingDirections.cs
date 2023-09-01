using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D wallCollider;
    [SerializeField] private CapsuleCollider2D touchingCol;
    [SerializeField] private CapsuleCollider2D wallSlide;
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.02f;
    public float ceilingDistance = 0.05f;

    public bool isOnWallSlide;

    Animator animator;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;


    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded = true;

    public bool IsGrounded { get
        {
            return _isGrounded;
        } private set
        {
            _isGrounded = value;
            animator.SetBool("isGrounded", value);
        }
    }

    [SerializeField]
    private bool _isOnWall = false;

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

    [SerializeField]
    private bool _isOnCeiling = false;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool("isOnCeiling", value);
        }
    }


    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        IsGrounded = touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;

        IsOnWall = wallCollider.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
        isOnWallSlide = wallSlide.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = wallCollider.Cast(Vector2.up, contactFilter, ceilingHits, ceilingDistance) > 0;
        
    }

}
