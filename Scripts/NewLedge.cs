using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLedge : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private LayerMask whatIsGround;

    private bool canDetect;


    private void FixedUpdate()
    {
        if(canDetect)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetect = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetect = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
