using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerStatObserver : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private float pollInterval = 0.05f; // 20Hz 감시
    [SerializeField] private int maxGauge = 5;           // UI 기준

    // UI가 구독할 이벤트
    public event Action<int, int> OnHealthChanged; // (현재, 최대)
    public event Action<int, int> OnGaugeChanged;  // (현재, 최대)

    // private 필드 리플렉션 캐시
    private FieldInfo fiCurrentHealth;
    private FieldInfo fiMaxHealth;

    private int lastHealth, lastMaxHealth, lastGauge;

    private void Awake()
    {
        if (!playerStat) playerStat = FindObjectOfType<PlayerStat>();

        var t = typeof(PlayerStat);
        fiCurrentHealth = t.GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        fiMaxHealth = t.GetField("maxHealth", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private void OnEnable()
    {
        StartCoroutine(PollLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator PollLoop()
    {
        // 초기 브로드캐스트
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
        if (fiCurrentHealth == null || playerStat == null) return lastHealth;
        float f = (float)fiCurrentHealth.GetValue(playerStat);
        return Mathf.Clamp(Mathf.RoundToInt(f), 0, ReadMaxHealth());
    }

    private int ReadMaxHealth()
    {
        if (fiMaxHealth == null || playerStat == null) return lastMaxHealth > 0 ? lastMaxHealth : 5;
        float f = (float)fiMaxHealth.GetValue(playerStat);
        return Mathf.Max(1, Mathf.RoundToInt(f));
    }

    private int ReadGauge()
    {
        if (playerStat == null) return lastGauge;
        return Mathf.Clamp(playerStat.Gauge, 0, maxGauge);
    }

    // 외부(특수공격 입력 등)에서 접근할 수 있도록 공개
    public PlayerStat Stat => playerStat;
    public int MaxGauge => maxGauge;
}
