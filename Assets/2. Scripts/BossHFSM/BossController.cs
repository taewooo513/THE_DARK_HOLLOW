using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
public class BossController : MonoBehaviour
{
    public enum AttackChoice
    {
        None,
        Dash,
        Ranged,
        Mid
    }

    [HideInInspector] public AttackChoice lastChosen = AttackChoice.None;   // �������� ����

    [HideInInspector] public Transform player;              // Ÿ��(������ �ڵ����� Player �±� �˻�)
    [HideInInspector] public BossStat stat;
    [HideInInspector] public Rigidbody2D rb;
    // FSM
    [HideInInspector] public BossStateMachine fsm { get; private set; }

    // =================[States]=================
    IdleState idle;
    ChooseState choose;
    AttackDashState atkDash;
    AttackMidState atkMid;
    AttackRangedState atkRanged;
    DeadState dead;
    public IdleState SIdle => idle;
    public ChooseState SChoose => choose;
    public AttackDashState SAtkDash => atkDash;
    public AttackMidState SAtkMid => atkMid;
    public AttackRangedState SAtkRng => atkRanged;
    public DeadState SDead => dead;

    // ��ٿ�
    float readyDash, readyMid, readyRanged;

    // ������
    bool canSee;
    bool canHurt = true;
    bool isDead = false;
    bool secondPhase = false;
    public bool IsDead => isDead;
    float pAcc;
    float nextDecisionReadyAt = 0.5f;
    int hp01;
    SpriteRenderer spriteRenderer;

    Animator animator;

    float distCache = Mathf.Infinity;
    public float Dist => distCache;
    bool InAggro => Dist <= stat.detectRange;// == { get { return Dist <= stat.detectRange; } 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stat = GetComponent<BossStat>();
        spriteRenderer= GetComponent<SpriteRenderer>();
        fsm = new BossStateMachine();

