using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixerGroup bgmGroup; // 인스펙터에서 StartAudioMixer/BGM 연결
    public AudioMixerGroup seGroup;  // 인스펙터에서 StartAudioMixer/SE 연결

    private Dictionary<string, AudioClip> sounds;

    private AudioSource bgmAudio;
    private AudioSource efxAudio;
    public float allVolume { get; private set; } = 1f;
    public float efxVolume { get; private set; } = 1f;
    public float bgmVolume { get; private set; } = 1f;
    AsyncOperationHandle soundHandle;
    void Start()
    {
        if (transform.TryGetComponent(out AudioSource source))
        {
            var sources = GetComponents<AudioSource>();
            if (sources.Length < 2)
            {
                bgmAudio = gameObject.AddComponent<AudioSource>();
                efxAudio = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                bgmAudio = sources[0];
                efxAudio = sources[1];
            }
        }
        else
        {
            bgmAudio = transform.AddComponent<AudioSource>();
            efxAudio = transform.AddComponent<AudioSource>();
        }

        bgmAudio.loop = true;

        if (bgmGroup != null) bgmAudio.outputAudioMixerGroup = bgmGroup;
        if (seGroup != null) efxAudio.outputAudioMixerGroup = seGroup;

        sounds = new Dictionary<string, AudioClip>();

    }

    public override void Release()
    {
        sounds.Clear();
        Addressables.Release(soundHandle);
        Debug.Log("test");
    }
    public void SetEFXVolume(float volume)
    {
        efxVolume = volume;
        efxAudio.volume = efxVolume * allVolume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmAudio.volume = bgmVolume * allVolume;
    }

    public void SetAllVolume(float volume)
    {
        allVolume = volume;
        SetBGMVolume(bgmVolume);
        SetBGMVolume(efxVolume);
    }

    public void InsertSound(string key, AudioClip audioClip)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            Debug.Log($"{key} is duplicate in sounds");
            return;
        }
        sounds.Add(key, clip);
    }
    public void PlayBGMSound(string key)
    {
        if (sounds == null)
        {
            Debug.LogWarning("Sounds is null");
            return;
        }

        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            bgmAudio.clip = clip;
            bgmAudio.Play();
        }
        else
        {
            Debug.LogWarning("{key} sound not assigned in SoundManager.");
        }
    }
    public void PlayEFXSound(string key)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            efxAudio.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("{key} sound not assigned in SoundManager.");
        }
    }
    public AsyncOperationHandle LoadSound(string label)
    {
        var handle = ResourceManager.Instance.LoadResource<AudioClip>(label, clip =>
        {
            sounds.Add(clip.name, clip);
        });
        handle.Completed += OnLoadCompleteObject;
        soundHandle = handle;
        return soundHandle;
    }

    private void OnLoadCompleteObject<T>(AsyncOperationHandle<IList<T>> handle) where T : UnityEngine.Object
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Load Sounds Succeeded");
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Load Sounds Failed");
        }
    }
}
