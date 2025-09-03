using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("직렬화요소")]
    [SerializeField] Animator preAnimator;
    [SerializeField] GameObject preAttack;

    public Transform player;              // 타겟(없으면 자동으로 Player 태그 검색)
    public BossStat stat;
    public Rigidbody2D rb;
    // FSM
    public BossStateMachine fsm { get; private set; }

    // =================[States]=================
    IdleState idle;
    ChaseState chase;
    ChooseState choose;
    AttackDashState atkDash;
    AttackRangedState atkRanged;
    RecoverState recover;
    DeadState dead;

    // 쿨다운
    float readyDash, readyRanged;

    // 변수들
    bool canSee;
    bool canHurt = true;
    float pAcc;
    float hp01;


    Animator animator;

    float distCache = Mathf.Infinity;
    public float Dist => distCache;
    bool InAggro => Dist <= stat.detectRange;

    public IdleState SIdle => idle;
    public ChaseState SChase => chase;
    public ChooseState SChoose => choose;
    public AttackDashState SAtkDash => atkDash;
    public AttackRangedState SAtkRng => atkRanged;
    public RecoverState SRecover => recover;
    public DeadState SDead => dead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        stat = GetComponent<BossStat>();

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
        hp01 = stat.hp01;
    }

    void Update()
    {
        // ----주기 업데이트----
        pAcc += Time.deltaTime;
        if (pAcc >= 1f / Mathf.Max(0.1f, stat.perceptionHz))    // 체크 간격 주기 연산(최소 0.1보장), 1초에 0.1f~stat.perceptionHz까지 프레임보간)
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

    // 시야 처리 로직
    void UpdatePerception2D()
    {
        if (!player) 
        { 
            canSee = false; 
            distCache = Mathf.Infinity; 
            return; 
        }
        distCache = Vector2.Distance(transform.position, player.position);  // 플레이어와 보스간 거리 캐싱
        canSee = distCache <= stat.detectRange; 
    }

    // 공용 유틸 (상태에서 호출)
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

    // 투사체 스폰
    public void FireProjectile()
    {
        if (!stat.bulletPrefab) return;
        Vector3 spawnPos = stat.firePoint ? stat.firePoint.position : transform.position;
        float dx = player ? (player.position.x - spawnPos.x)
                          : (transform.localScale.x >= 0 ? 1f : -1f);
        bool faceLeft = dx < 0f;

        // 2) 부모 오브젝트를 해당 방향으로 회전해 생성 (Y=0 ↔ -180)
        Quaternion rot = Quaternion.Euler(0f, faceLeft ? -180f : 0f, 0f);
        GameObject go = Instantiate(stat.bulletPrefab, spawnPos, rot); // 풀링 쓰면 교체

        if (player && go.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            rb2.velocity = (faceLeft ? Vector2.left : Vector2.right) * stat.bulletSpeed;
/*            Vector2 dir = (player.position - spawnPos).normalized;
            rb2.velocity = dir * stat.bulletSpeed; // ← 속도 통일*/
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
            hp01 -= damage;
            if (hp01 <= 0)
            {
                hp01 = 0;
                fsm.Change(dead);
            }
            else
            {
                Play("TakeDamage");
                StartCoroutine(OnTakeDamageRoutine());
            }
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


    // Gizmos (거리/사정/대시/투사체 예상거리)
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
