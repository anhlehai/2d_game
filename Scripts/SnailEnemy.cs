using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    public ContactFilter2D contactFilter;


    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D capCol;


    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool groundDetected, wallDetected;
    [SerializeField] private float groundDistance;
    [SerializeField] private float wallDistance;

    private enum State { Moving, IsHit, Death }


    private Vector2 WallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        UpdateMovingState();

        /*switch(currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.IsHit:
                UpdateIsHitState();
                break;
            case State.Death:
                UpdateDeadState();
                break;
        }*/
    }

    private State currentState;

    //Moving------------

    private void EnterMovingState()
    {

    }

    private void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDistance, whatIsGround);
        //wallDetected = capCol.Cast(WallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
        wallDetected = Physics2D.Raycast(wallCheck.position, WallCheckDirection, wallDistance, whatIsGround);
        if (!groundDetected || wallDetected)
        {
            transform.localScale *= new Vector2(-1, 1);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * transform.localScale.x, rb.velocity.y);
        }
    }
    private void ExitMovingState()
    {

    }

    //Got hit ---------------------

    private void EnterIsHitState()
    {

    }

    private void UpdateIsHitState()
    {

    }
    private void ExitIsHitState()
    {

    }

    //Dead -------------------

    private void EnterDeadState()
    {

    }

    private void UpdateDeadState()
    {

    }
    private void ExitDeadState()
    {

    }

    //functions ----------------




    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.IsHit:
                ExitIsHitState();
                break;
            case State.Death:
                ExitDeadState();
                break;
        }

        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.IsHit:
                EnterIsHitState();
                break;
            case State.Death:
                EnterDeadState();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        Gizmos.DrawWireSphere(wallCheck.position, wallDistance);
    }

}
