using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BossStage : MonoScene
{
    public override void Init()
    {
        ObjectManager.Instance.AddObject("BossStage", Vector3.zero, Quaternion.identity);
        Debug.Log("te");
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
    }

}
