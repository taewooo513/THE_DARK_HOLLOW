using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialInput : MonoBehaviour
{
    [SerializeField] private PlayerStatObserver observer;

    private void Awake()
    {
        if (!observer) observer = FindObjectOfType<PlayerStatObserver>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && observer != null && observer.Stat != null)
        {
            // 가득 찼을 때만 특수공격 허용
            if (observer.Stat.Gauge >= observer.MaxGauge)
            {
                observer.Stat.Gauge = 0;
                // GaugeUI는 Observer가 변화 감지해서 테두리 끔
            }
        }
    }
}
