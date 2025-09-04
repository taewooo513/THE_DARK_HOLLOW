using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CharacterManager.instance.PlayerStat.playerCheckPoint = transform.position;
        }
    }
}
