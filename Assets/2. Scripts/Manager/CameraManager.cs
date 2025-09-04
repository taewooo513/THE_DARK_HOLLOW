using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : Singleton<CameraManager>
{
    Camera mainCamera;
    Cinemachine.CinemachineVirtualCamera cinemachine;
    SpriteRenderer spriteRenderer;
    CinemachineBasicMultiChannelPerlin noise;
    public CompositeCollider2D collider2D;
    Cinemachine.CinemachineConfiner2D confiner2D;
    Coroutine shackCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        Init();
    }
    public override void Init()
    {
        mainCamera = Camera.main;
        if (mainCamera.TryGetComponent(out Cinemachine.CinemachineVirtualCamera val))
        {
            cinemachine = val;
            cinemachine.Follow = CharacterManager.Instance.PlayerStat.transform;

            noise = val.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (mainCamera.TryGetComponent(out Cinemachine.CinemachineConfiner2D val2))
            {
                if (collider2D != null)
                {
                    confiner2D = val2;
                    confiner2D.m_BoundingShape2D = collider2D;
                }
            }
        }
    }

    // ÁøÆø, ºóµµ, ±â°£
    public void CameraShack(float amplitude, float frequency, float duration)
    {
        shackCoroutine = StartCoroutine(CameraShacking(amplitude, frequency, duration));
    }

    IEnumerator CameraShacking(float amplitude, float frequency, float duration) // 
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        yield return new WaitForSeconds(duration);

        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }

    public override void Release()
    {
        cinemachine = null;
        noise = null;
        collider2D = null;
        confiner2D = null;
        shackCoroutine = null;
        StopCoroutine("shackCoroutine");
    }
}
