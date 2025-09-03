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

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20); // dB 변환
        PlayerPrefs.SetFloat("Master", value);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGM", value);
    }

    public void SetSEVolume(float value)
    {
        audioMixer.SetFloat("SE", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SE", value);
    }
}
