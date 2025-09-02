using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;              // 타깃
    public Rigidbody2D rb;
    public Animator animator;
    public BossStat stat;

    // FSM
    public BossStateMachine fsm { get; private set; }
    // States
    IdleState idle; ChaseState chase; ChooseState choose;
    AttackDashState atkDash; AttackRangedState atkRanged;
    RecoverState recover; DeadState dead;

    // 쿨다운
    float readyDash, readyRanged;

    // 캐시
   public float Dist => player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
    bool InAggro => Dist <= stat.detectRange;
    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!stat) stat = GetComponent<BossStat>();

        fsm = new BossStateMachine();

        // 상태 인스턴스
        idle = new IdleState(this, fsm);
        chase = new ChaseState(this, fsm);
        choose = new ChooseState(this, fsm);
        atkDash = new AttackDashState(this, fsm);
        atkRanged = new AttackRangedState(this, fsm);
        recover = new RecoverState(this, fsm);
        dead = new DeadState(this, fsm);
    }

    void Start()
    {
        // CharacterManager에 보스 등록 (원 코드 호환)
        if (CharacterManager.instance) CharacterManager.instance.Boss = GetComponent<Boss>();
        fsm.Change(idle);
    }

    void Update()
    {
        // 1) 아웃 오브 어그로 글로벌 가드: 범위를 벗어나면 어떤 상태든 즉시 Idle로 강제
        if (!InAggro && fsm.Current != null && fsm.Current.Name != "Idle")
        {
            StopMove();
            fsm.Change(idle, reason: "Force"); // 잠금 무시하고 즉시 복귀
            return;
        }

        // 2) 평상시 틱
        fsm.Tick(Time.deltaTime);
    }

    void FixedUpdate() => fsm.FixedTick(Time.fixedDeltaTime);

    // ─────────────────────────────
    // 공용 유틸 (상태에서 호출)
    // ─────────────────────────────
    public bool CanSeePlayer() => player && InAggro; 
    public bool CDReadyDash() => Time.time >= readyDash;
    public bool CDReadyRanged() => Time.time >= readyRanged;
    public void StartCD_Dash() => readyDash = Time.time + stat.dashCooldown;
    public void StartCD_Ranged() => readyRanged = Time.time + stat.rangedCooldown;

    public void MoveTowards(Vector2 pos, float speed)
    {
        var dir = (pos - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
        if (dir.x != 0) transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }
    public void StopMove() => rb.velocity = Vector2.zero;

    public void FaceToPlayer()
    {
        if (!player) return;
        var dir = (player.position - transform.position);
        if (Mathf.Abs(dir.x) > 0.01f) transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    public void Play(string trigger)
    {
        if (!animator || string.IsNullOrEmpty(trigger)) return;
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
    }

    // 투사체 스폰
    public void FireProjectile()
    {
        if (!stat.bulletPrefab) return;
        Vector3 spawnPos = stat.firePoint ? stat.firePoint.position : transform.position;
        var go = Instantiate(stat.bulletPrefab, spawnPos, Quaternion.identity);
        // TODO: 발사체 이동/데미지 스크립트에 방향/속도 주입 (예시)
        if (player && go.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            Vector2 dir = (player.position - spawnPos).normalized;
            rb2.velocity = dir * 10f;
        }
    }

    public void ToDie()
    {
        Destroy(this.gameObject, 3f);
    }


    // 상태 접근자(내부 전이용)
    public IdleState SIdle => idle;
    public ChaseState SChase => chase;
    public ChooseState SChoose => choose;
    public AttackDashState SAtkDash => atkDash;
    public AttackRangedState SAtkRng => atkRanged;
    public RecoverState SRecover => recover;
    public DeadState SDead => dead;
}
