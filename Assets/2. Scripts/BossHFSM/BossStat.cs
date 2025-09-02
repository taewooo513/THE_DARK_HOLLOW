using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("Aggro / Perception")]
    public float detectRange = 10f;   // 인식 범위(이 범위를 벗어나면 Idle 강제)
    public float nearRange = 1.8f;    // 돌진(근접) 임계
    public float farRange = 5.5f;    // 원거리 임계

    [Header("Move Speeds")]
    public float moveSpeed = 3.5f;
    public float dashSpeed = 12f;

    [Header("Dash Timings")]
    public float dashWindup = 0.20f;
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

    [Header("Phase / HP")]
    [Range(0, 1)] public float hp01 = 1f;    // 0~1 정규화 HP (원하면 외부에서 갱신)
}
