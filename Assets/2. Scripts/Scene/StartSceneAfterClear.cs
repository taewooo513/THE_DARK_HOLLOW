using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartSceneAfterClear : MonoScene
{
    public override void Init()
    {
        ObjectManager.Instance.AddObject("StartSceneAfterClear", Vector3.zero, Quaternion.identity);
    }

    public override AsyncOperationHandle LoadPrefabs()
    {
        return ObjectManager.Instance.LoadGameObject("StartSceneAfterClear");
    }

    public override AsyncOperationHandle LoadSounds()
    {
        return SoundManager.Instance.LoadSound("StartSceneAfterClear");
    }

    public override void OnPadeOut()
    {
    }

    public override void Release()
    {
        SoundManager.Instance.Release();
        ObjectManager.Instance.Release();
    }
}
