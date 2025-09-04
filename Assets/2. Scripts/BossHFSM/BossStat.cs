using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("애니메이션용")]
    [Tooltip("예고동작 애니메이션 프리팹")]
    public GameObject preAttack;
    [Tooltip("피격 애니메이션 프리팹")]
    public GameObject hitEffect;

    [Header("다음 패턴 딜레이")]
    [Tooltip("어떤 공격 후에도 다음 선택까지 최소 대기")]
    public float decisionDelay = 1.5f;

    [Header("보스 범위(인식, 공격)")]
    [Tooltip("인식 범위(노랑색)")]
    public float detectRange = 10f;   // (이 범위를 벗어나면 Idle 강제)
    [Tooltip("돌진패턴 범위(초록색)")]
    public float nearRange = 1.8f;
    [Tooltip("중거리 공격 범위(빨간색)")]
    public float midRange = 3.0f;
    [Tooltip("원거리 공격 범위(파란색)")]
    public float farRange = 5.5f;

    [Header("보스 인식 주기")]
    [Tooltip("시야/거리 계산 주기(Hz). 8~10 권장")]
    public float perceptionHz = 8f; // 눈깜빡임 주기

    [Header("돌진공격 속성 설정")]
    public float dashSpeed = 12f;
    public float dashWindup = 0.5f;
    public float dashActive = 0.28f;    // 돌진 지속시간
    public float dashRecover = 0.35f;
    public float dashCooldown = 8f;

    [Header("중거리 공격 속성 설정")]
    public float midWindup = 0.25f;
    public float midActive = 0.0f;
    public float midRecover = 0.35f;
    public float midCooldown = 5f;
    [Tooltip("중거리 공격 프리팹(위치변환용)")]
    public GameObject midAttackPrefab;
    [Tooltip("중거리 공격 프리팹 위치 오프셋")]
    public float midOffsetX = 2.2f;

    [Header("원거리 공격 설정")]
    public float rangedWindup = 0.20f;
    public float rangedActive = 0.00f;     // 투사체면 0도 OK
    public float rangedRecover = 0.35f;
    public float rangedCooldown = 1.4f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;       // 예상 이동거리 = speed * lifetime
    [Tooltip("원거리공격 포인트(시작 위치용)")]
    public Transform firePoint;

    [Header("HP")]
    [Range(1, 100)] public int hp01;

}
