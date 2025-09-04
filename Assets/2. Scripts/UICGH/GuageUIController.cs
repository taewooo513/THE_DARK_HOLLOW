using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageUIController : MonoBehaviour
{
    [SerializeField] private PlayerStatObserver observer;

    [Header("게이지 표시 타겟")]
    [SerializeField] private Image uiImage;                 // Canvas UI용
    [SerializeField] private SpriteRenderer spriteRenderer; // 월드 스페라이트용

    [Header("게이지 단계 스프라이트 (0=빈 게이지, ... , 풀 게이지)")]
    [SerializeField] private List<Sprite> gaugeSprites = new();

    [Tooltip("가득 차면 활성화될 테두리 (초기 비활성화)")]
    [SerializeField] private GameObject borderHighlight;

    private int lastIndex = -1;

    private void Awake()
    {
        if (!observer) observer = FindObjectOfType<PlayerStatObserver>();
        if (borderHighlight) borderHighlight.SetActive(false);
    }

    private void OnEnable()
    {
        if (observer != null)
            observer.OnGaugeChanged += OnGaugeChanged;
    }

    private void OnDisable()
    {
        if (observer != null)
            observer.OnGaugeChanged -= OnGaugeChanged;
    }

    private void OnGaugeChanged(int current, int max)
    {
        // 게이지 값 → 스프라이트 인덱스 매핑
        int idx = MapToSpriteIndex(current, max);
        if (idx == lastIndex) return; // 변화 없으면 스킵

        // 스프라이트 교체
        Sprite s = (idx >= 0 && idx < gaugeSprites.Count) ? gaugeSprites[idx] : null;
        if (s != null)
        {
            if (uiImage) uiImage.sprite = s;
            if (spriteRenderer) spriteRenderer.sprite = s;
            lastIndex = idx;
        }

        // 풀게이지일 때만 테두리 ON
        if (borderHighlight) borderHighlight.SetActive(current >= max);
    }

    // gaugeSprites 개수와 max+1이 정확히 일치하면 1:1 매핑,
    // 그렇지 않으면 비율로 가장 가까운 프레임을 선택.
    private int MapToSpriteIndex(int current, int max)
    {
        if (gaugeSprites == null || gaugeSprites.Count == 0) return -1;

        current = Mathf.Clamp(current, 0, max);
        if (max <= 0) return 0;

        if (gaugeSprites.Count == max + 1)
        {
            // 예: max=5, current=3 -> index=3
            return current;
        }
        else
        {
            // 예: 스프라이트가 6장이 아닌 경우 비율로 근사 매핑
            float t = (float)current / max; // 0~1
            int idx = Mathf.RoundToInt(t * (gaugeSprites.Count - 1));
            return Mathf.Clamp(idx, 0, gaugeSprites.Count - 1);
        }
    }
}
