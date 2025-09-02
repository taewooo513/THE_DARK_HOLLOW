using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("Aggro / Perception")]
    public float detectRange = 10f;   // 인식 범위(이 범위를 벗어나면 Idle 강제)
    public float nearRange = 1.8f;  // 돌진(근접) 임계
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
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Tooltip("기즈모 미리보기용(발사 속도)")]
    public float bulletSpeed = 10f;         // FireProjectile에서도 사용하도록 권장
    [Tooltip("기즈모 미리보기용(투사체 생존 시간)")]
    public float bulletLifetime = 2f;       // 예상 이동거리 = speed * lifetime

    [Header("Phase / HP")]
    [Range(100, 10000)] public float hp01 = 100;   

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
