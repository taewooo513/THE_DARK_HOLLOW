using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour
{
    [Tooltip("왼쪽→오른쪽 순서로, 현재 살아있는 칸들이 들어있는 배열(최대 5개)")]
    [SerializeField] private List<LifeIcon> lifeIcons = new List<LifeIcon>();

    [Tooltip("플레이어 스탯 참조")]
    [SerializeField] private PlayerStatObserver observer;

    private void Awake()
    {
        if(!observer) observer = FindObjectOfType<PlayerStatObserver>();
    }

    private void OnEnable()
    {
        if (observer != null) observer.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (observer != null) observer.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int current, int max)
    {
        while (lifeIcons.Count > current)
        {
            int last = lifeIcons.Count - 1;
            var icon = lifeIcons[last];
            lifeIcons.RemoveAt(last);
            if (icon) icon.IconDestroy();
        }
        // 현재 살아있는 아이콘 수가 current가 되도록 맞춘다.
        // 줄어든 만큼 뒤쪽(오른쪽 끝)부터 터뜨린다.
        // 예) 5→4면 마지막 인덱스 4를 파괴

    }

}
