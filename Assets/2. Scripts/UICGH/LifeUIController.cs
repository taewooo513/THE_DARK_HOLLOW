using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour
{
    [Tooltip("왼쪽→오른쪽 순서로, 현재 살아있는 칸들이 들어있는 배열(최대 5개)")]
    [SerializeField] private List<LifeIcon> lifeIcons = new List<LifeIcon>();

    [Tooltip("플레이어 스탯 관찰자(자동 탐색됨)")]
    [SerializeField] private PlayerStatObserver observer;

    // ====== 추가: 주기 탐색 간격 ======
    [SerializeField] private float findInterval = 0.5f;

    private bool subscribed;

    private void Awake()
    {
        if (!observer) observer = FindObjectOfType<PlayerStatObserver>();
    }

    private void OnEnable()
    {
        // 즉시 구독 시도
        TrySubscribe(observer);
        // 그리고 주기 감시 시작
        StartCoroutine(ObserverWatchdog());
    }

    private void OnDisable()
    {
        TryUnsubscribe();
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator ObserverWatchdog()
    {
        var wait = new WaitForSeconds(findInterval);

        while (enabled)
        {
            // 참조가 없거나 파괴되었으면 재탐색
            if (observer == null)
            {
                var found = FindObjectOfType<PlayerStatObserver>();
                if (found != null)
                {
                    observer = found;
                    TrySubscribe(observer); // 새로 찾으면 즉시 구독
                }
            }
            else
            {
                // 참조는 있는데 구독이 빠졌으면 복구
                if (!subscribed)
                    TrySubscribe(observer);

                // 옵저버가 비활성/파괴되었으면 해제
                if (!observer || !observer.isActiveAndEnabled)
                    TryUnsubscribe();
            }

            yield return wait;
        }
    }

    private void TrySubscribe(PlayerStatObserver target)
    {
        if (target == null || subscribed) return;
        target.OnHealthChanged += HandleHealthChanged;
        subscribed = true;
    }

    private void TryUnsubscribe()
    {
        if (!subscribed) return;
        if (observer != null)
            observer.OnHealthChanged -= HandleHealthChanged;
        subscribed = false;
        observer = null;
    }

    private void HandleHealthChanged(int current, int max)
    {
        // 현재 예시는 체력이 줄어들 때만 오른쪽부터 제거하는 로직이 들어있었음.
        // 필요하면 체력 증가(아이콘 생성)도 여기서 다루면 됨(프리팹/풀 필요).
        while (lifeIcons.Count > current)
        {
            int last = lifeIcons.Count - 1;
            var icon = lifeIcons[last];
            lifeIcons.RemoveAt(last);
            if (icon) icon.IconDestroy();
        }
    }
}
