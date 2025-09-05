using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_01_Attack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerStat playerStat))
        {
            playerStat.TakeDamage();
            CameraManager.Instance.CameraShack(1, 10, 0.08f);
        }
    }
}
