using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // 인스펙터에서 장면 불러오기
    public string MainScene;

    // 버튼에서  함수 호출
    public void LoadScene()
    {
        SceneLoadManager.Instance.LoadScene(SceneKey.mainScene);
    }

}
