using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        vcam.Follow = go.transform;
        Debug.Log("SetPlayerCamera" + go.name);
    }
}
