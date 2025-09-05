using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_01AnimationEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private Enemy_01 enemy;
    public GameObject leftColl;
    public GameObject rightColl;

    public void Awake()
    {
        enemy = GetComponentInParent<Enemy_01>();
    }

    public void AbleAttackCollision()
    {
        if (enemy.isRight == true)
        {
            rightColl.SetActive(true);
        }
        else
            leftColl.SetActive(true);
    }
    public void DisAbleAttackCollision()
    {
        if (enemy.isRight == true)
        {
            rightColl.SetActive(false);
        }
        else
            leftColl.SetActive(false);
    }

    public void EndAttack()
    {
        enemy.AttackEnd();
    }

    public void EndDamaged()
    {
        enemy.SwitchState(EnemyState.Idle);
    }

    public void EndDie()
    {
        Destroy(enemy.gameObject);
    }
}
