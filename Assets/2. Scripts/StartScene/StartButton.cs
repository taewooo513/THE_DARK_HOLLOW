using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // �ν����Ϳ��� ��� �ҷ�����
    public string MainSceneCGH;

    // ��ư����  �Լ� ȣ��
    public void LoadScene()
    {
        SceneLoadManager.Instance.LoadScene(SceneKey.mainScene);
    }

}
