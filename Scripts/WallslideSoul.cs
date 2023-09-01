using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallslideSoul : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.wallSlideColledcted = true;
            Destroy(gameObject);
        }
    }
}
