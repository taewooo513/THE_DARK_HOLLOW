using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStartButton : MonoBehaviour
{
    public string StartSceneCGH;

    public void LoadScene()
    {
        SceneLoadManager.Instance.LoadScene(SceneKey.mainScene);
    }
}
