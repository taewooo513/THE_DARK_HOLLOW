using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerStatObserver : MonoBehaviour
{
    // ====== 기존 필드 ======
    public PlayerStat _playerStat;
    public PlayerStat PlayerStat { get { return _playerStat; } set { _playerStat = value; } }
    [SerializeField] private float pollInterval = 0.05f; // 20Hz 감시
    [SerializeField] private int maxGauge = 5;           // UI 기준

    // ====== 추가: 주기 탐색 간격 ======
    [SerializeField] private float findInterval = 0.5f;

    public event Action<int, int> OnHealthChanged; // (현재, 최대)
    public event Action<int, int> OnGaugeChanged;  // (현재, 최대)

    private FieldInfo fiCurrentHealth;
    private FieldInfo fiMaxHealth;

    private int lastHealth, lastMaxHealth, lastGauge;

    // ====== 내부 상태: 마지막으로 바인딩된 Stat 캐시 ======
    private PlayerStat lastBoundStat;

    private void Awake()
    {
        // 첫 시도
        if (!PlayerStat)
            PlayerStat = FindObjectOfType<PlayerStat>();

        if (PlayerStat != null)
            BindReflection(PlayerStat);
    }

    private void OnEnable()
    {
        StartCoroutine(PollLoop());
        StartCoroutine(FindLoop()); // ====== 추가: 주기적 재탐색/재바인딩 ======
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void BindReflection(PlayerStat target)
    {
        var t = typeof(PlayerStat);
        // PlayerStat의 private 'maxHealth'만 필요. (currentHealth는 공개 CurrentHealth로부터 읽어도 되지만,
        // 네 코드 스타일 유지해 리플렉션 접근 그대로 둠)
        fiCurrentHealth = t.GetField("CurrentHealth", BindingFlags.Public | BindingFlags.Instance)
                          ?? t.GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        fiMaxHealth = t.GetField("maxHealth", BindingFlags.NonPublic | BindingFlags.Instance);

        lastBoundStat = target;

        // 바인딩 직후, 즉시 한 번 브로드캐스트 (UI가 늦게 붙었어도 최신값 보장)
        lastMaxHealth = ReadMaxHealth();
        lastHealth = ReadHealth();
        lastGauge = ReadGauge();
        OnHealthChanged?.Invoke(lastHealth, lastMaxHealth);
        OnGaugeChanged?.Invoke(lastGauge, maxGauge);
    }

    private IEnumerator FindLoop()
    {
        var wait = new WaitForSeconds(findInterval);

        while (enabled)
        {
            // 1) 대상이 비었거나 파괴되었으면 재탐색
            if (PlayerStat == null)
            {
                var ps = FindObjectOfType<PlayerStat>();
                if (ps != null)
                {
                    PlayerStat = ps;
                    BindReflection(ps);
                }
            }
            else
            {
                // 2) 참조가 바뀌었거나(씬 재로딩 등) 리플렉션 핸들이 날아갔으면 재바인딩
                if (PlayerStat != lastBoundStat || fiMaxHealth == null || fiCurrentHealth == null)
                {
                    BindReflection(PlayerStat);
                }
            }

            yield return wait;
        }
    }

    private IEnumerator PollLoop()
    {
        // 초기 브로드캐스트 (BindReflection에서도 해주지만, 최초 Enable 때 안전망)
        lastMaxHealth = ReadMaxHealth();
        lastHealth = ReadHealth();
        lastGauge = ReadGauge();

        OnHealthChanged?.Invoke(lastHealth, lastMaxHealth);
        OnGaugeChanged?.Invoke(lastGauge, maxGauge);

        var wait = new WaitForSeconds(pollInterval);
        while (enabled)
        {
            int h = ReadHealth();
            int mh = ReadMaxHealth();
            int g = ReadGauge();

            if (h != lastHealth || mh != lastMaxHealth)
            {
                lastHealth = h;
                lastMaxHealth = mh;
                OnHealthChanged?.Invoke(h, mh);
            }

            if (g != lastGauge)
            {
                lastGauge = g;
                OnGaugeChanged?.Invoke(g, maxGauge);
            }

            yield return wait;
        }
    }

    private int ReadHealth()
    {
        if (PlayerStat == null) return lastHealth;

        // 공개 프로퍼티가 있으면 그걸 우선 사용
        var prop = typeof(PlayerStat).GetProperty("CurrentHealth");
        if (prop != null)
        {
            float f = Convert.ToSingle(prop.GetValue(PlayerStat));
            return Mathf.Clamp(Mathf.RoundToInt(f), 0, ReadMaxHealth());
        }

        if (fiCurrentHealth == null) return lastHealth;
        float v = Convert.ToSingle(fiCurrentHealth.GetValue(PlayerStat));
        return Mathf.Clamp(Mathf.RoundToInt(v), 0, ReadMaxHealth());
    }

    private int ReadMaxHealth()
    {
        if (PlayerStat == null) return lastMaxHealth > 0 ? lastMaxHealth : 5;
        if (fiMaxHealth == null) return lastMaxHealth > 0 ? lastMaxHealth : 5;

        float f = Convert.ToSingle(fiMaxHealth.GetValue(PlayerStat));
        return Mathf.Max(1, Mathf.RoundToInt(f));
    }

    private int ReadGauge()
    {
        if (PlayerStat == null) return lastGauge;
        return Mathf.Clamp(PlayerStat.Gauge, 0, maxGauge);
    }

    // 외부 접근용
    public PlayerStat Stat => PlayerStat;
    public int MaxGauge => maxGauge;
}