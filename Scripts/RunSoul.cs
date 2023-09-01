using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSoul : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.runCollected = true;
            Destroy(gameObject);
        }
    }
}
