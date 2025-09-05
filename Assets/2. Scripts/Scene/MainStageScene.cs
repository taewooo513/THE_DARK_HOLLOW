using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MainStageScene : MonoScene // 테스트용입니다
{
    public override void Init()
    {
        ObjectManager.Instance.AddObject("Stage1Scene", Vector3.zero, Quaternion.identity);
        ObjectManager.Instance.AddObject("Stage2Scene", Vector3.zero, Quaternion.identity);

        ObjectManager.Instance.AddObject("MainUI", Vector3.zero, Quaternion.identity);
        SoundManager.Instance.PlayBGMSound("BossBGM1");
    }

    public override AsyncOperationHandle LoadPrefabs()
    {
        return ObjectManager.Instance.LoadGameObject("Stage1");
    }

    public override AsyncOperationHandle LoadSounds()
    {
        return SoundManager.Instance.LoadSound("Stage1");
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
