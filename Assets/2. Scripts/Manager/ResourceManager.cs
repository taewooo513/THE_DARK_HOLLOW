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
    //사용법 각 사운드 오브젝트매니저에서 Insert함수를 사용하여 로드를시킴
    // 1. 씬별로 필요한 리소스들 라벨로 관리
    // 2. 씬에서 로드할거임 
    // 3. AddObject PlaySound key값을 프리팹과 맞춰야함 
    // 4. Release꼭해줘야 메모리에서 내려감 안그럼 메모리누수 씬에서 해줄거임
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ObjectManager.Instance.AddObject("Stage1", Vector3.zero, Quaternion.identity);
        }
    }
    public override void Release()
    {
    }

    public AsyncOperationHandle<IList<T>> LoadResource<T>(string label, Action<T> callback) where T : UnityEngine.Object
    {
        return Addressables.LoadAssetsAsync<T>(label, callback);
    }
}
