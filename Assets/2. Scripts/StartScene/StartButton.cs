using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // �ν����Ϳ��� ��� �ҷ�����
    public string MainScene;

    // ��ư����  �Լ� ȣ��
    public void LoadScene()
    {
        SceneLoadManager.Instance.LoadScene(SceneKey.stage1Scene);
    }

}
