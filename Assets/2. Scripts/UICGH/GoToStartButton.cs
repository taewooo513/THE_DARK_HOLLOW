using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToStartButton : MonoBehaviour
{
    public string StartSceneCGH = "StartSceneCGH";

    public string StartSceneAfterClear = "StartSceneAfterClear";

    public void LoadScene()
    {
        bool cleared = (BossDeadWatcher.I != null) && BossDeadWatcher.I.IsBossCleared;
        string target = cleared ? StartSceneAfterClear : StartSceneCGH;

        SceneManager.LoadScene(target);
    }
}
