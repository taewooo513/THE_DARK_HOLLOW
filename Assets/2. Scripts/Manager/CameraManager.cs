using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Init()
    {
        mainCamera = Camera.main;
    }

    public void FollowCamear(Vector3 pos)
    {
        mainCamera.transform.position = pos;
    }
}
