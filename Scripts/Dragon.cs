using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public bool skyDetected;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform skyCheck;
    [SerializeField] private float moveSpeed ;
    [SerializeField] private float skyDistance ;
    [SerializeField] private LayerMask whatIsSky ;
    [SerializeField] private Transform mouth;
    [SerializeField] private GameObject firePartical;
    [SerializeField] private float attackCooldown = 1f;
    private float timeAttack;

    private Vector2 WallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Update()
    {
        Attack();
    }
    private void FixedUpdate()
    {
        UpdateMovingState();
    }
    private void UpdateMovingState()
    {
        skyDetected = Physics2D.Raycast(skyCheck.position, WallCheckDirection, skyDistance, whatIsSky);
        
        //wallDetected = Physics2D.Raycast(wallCheck.position, WallCheckDirection, wallDistance, whatIsGround);
        if (skyDetected)
        {
            transform.localScale *= new Vector2(-1, 1);
        }
        else
        {
            rb.velocity = new Vector2(moveSpeed * transform.localScale.x, rb.velocity.y);
        }
    }

    private void Attack()
    {

        if(Time.time >= timeAttack + attackCooldown) 
        {
            Instantiate(firePartical, mouth.position, firePartical.transform.rotation);
            timeAttack = Time.time;
        }
            
    }

}
