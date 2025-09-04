using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class Enemy_01 : Enemy
{
    public bool isRight;
    public Animator animator;
    public BoxCollider2D attackColliderLeft;
    public BoxCollider2D attackColliderRight;
    PlayerStat player;
    bool isAttackCollTime = false;
    private SpriteRenderer spriteRenderer;
    Coroutine attackCoroutine;
    public float walkRange;
    public float attackRange;
    public LayerMask layerMask;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SwitchState(EnemyState.Idle);
    }
    private void Start()
    {
        player = CharacterManager.Instance.PlayerStat;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.walk:
                Movement();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void Idle()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < walkRange && Vector3.Distance(player.transform.position, transform.position) >= attackRange)
        {
            animator.SetBool("IsMove", true);
            SwitchState(EnemyState.walk);
        }
        else if (isAttackCollTime == false && Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            SwitchState(EnemyState.Attack);
        }
    }

    private void Attack()
    {
    }

    private void Movement()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            SwitchState(EnemyState.Idle);
        }
        else
        {
            Vector3 dir = player.transform.position - transform.position;
            Ray[] ray = new Ray[2];
            ray[0] = new Ray(transform.position + Vector3.left * 0.1f, Vector3.down);
            ray[1] = new Ray(transform.position + Vector3.right * 0.1f, Vector3.down);
            dir = dir.normalized;
            dir.y = 0;

            if (Physics2D.Raycast(ray[0].origin, ray[0].direction, 1f, layerMask) && dir.x < 0)
            {
                transform.position += dir * speed * Time.deltaTime;
            }
            else if (Physics2D.Raycast(ray[1].origin, ray[1].direction, 1f, layerMask) && dir.x > 0)
            {
                transform.position += dir * speed * Time.deltaTime;
            }

            spriteRenderer.flipX = dir.x <= 0 ? true : false;
            isRight = !spriteRenderer.flipX;
            Debug.DrawLine(ray[0].origin, ray[0].origin + Vector3.down * 1f);
            Debug.DrawLine(ray[1].origin, ray[1].origin + Vector3.down * 1f);
        }
    }
    public override void AttackTrigger()
    {
        animator.SetTrigger("AttackTrigger1");
        StartCoroutine(AttackCollTime(attackSpeed));
    }

    public void AttackEnd()
    {
        SwitchState(EnemyState.Idle);
    }

    public override void DamagedTrigger(float dmg)
    {
        animator.SetTrigger("IsHit");
        hp -= dmg;
        if (hp <= 0)
        {
            isDie = true;
            SwitchState(EnemyState.Die);
        }
    }
    public override void DieTrigger()
    {
        animator.SetBool("IsDie", true);//
    }

    public override void IdleTrigger()
    {
        animator.SetBool("IsMove", false);
    }

    IEnumerator AttackCollTime(float atkSpeed)
    {
        isAttackCollTime = true;

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        AttackEnd();
        yield return new WaitForSeconds(atkSpeed);
        isAttackCollTime = false;
    }
}
