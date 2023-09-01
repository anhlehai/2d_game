using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject player;
    [SerializeField] private Respawn respawn;
    // Start is called before the first frame update


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Touched");
        if (collision.gameObject.tag.Equals( "Player"))
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathParticle, player.transform.position, deathParticle.transform.rotation);
        
        respawn.RespawnTime();
        player.SetActive(false);
    }
}
