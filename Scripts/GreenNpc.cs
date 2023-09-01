using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenNpc : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        npc.SetActive(true);
    }
}