        // =================���� �ν��Ͻ�(���ο� ���°� �߰��Ǹ� ���⿡ �߰�)=================
        idle = new IdleState(this, fsm);
        choose = new ChooseState(this, fsm);
        atkDash = new AttackDashState(this, fsm);
        atkMid = new AttackMidState(this, fsm);
        atkRanged = new AttackRangedState(this, fsm);
        dead = new DeadState(this, fsm);
    }

    void Start()
    {

        if (CharacterManager.instance) CharacterManager.instance.Boss = GetComponent<Boss>();
        // Player �±� �ڵ� �Ҵ�
        if (!player)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) player = go.transform;
        }
        fsm.Change(idle);
        hp01 = stat.hp01;

        // ����ü ��ų Ǯ�� �ʱ�ȭ
        ObjectPoolingManager.Instance.InsertPoolQueue("Skill_3", 5);
        ObjectPoolingManager.Instance.InsertPoolQueue("Skill_6", 5);
    }

    void Update()
    {
        if (isDead) return;

        // ----�ֱ� ������Ʈ----
        pAcc += Time.deltaTime;
        if (pAcc >= 1f / Mathf.Max(0.1f, stat.perceptionHz))    // üũ ���� �ֱ� ����(�ּ� 0.1����), 1�ʿ� 0.1f~stat.perceptionHz���� �����Ӻ���)
        {
            pAcc = 0f;
            UpdatePerception2D();
        }

        // �νĹ��� ���̸� � ���µ� ��� Idle
        if (!InAggro && fsm.Current != null && fsm.Current.Name != "Idle")
        {
            StopMove();
            fsm.Change(idle, reason: "Force");
            return;
        }

        if (!secondPhase && hp01 <= Mathf.CeilToInt(stat.hp01 * 0.5f))
        {
            StartCoroutine(EnterSecondPhaseRoutine());
        }
        fsm.Tick(Time.deltaTime);
    }

    void FixedUpdate()
    {
        fsm.FixedTick(Time.fixedDeltaTime);
    }
    //2������ ���� ����
    IEnumerator EnterSecondPhaseRoutine()
    {
        secondPhase = true;

        // ª�� ��
        StopMove();
        nextDecisionReadyAt = 3f;        // ���� �ð���ŭ �ǻ���� ���� �༭ �ٷ� ���� �� ������
        fsm.Change(idle, reason: "Force");
        AnimationPlay_Trigger("SecondPhase");
        spriteRenderer.color = new Color(1f, 0f, 0f);
        // �ʿ��ϸ� ���⼭ ��� ��ٸ��� (�ִ� ���̿� ���� ����)
        yield return new WaitForSeconds(2f);
        nextDecisionReadyAt = 0.5f;
    }

    // �þ� ó�� ����
    void UpdatePerception2D()
    {
        if (!player)
        {
            canSee = false;
            distCache = Mathf.Infinity;  // �÷��̾ ������ ���Ѵ�
            return;
        }
        distCache = Vector2.Distance(transform.position, player.position);  // �÷��̾�� ������ �Ÿ� ĳ��
        canSee = distCache <= stat.detectRange;
    }

    // ���� ��ƿ (���¿��� ȣ��)
    public bool CanSeePlayer() => canSee;
    public bool DecisionReady() => Time.time >= nextDecisionReadyAt;
    public void StartDecisionDelay() => nextDecisionReadyAt = Time.time + (secondPhase ? SetSecondPhase(stat.decisionDelay) : stat.decisionDelay);
    public bool CDReadyDash() => Time.time >= readyDash;
    public bool CDReadyMid() => Time.time >= readyMid;
    public bool CDReadyRanged() => Time.time >= readyRanged;
    public void StartCD_Dash() => readyDash = Time.time + (secondPhase ? SetSecondPhase(stat.dashCooldown) : stat.dashCooldown);
    public void StartCD_Mid() => readyMid = Time.time + (secondPhase ? SetSecondPhase(stat.midCooldown) : stat.midCooldown); 
    public void StartCD_Ranged() => readyRanged = Time.time + (secondPhase ? SetSecondPhase(stat.rangedCooldown) : stat.rangedCooldown);
    public void StopMove() => rb.velocity = Vector2.zero;       // ���ڸ� ����

    float SetSecondPhase(float v) => secondPhase ? v * 0.5f : v; // 2������� ��ٿ� ����


    // �ִϸ��̼� ���
    public void AnimationPlay_Trigger(string trigger)
    {
        if (!animator || string.IsNullOrEmpty(trigger)) return;
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
    }

    // ��������������������������������������������������������������������������������������������������������������������������
    // [Attack���� ��ƿ �Լ���]
    // ��������������������������������������������������������������������������������������������������������������������������

    // ---------------------------[RushAttack]---------------------------
    public void OnPreAttackEffect()
    {
        stat.preAttack.SetActive(true);
        StartCoroutine(OnPreAttackRoutine());
    }

    IEnumerator OnPreAttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        stat.preAttack.SetActive(false);
    }

    // ---------------------------[MidAttack]---------------------------
    // �÷��̾ �ٶ󺸰� ��
    public void FaceToPlayerFireMidAttack()
    {
        if (!player || !stat.midAttackPrefab) return;

        // �÷��̾ ���� ���� ����/���������� �Ǵ�
        float dx = player ? (player.position.x - transform.position.x) : (transform.localScale.x >= 0 ? 1f : -1f);
        bool playerLeft = dx < 0f;

        // �����̸� -, �������̸� +
        float targetX = playerLeft ? -stat.midOffsetX : stat.midOffsetX;
        stat.midAttackPrefab.transform.localPosition = new Vector2(targetX, -0.95f);
        stat.midAttackPrefab.SetActive(true);
    }

    public void OnMidAttackEffect()
    {
        stat.midAttackPrefab.SetActive(true);
        StartCoroutine(OnMidAttackRoutine());
    }

    IEnumerator OnMidAttackRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        stat.midAttackPrefab.SetActive(false);
    }

    // ---------------------------[RangedAttack]---------------------------
    // ����ü ����
    public void FireProjectile()
    {
        Vector3 spawnPos = stat.firePoint ? stat.firePoint.position : transform.position;
        float dx = player ? (player.position.x - spawnPos.x) : (transform.localScale.x >= 0 ? 1f : -1f);
        bool faceLeft = dx < 0f;
        //�θ� ������Ʈ�� �ش� �������� ȸ���� ���� (Y=0 �� -180)
        Quaternion rot = Quaternion.Euler(0f, faceLeft ? -180f : 0f, 0f);
        if (secondPhase)
        {
            FireBullt2(faceLeft, spawnPos, rot);
        }
        else 
        {
            FireBullt1(faceLeft, spawnPos, rot);
        }
    }
    void FireBullt1(bool faceLeft, Vector3 spawnPos, Quaternion rot)
    {
        GameObject go = ObjectPoolingManager.Instance.AddObject("Skill_3", spawnPos, rot);

        if (player && go.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            rb2.velocity = (faceLeft ? Vector2.left : Vector2.right) * stat.bulletSpeed;
        }
        StartCoroutine(DestroyBulltRoutine(go));
    }
    IEnumerator DestroyBulltRoutine(GameObject go)
    {
        yield return new WaitForSeconds(stat.bulletLifetime);
        ObjectPoolingManager.Instance.DestoryObject("Skill_3", go);
    }

    //2�������
    void FireBullt2(bool faceLeft, Vector3 spawnPos, Quaternion rot)
    {
        StartCoroutine(BulltRoutine(faceLeft, spawnPos, rot));
    }
    IEnumerator BulltRoutine(bool faceLeft, Vector3 spawnPos, Quaternion rot)
    {
        GameObject go1 = ObjectPoolingManager.Instance.AddObject("Skill_6", spawnPos, rot);
        if (player && go1.TryGetComponent<Rigidbody2D>(out var rb1))
        {
            rb1.velocity = (faceLeft ? Vector2.left : Vector2.right) * stat.bulletSpeed;
        }
        yield return new WaitForSeconds(1f);
        GameObject go2 = ObjectPoolingManager.Instance.AddObject("Skill_3", spawnPos, rot);
        if (player && go2.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            rb2.velocity = (faceLeft ? Vector2.left : Vector2.right) * stat.bulletSpeed;
        }
        yield return new WaitForSeconds(stat.bulletLifetime);
        ObjectPoolingManager.Instance.DestoryObject("Skill_6", go1);
        yield return new WaitForSeconds(0.5f);
        ObjectPoolingManager.Instance.DestoryObject("Skill_3", go2);
    }


    // ---------------------------[���� �������� �ݶ��̴� ��ȣ�ۿ� ����]---------------------------
    public void ToDie()
    {
        StartCoroutine(DieRoutien());
    }
    IEnumerator DieRoutien()
    {
        yield return new WaitForSeconds(3f);
        hp01 = stat.hp01;
        SceneLoadManager.instance.LoadScene("StartSceneAfterClear");
    }
    public void TakeDamage(int damage, GameObject _object)
    {
        if (canHurt == true)
        {
            canHurt = false;
            hp01 -= damage;
            SoundManager.Instance.PlayEFXSound("BossHit_EFX");
            Debug.Log($"���� ü�� : {hp01}");

            if (hp01 <= 0)  //����
            {
                isDead = true;
                secondPhase = false;
                spriteRenderer.color = new Color(1f, 1f, 1f);
                // ��𼭵� ������ ���� Dead ���·� ��ȯ
                if (isDead && fsm.Current != null && fsm.Current.Name != "Dead")
                {
                    StopMove();
                    fsm.Change(dead, reason: "Force");
                    return;
                }
            }
            else
            {
                AnimationPlay_Trigger("TakeDamage");
                StartCoroutine(OnTakeDamageRoutine(_object));
            }
        }
    }

    IEnumerator OnTakeDamageRoutine(GameObject _object)
    {
         _object.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        canHurt = true;
        _object.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Boss Hit Player");
            CharacterManager.instance.PlayerStat.TakeDamage();
        }
    }

    // ---------------------------[Gizmos (�Ÿ�/����/���/����ü ����Ÿ�)]---------------------------
    void OnDrawGizmosSelected()
    {
        // ���۷����� �����Ϳ��� ������� �� �����Ƿ� ���� üũ
        var s = stat ? stat : GetComponent<BossStat>();
        if (!s) return;

        // �߽���
        Vector3 c = transform.position;

        // 1) ���� ��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(c, s.detectRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(c, s.nearRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(c, s.midRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(c, s.farRange);
        
        // 2) ���� ���� �̵��Ÿ�
        float dashDist = s.dashSpeed * s.dashActive;
        Vector3 dashDir = (transform.localScale.x >= 0) ? Vector3.right : Vector3.left;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(c, c + dashDir * dashDist);

        // ȭ��ǥ �Ӹ�
        Vector3 tip = c + dashDir * dashDist;
        Gizmos.DrawLine(tip, tip + Quaternion.Euler(0, 0, 150) * dashDir * 0.4f);
        Gizmos.DrawLine(tip, tip + Quaternion.Euler(0, 0, -150) * dashDir * 0.4f);

        // 3) ����ü ���� �̵��Ÿ�(�þ� ����)
        if (s.firePoint)
        {
            float projDist = s.bulletSpeed * s.bulletLifetime;
            Vector3 fp = s.firePoint.position;

            // �⺻�� �ٶ󺸴� ����, �÷��̾ ������ �÷��̾� �������� ǥ��
            Vector3 pDir = (player ? (player.position - fp).normalized
                                   : ((transform.localScale.x >= 0) ? Vector3.right : Vector3.left));
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(fp, fp + pDir * projDist);
            // ȭ��ǥ �Ӹ�
            Vector3 tip2 = fp + pDir * projDist;
            Gizmos.DrawLine(tip2, tip2 + Quaternion.Euler(0, 0, 150) * pDir * 0.35f);
            Gizmos.DrawLine(tip2, tip2 + Quaternion.Euler(0, 0, -150) * pDir * 0.35f);
        }
    }
}
