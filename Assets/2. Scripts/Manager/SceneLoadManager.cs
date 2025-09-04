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
    public const string bossScene = "Lee.BossTestScene";
}
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    Dictionary<string, MonoScene> scenes;

    MonoScene nowScene;
    Coroutine asyncLoadScene;
    private GameObject fadeObject;

    protected override void Awake()
    {
        base.Awake();
        scenes = new Dictionary<string, MonoScene>();
        AddScene(SceneKey.mainScene, new MainStageScene());
        AddScene(SceneKey.bossScene, new BossStage());
    }

    public void AddScene(string key, MonoScene monoScene)
    {
        if (scenes.TryGetValue(key, out MonoScene scene))
        {
            Debug.Log($"{key} is duplicate in scene");
            return;
        }
        scenes.Add(key, monoScene);
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
        UIManager.Instance.FindUIManager<FadeInOutManager>("FadeManager").alpha = 0;
        while (true)
        {
            if (UIManager.Instance.FindUIManager<FadeInOutManager>("FadeManager").FadeIn() == true)
            {
                break;
            }
            yield return null;
        }

        if (nowScene != null)
        {
            nowScene.Release();
        }
        nowScene = scenes[key];

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

        while (true)
        {
            if (UIManager.Instance.FindUIManager<FadeInOutManager>("FadeManager").FadeOut() == true)
            {
                break;
            }
            yield return null;
        }
    }
}
