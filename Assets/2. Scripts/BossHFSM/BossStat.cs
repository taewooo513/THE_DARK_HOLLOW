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

    // 상태 변환 가중치 
    [Header("Utility")]
    [Range(0f, 1f)] public float minScoreToAct = 0.35f; // 이 점수 미만이면 공격하지 않고 거리 조절(Chase)
    [Range(0f, 0.5f)] public float stickiness = 0.15f;  // 직전 선택 패턴에 주는 보너스(플리커 방지)
    [Range(0f, 0.25f)] public float utilityNoise = 0.02f; // 미세 랜덤(동점 깨기)

    [Header("가우시안 폭(σ)을 선호거리의 몇 배로 둘지")]
    [Range(0.05f, 2f)] public float nearSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float midSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float farSigmaFrac = 0.6f;

    [Header("공격별 가중치(기본=1). 페이즈/디자인 의도에 따라 조절")]
    public float weightDash = 1f;
    public float weightMid = 1f;
    public float weightRanged = 1f;

    [Header("바닥 점수(멀어도 완전 0은 방지)")]
    [Range(0f, 0.5f)] public float baseBiasDash = 0.05f;
    [Range(0f, 0.5f)] public float baseBiasMid = 0.05f;
    [Range(0f, 0.5f)] public float baseBiasRanged = 0.05f;

    void OnValidate()
    {
        perceptionHz = Mathf.Max(0.1f, perceptionHz);
        detectRange = Mathf.Max(0f, detectRange);
        nearRange = Mathf.Max(0f, nearRange);
        farRange = Mathf.Max(0f, farRange);
        dashSpeed = Mathf.Max(0f, dashSpeed);
        bulletSpeed = Mathf.Max(0f, bulletSpeed);
        bulletLifetime = Mathf.Max(0f, bulletLifetime);
    }
}
