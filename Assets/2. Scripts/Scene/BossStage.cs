using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BossStage : MonoScene
{
    public override void Init()
    {
        ObjectManager.Instance.AddObject("BossScene", Vector3.zero, Quaternion.identity);
        ObjectManager.Instance.AddObject("MainUI", Vector3.zero, Quaternion.identity);
        SoundManager.Instance.PlayBGMSound("BossBGM2");
    }

    public override AsyncOperationHandle LoadPrefabs()
    {
        return ObjectManager.Instance.LoadGameObject("Boss");
    }

    public override AsyncOperationHandle LoadSounds()
    {
        return SoundManager.Instance.LoadSound("Boss");
    }

    public override void Release()
    {
        SoundManager.Instance.Release();
        ObjectManager.Instance.Release();
    }
}
