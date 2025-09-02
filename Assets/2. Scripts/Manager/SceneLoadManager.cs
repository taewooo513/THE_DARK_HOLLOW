using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class SceneKey
{
    public const string mainScene = "TaewoongTest";
    public const string titleScene = "TitleScene";
    public const string endingScene = "EndingScene";
}
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    Dictionary<string, MonoScene> scenes;
    // Start is called before the first frame update
    MonoScene nowScene;
    Coroutine asyncLoadScene;
    protected override void Awake()
    {
        base.Awake();
        scenes = new Dictionary<string, MonoScene>();
        AddScene(SceneKey.mainScene, new MainStageScene());
    }

    public void AddScene(string key, MonoScene monoScene)
    {
        if (scenes.TryGetValue(key, out MonoScene scene))
        {
            Debug.Log($"{key} is duplicate in scene");
            return;
        }
        scenes.Add(key, monoScene);
        Debug.Log(4);

    }

    public void LoadScene(string key)
    {
        if (asyncLoadScene != null) StopCoroutine(asyncLoadScene);

        if (scenes.TryGetValue(key, out MonoScene scene))
        {
            asyncLoadScene = StartCoroutine(AsyncLoadScene(key));
            return;
        }
        Debug.Log($"Not Find {key} in Objects");
    }

    IEnumerator AsyncLoadScene(string key)
    {
        if (nowScene != null)
        {
            nowScene.Release();
        }
        nowScene = scenes[key];
        Debug.Log(6);

        var operation = SceneManager.LoadSceneAsync(key);
        operation.allowSceneActivation = false;

        var loadHandlePrefab = nowScene.LoadPrefabs();
        var loadHandleSound = nowScene.LoadSounds();

        while (!loadHandlePrefab.IsDone || !loadHandlePrefab.IsDone)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }
        nowScene.Init();
    }
}
