using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    // ��ư�� ������ �Լ�
    public void Timestop()
    {
        Time.timeScale = 0f;  // ����
    }

    public void Timerelease()
    {
        Time.timeScale = 1f;  // �ٽ� ����
    }

}
