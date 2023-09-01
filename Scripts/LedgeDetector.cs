using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    Rigidbody2D rb;
    public bool canDetect;
    [SerializeField] private BoxCollider2D wallCollider;
    [SerializeField] private CapsuleCollider2D touchingCol;
    public ContactFilter2D contactFilter;

    public bool ledgeDetected = false;

    public float wallDistance = 0.02f;
    public float ledgeDistance = 0.05f;
    Animator animator;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;


    
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ledgeHits = new RaycastHit2D[5];

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
            
        }
    }

    [SerializeField]
    private bool _isOnLedge = false;

    public bool IsOnLedge
    {
        get
        {
            return _isOnLedge;
        }
        private set
        {
            _isOnLedge = value;
            
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
        IsOnWall = wallCollider.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) > 0 ||
                   wallCollider.Cast(Vector2.up, contactFilter, wallHits, wallDistance) > 0 ||
                   wallCollider.Cast(Vector2.down, contactFilter, wallHits, wallDistance) > 0;
        IsOnLedge = touchingCol.Cast(wallCheckDirection, contactFilter, ledgeHits, ledgeDistance) > 0 ||
            touchingCol.Cast(Vector2.up, contactFilter, ledgeHits, ledgeDistance) > 0 ||
            touchingCol.Cast(Vector2.down, contactFilter, ledgeHits, ledgeDistance) > 0;

        if(IsOnLedge)
        {
            if(!IsOnWall)
            {
                ledgeDetected = true;
            }
            else
            {
                ledgeDetected = false;
            }
        }
        else
        {
            ledgeDetected = false;
        }

    }


/*    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDetect)
        {
            ledgeDetected = true;
        }
        else
        {
            ledgeDetected = false;
        }

    }*/



}
