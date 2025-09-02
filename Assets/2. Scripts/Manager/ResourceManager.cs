using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : Singleton<ResourceManager>
{
    AsyncOperationHandle<IList<GameObject>> objsHandle;
    AsyncOperationHandle<IList<AudioClip>> soundsHandle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ObjectManager.Instance.AddObject("Stage1", Vector3.zero, Quaternion.identity);
        }
    }
    public override void Release()
    {
        if (objsHandle.IsValid())
            Addressables.Release(objsHandle);
        if (soundsHandle.IsValid())
            Addressables.Release(soundsHandle);
    }

    public AsyncOperationHandle<IList<T>> LoadResource<T>(string label, Action<T> callback) where T : UnityEngine.Object
    {
        return Addressables.LoadAssetsAsync<T>(label, callback);
    }
}
