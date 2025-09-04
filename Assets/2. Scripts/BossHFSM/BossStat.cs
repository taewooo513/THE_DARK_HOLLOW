using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("Aggro / Perception")]
    public float detectRange = 10f;   // 인식 범위(이 범위를 벗어나면 Idle 강제)
    public float nearRange = 1.8f;  // 돌진(근접) 임계
    public float midRange = 3.0f; // 중거리 임계
    public float farRange = 5.5f;  // 원거리 임계

    [Tooltip("시야/거리 계산 주기(Hz). 8~10 권장")]
    public float perceptionHz = 8f; // 눈깜빡임 주기

    [Header("Move Speeds")]
    public float moveSpeed = 3.5f;
    public float dashSpeed = 12f;

    [Header("Dash Timings")]
    public float dashWindup = 0.5f;
    public float dashActive = 0.28f;
    public float dashRecover = 0.35f;
    public float dashCooldown = 1.8f;

    [Header("Ranged Timings")]
    public float rangedWindup = 0.20f;
    public float rangedActive = 0.00f;     // 투사체면 Active=0도 OK
    public float rangedRecover = 0.35f;
    public float rangedCooldown = 1.4f;

    [Header("Ranged Projectile")]
    public Transform firePoint;

    [Tooltip("기즈모 미리보기용(발사 속도)")]
    public float bulletSpeed = 10f;         // FireProjectile에서도 사용하도록 권장
    [Tooltip("기즈모 미리보기용(투사체 생존 시간)")]
    public float bulletLifetime = 3f;       // 예상 이동거리 = speed * lifetime

    [Header("Phase / HP")]
    [Range(1, 100)] public int hp01;

    // 상태 변환 가중치 
    [Header("Utility / Selection")]
    [Range(0f, 1f)] public float minScoreToAct = 0.35f; // 이 점수 미만이면 공격하지 않고 거리 조절(Chase)
    [Range(0f, 0.5f)] public float stickiness = 0.15f;  // 직전 선택 패턴에 주는 보너스(플리커 방지)
    [Range(0f, 0.25f)] public float utilityNoise = 0.02f; // 미세 랜덤(동점 깨기)

    [Tooltip("가우시안 폭(σ)을 선호거리의 몇 배로 둘지")]
    [Range(0.05f, 2f)] public float nearSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float midSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float farSigmaFrac = 0.6f;

    [Tooltip("공격별 가중치(기본=1). 페이즈/디자인 의도에 따라 조절")]
    public float weightDash = 1f;
    public float weightMid = 1f;
    public float weightRanged = 1f;

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
