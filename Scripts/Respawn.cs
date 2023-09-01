using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] private float respawnTime;
    [SerializeField] private PlayerMovement movement;

    private float respawnTimeStart;
    private bool respawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckRespawn();
    }

    public void RespawnTime()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    public void CheckRespawn()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            player.transform.position = transform.position;
            respawn = false;
            player.SetActive(true);
            movement.isDashing = false;
            movement.canDash = true;
        }
    }
}
