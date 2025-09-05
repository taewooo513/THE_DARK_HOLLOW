using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ObjectGlitchReveal : MonoBehaviour
{
    [Header("Timing")]
    public float totalTime = 0.5f;   // 전체 연출 시간
    public int flickerCount = 2;     // 깜빡임 횟수

    [Header("Jitter")]
    public float jitterPos = 4f;     // 위치 흔들림 (UI 기준: px)
    public float jitterRot = 4f;     // Z 회전 흔들림 (deg)
    public float jitterScale = 0.06f;// 스케일 흔들림 비율

    CanvasGroup group;
    RectTransform rt;        // UI면 RectTransform 사용
    Transform tr;            // 비UI용 Transform
    Vector3 basePos3D;
    Quaternion baseRot;
    Vector3 baseScale;
    bool isUI;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        rt = GetComponent<RectTransform>();
        isUI = rt != null;
        tr = transform;

        if (isUI)
            basePos3D = rt.anchoredPosition3D;
        else
            basePos3D = tr.localPosition;

        baseRot = tr.localRotation;
        baseScale = tr.localScale;
    }

    public void InstantHide()
    {
        group.alpha = 0f;
        ResetTRS();
        gameObject.SetActive(true);
    }

    void ResetTRS()
    {
        if (isUI) rt.anchoredPosition3D = basePos3D;
        else tr.localPosition = basePos3D;

        tr.localRotation = baseRot;
        tr.localScale = baseScale;
    }

    public IEnumerator Co_Reveal()
    {
        InstantHide();

        float step = Mathf.Max(0.01f, totalTime / Mathf.Max(1, flickerCount));
        for (int i = 0; i < flickerCount; i++)
        {
            // ON (랜덤 흔들림)
            group.alpha = 1f;

            Vector3 jPos = new Vector3(
                Random.Range(-jitterPos, jitterPos),
                Random.Range(-jitterPos, jitterPos),
                0f
            );
            float jRot = Random.Range(-jitterRot, jitterRot);
            float jScale = 1f + Random.Range(-jitterScale, jitterScale);

            if (isUI) rt.anchoredPosition3D = basePos3D + jPos;
            else tr.localPosition = basePos3D + jPos;

            tr.localRotation = Quaternion.Euler(0, 0, jRot) * baseRot;
            tr.localScale = baseScale * jScale;

            yield return new WaitForSeconds(step * 0.5f);

            // OFF (원위치/투명)
            group.alpha = 0f;
            ResetTRS();
            yield return new WaitForSeconds(step * 0.5f);
        }

        // 마지막에 안정적으로 표시
        group.alpha = 1f;
        ResetTRS();
    }
}
