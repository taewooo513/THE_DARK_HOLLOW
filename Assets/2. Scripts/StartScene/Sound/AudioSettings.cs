using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    void Start()
    {
        // 저장된 값 불러오기 (기본값은 0dB)
        masterSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM", 0.75f);
        seSlider.value = PlayerPrefs.GetFloat("SE", 0.75f);

        SetMasterVolume(masterSlider.value);
        SetBGMVolume(bgmSlider.value);
        SetSEVolume(seSlider.value);

        // 슬라이더 이벤트 연결
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        seSlider.onValueChanged.AddListener(SetSEVolume);
    }

    private float ToDb_Gamma(float value, float minDb = -80f, float gamma = 0.4f)
    {
        value = Mathf.Clamp01(value);
        float t = Mathf.Pow(value, gamma);      // 커브 적용
        return Mathf.Lerp(minDb, 0f, t);        // dB 선형 보간
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", ToDb_Gamma(value, -80f, 0.4f)); // dB 변환
        PlayerPrefs.SetFloat("Master", value);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", ToDb_Gamma(value, -80f, 0.4f));
        PlayerPrefs.SetFloat("BGM", value);
    }

    public void SetSEVolume(float value)
    {
        audioMixer.SetFloat("SE", ToDb_Gamma(value, -80f, 0.4f));
        PlayerPrefs.SetFloat("SE", value);
    }
}
