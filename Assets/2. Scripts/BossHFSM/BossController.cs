using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;              // 타깃(없으면 자동으로 Player 태그 검색)
    public Rigidbody2D rb;
    public Animator animator;
    public Animator preAnimator;
    public BossStat stat;

    // FSM
    public BossStateMachine fsm { get; private set; }
    // States
    IdleState idle;
    ChaseState chase;
    ChooseState choose;
    AttackDashState atkDash;
    AttackRangedState atkRanged;
    RecoverState recover;
    DeadState dead;

    // 쿨다운
    float readyDash, readyRanged;

    // ── Perception 캐시(계산형) ──
    bool canSee;
    float distCache = Mathf.Infinity;
    float pAcc;
    bool canHurt = true;

    // 캐시 접근자
    public float Dist => distCache;
    bool InAggro => Dist <= stat.detectRange;

    [SerializeField] GameObject preAttack;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!stat) stat = GetComponent<BossStat>();

        // Player 태그 자동 할당(수동 지정되어 있으면 유지)
        if (!player)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) player = go.transform;
        }

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
        if (CharacterManager.instance) CharacterManager.instance.Boss = GetComponent<Boss>();
        fsm.Change(idle);
    }

    void Update()
    {
  // ── Perception 주기 업데이트──
        pAcc += Time.deltaTime;
        if (pAcc >= 1f / Mathf.Max(0.1f, stat.perceptionHz))
        {
            pAcc = 0f;
            UpdatePerception2D();
        }

        // 인식범위 밖이면 어떤 상태든 즉시 Idle
        if (!InAggro && fsm.Current != null && fsm.Current.Name != "Idle")
        {
            StopMove();
            fsm.Change(idle, reason: "Force");
            return;
        }

        // 평상시 틱
        fsm.Tick(Time.deltaTime);
    }

    void FixedUpdate() => fsm.FixedTick(Time.fixedDeltaTime);

    // ─────────────────────────────
    // Perception(계산형) : 거리만으로도 충분(원하면 FOV/LoS 추가 가능)
    // ─────────────────────────────
    void UpdatePerception2D()
    {
        if (!player) { canSee = false; distCache = Mathf.Infinity; return; }
        distCache = Vector2.Distance(transform.position, player.position);
        canSee = distCache <= stat.detectRange; // 간단 버전: 범위 내면 "볼 수 있음"
    }

    // ─────────────────────────────
    // 공용 유틸 (상태에서 호출)
    // ─────────────────────────────
    public bool CanSeePlayer() => canSee;
    public bool CDReadyDash() => Time.time >= readyDash;
    public bool CDReadyRanged() => Time.time >= readyRanged;
    public void StartCD_Dash() => readyDash = Time.time + stat.dashCooldown;
    public void StartCD_Ranged() => readyRanged = Time.time + stat.rangedCooldown;

    public void MoveTowards(Vector2 pos, float speed)
    {
        var dir = (pos - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
    }
    public void StopMove() => rb.velocity = Vector2.zero;

    public void FaceToPlayer()
    {
        if (!player) return;
        var dir = (player.position - transform.position);
    }

    public void Play(string trigger)
    {
        if (!animator || string.IsNullOrEmpty(trigger)) return;
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
    }
    public void OnPreAttackEffect()
    {
        preAttack.SetActive(true);
        StartCoroutine(OnPreAttackRoutine());
    }
    IEnumerator OnPreAttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        preAttack.SetActive(false);
    }

    // 투사체 스폰(기즈모와 일치하도록 stat.bulletSpeed 사용)
    public void FireProjectile()
    {
        if (!stat.bulletPrefab) return;
        Vector3 spawnPos = stat.firePoint ? stat.firePoint.position : transform.position;
        var go = Instantiate(stat.bulletPrefab, spawnPos, Quaternion.identity);

        if (player && go.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            Vector2 dir = (player.position - spawnPos).normalized;
            rb2.velocity = dir * stat.bulletSpeed; // ← 속도 통일
        }
        // 수명은 발사체 스크립트에서 bulletLifetime을 참고해 Destroy하도록 권장
    }

    public void ToDie()
    {
        Destroy(this.gameObject, 3f);
    }

    public void TakeDamage(float damage)
    {
        if (canHurt == true)
        {
            canHurt = false;
            stat.hp01 -= damage;
            Play("TakeDamage");
            StartCoroutine(OnTakeDamageRoutine());
        }
    }
    IEnumerator OnTakeDamageRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        canHurt = true;
    }

    public void ApplyDamage()
    {
        
    }


    // 상태 접근자(내부 전이용)
    public IdleState SIdle => idle;
    public ChaseState SChase => chase;
    public ChooseState SChoose => choose;
    public AttackDashState SAtkDash => atkDash;
    public AttackRangedState SAtkRng => atkRanged;
    public RecoverState SRecover => recover;
    public DeadState SDead => dead;

    // ─────────────────────────────
    // Gizmos (거리/사정/대시/투사체 예상거리)
    // ─────────────────────────────
    void OnDrawGizmosSelected()
    {
        // 레퍼런스가 에디터에서 비어있을 수 있으므로 안전 체크
        var s = stat ? stat : GetComponent<BossStat>();
        if (!s) return;

        // 중심점
        Vector3 c = transform.position;

        // 1) 범위 링
        // detectRange : 노랑, nearRange : 초록, farRange : 파랑
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(c, s.detectRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(c, s.nearRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(c, s.farRange);

        // 2) 돌진 예상 이동거리(마젠타 라인)
        float dashDist = s.dashSpeed * s.dashActive;
        Vector3 dashDir = (transform.localScale.x >= 0) ? Vector3.right : Vector3.left;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(c, c + dashDir * dashDist);
        // 화살표 머리
        Vector3 tip = c + dashDir * dashDist;
        Gizmos.DrawLine(tip, tip + Quaternion.Euler(0, 0, 150) * dashDir * 0.4f);
        Gizmos.DrawLine(tip, tip + Quaternion.Euler(0, 0, -150) * dashDir * 0.4f);

        // 3) 투사체 예상 이동거리(시안 라인)
        if (s.firePoint)
        {
            float projDist = s.bulletSpeed * s.bulletLifetime;
            Vector3 fp = s.firePoint.position;

            // 기본은 바라보는 방향, 플레이어가 있으면 플레이어 방향으로 표시
            Vector3 pDir = (player ? (player.position - fp).normalized
                                   : ((transform.localScale.x >= 0) ? Vector3.right : Vector3.left));
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(fp, fp + pDir * projDist);
            // 화살표 머리
            Vector3 tip2 = fp + pDir * projDist;
            Gizmos.DrawLine(tip2, tip2 + Quaternion.Euler(0, 0, 150) * pDir * 0.35f);
            Gizmos.DrawLine(tip2, tip2 + Quaternion.Euler(0, 0, -150) * pDir * 0.35f);
        }
    }
}
