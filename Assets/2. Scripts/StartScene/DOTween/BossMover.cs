using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;       // 보스 실루엣 이동 속도
    public float setY = -8f;        // 아래쪽 시작점 (화면 밖)
    public float endY = 0f;           // 위쪽 도착점 (화면 안)

    [Header("DOTween")]
    public float distance = 0.5f;
    public float duration = 1f;

    private bool isFloating = false;

    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = setY;
        transform.position = pos;
    }

    void Update()
    {
        if (isFloating) return; // 반복 운동 들어가면 Update 멈춤

        // 위로 이동
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 도착점(endY)로 도달하면 정지
        if (transform.position.y >= endY)
        {
            Vector3 pos = transform.position;
            pos.y = endY;
            transform.position = pos;

            StartFloating();
            isFloating = true;
        }
    }

    void StartFloating()
    {
        // 위아래로 반복하는 DOTween 실행
        transform.DOMoveY(endY + distance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // 무한 반복 (위/아래)
    }
}
