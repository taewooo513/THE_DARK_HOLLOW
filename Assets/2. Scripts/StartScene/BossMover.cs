using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;       // 보스 실루엣 이동 속도
    public float setY = -8f;        // 아래쪽 시작점 (화면 밖)
    public float endY = 0f;           // 위쪽 도착점 (화면 안)

    void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 도착점(endY)로 도달하면 정지
        if (transform.position.y >= endY)
        {
            Vector3 pos = transform.position;
            pos.y = endY;
            transform.position = pos;
        }
    }
}
