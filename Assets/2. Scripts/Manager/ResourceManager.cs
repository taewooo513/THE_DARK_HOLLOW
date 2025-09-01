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

    void Start()
    {
        LoadResource("Stage1");
    }
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

    public void LoadResource(string label)
    {
        Addressables.LoadAssetsAsync<GameObject>(label, obj =>
        {
            ObjectManager.Instance.InsertObject(obj.name, obj);
            Debug.Log(obj.name);
        }).Completed += OnLoadCompletePrefabs;
        Addressables.LoadAssetsAsync<AudioClip>(label, audioClip =>
        {
            SoundManager.Instance.InsertSound(audioClip.name, audioClip);
        }).Completed += OnLoadCompleteSounds;
    }

    private void OnLoadCompletePrefabs(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Load Prefabs Succeeded");
            objsHandle = handle;
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Load Prefabs Failed");
        }
    }

    private void OnLoadCompleteSounds(AsyncOperationHandle<IList<AudioClip>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Load Sounds Succeeded");
            soundsHandle = handle;
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Load Sounds Failed");
        }
    }
}
