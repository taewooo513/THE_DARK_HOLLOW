using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class MonoScene
{
    public int stage;
    public abstract AsyncOperationHandle LoadPrefabs();
    public abstract AsyncOperationHandle LoadSounds();

    public abstract void Init();

    public abstract void Release();
}
