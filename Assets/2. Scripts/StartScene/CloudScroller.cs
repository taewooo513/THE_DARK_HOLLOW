using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    public float moveSpeed = 2f;       // 구름 이동 속도
    public float resetX = -15f;        // 왼쪽으로 재배치될 위치 (화면 밖)
    public float endX = 15f;           // 오른쪽으로 나갔는지 확인할 위치 (화면 밖)

    void Update()
    {
        // 오른쪽으로 이동
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // 화면 밖(endX)으로 완전히 나가면 왼쪽(resetX)으로 이동
        if (transform.position.x > endX)
        {
            Vector3 pos = transform.position;
            pos.x = resetX;
            transform.position = pos;
        }
    }
}
