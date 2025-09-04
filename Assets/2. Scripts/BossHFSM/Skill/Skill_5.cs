using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_5 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Skill_5 Hit Player");
            CharacterManager.instance.PlayerStat.TakeDamage();
        }
    }
}
