using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    // 버튼에 연결할 함수
    public void Timestop()
    {
        Time.timeScale = 0f;  // 멈춤
    }

    public void Timerelease()
    {
        Time.timeScale = 1f;  // 다시 실행
    }

}
