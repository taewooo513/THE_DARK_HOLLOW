using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StartScene : MonoScene
{
    public override void Init()
    {
        ObjectManager.Instance.AddObject("StartScene", Vector3.zero, Quaternion.identity);
    }

    public override AsyncOperationHandle LoadPrefabs()
    {
        return ObjectManager.Instance.LoadGameObject("StartScene");
    }

    public override AsyncOperationHandle LoadSounds()
    {
        return SoundManager.Instance.LoadSound("StartScene");
    }

    public override void Release()
    {
        SoundManager.Instance.Release();
        ObjectManager.Instance.Release();
    }
}
